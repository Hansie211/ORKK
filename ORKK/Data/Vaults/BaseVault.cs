﻿using ORKK.Data.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ORKK.Data.Vaults
{
    public abstract class BaseVault<T> where T : DatabaseVaultObject
    {
        protected readonly Type ObjectType = typeof(T);
        protected ColumnProperty[] propList;
        protected ColumnProperty propID;

        private static readonly SqlConnection Connection = new SqlConnection($@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }");
        private readonly List<int> StoredIDs = new List<int>();
        private readonly List<int> RemovedIDs = new List<int>();
        private readonly string TableName;
        private readonly string InsertQuery;
        private readonly string UpdateQuery;
        private readonly string DeleteQuery;
        private int lastID;

        protected abstract void InitializePropList();

        public int NextID()
        {
            return ++lastID;
        }

        public BindingList<T> Entries { get; }

        public int Count 
        { 
            get => Entries.Count; 
        }

        public BaseVault()
        {
            Entries = new BindingList<T>();

            TableNameAttribute attribute = (TableNameAttribute)Attribute.GetCustomAttribute(GetType(), typeof(TableNameAttribute));
            if (attribute is null)
            {
                throw new MissingFieldException($"Missing 'TableName' attribute for { GetType() }.");
            }

            TableName = attribute.Value;

            InitializePropList();

            // Generate the queries
            IEnumerable<string> allColumns = propList.Where(x => x.ColumnName != propID.ColumnName).Select(x => x.ColumnName);
            IEnumerable<string> allValues = propList.Where(x => x.ColumnName != propID.ColumnName).Select(x => $"{ x.ColumnName } = @{ x.ColumnName }");

            InsertQuery = $@"INSERT INTO { TableName } ({ string.Join(", ", allColumns) }) VALUES (@{ string.Join(", @", allColumns)})";
            UpdateQuery = $@"UPDATE { TableName } SET { string.Join(", ", allValues) } WHERE { propID.ColumnName } = @{ propID.ColumnName }";
            DeleteQuery = $@"DELETE FROM { TableName } WHERE { propID.ColumnName } = @{ propID.ColumnName }";
        }

        public T GetEntry(int id)
        {
            return Entries.FirstOrDefault(x => x.ID == id);
        }

        public void AddEntry(T entry)
        {
            Entries.Add(entry);
        }

        public void RemoveEntry(int id)
        {
            Entries.Remove(GetEntry(id));
            RemovedIDs.Add(id);
        }

        public void FillVaultFromDB()
        {
            Connection.Open();
            try
            {
                using (var command = new SqlCommand($@"SELECT * FROM { TableName }", Connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        T entry = FromDatabase(reader);
                        Entries.Add(entry);
                        StoredIDs.Add(entry.ID);
                    }

                    reader.Close();
                }
            }
            finally
            {
                Connection.Close();
            }

            lastID = GetLastIDFromDB();
        }

        public void SyncDBFromVault(bool checkDirty = true)
        {
            if (checkDirty && (!IsDirty()))
            {
                return;
            }

            Connection.Open();
            try
            {
                foreach (T entry in Entries)
                {
                    if (!StoredIDs.Contains(entry.ID))
                    {
                        using (SqlCommand command = new SqlCommand(string.Format(InsertQuery, TableName), Connection))
                        {

                            FillCommand(command, entry);
                            command.ExecuteNonQuery();

                            StoredIDs.Add(entry.ID);
                        }
                    }
                    else
                    {
                        if (!entry.AnyPropertyChanged)
                        {

                            continue;
                        }

                        using (SqlCommand command = new SqlCommand(string.Format(UpdateQuery, TableName), Connection))
                        {

                            FillCommand(command, entry);
                            command.ExecuteNonQuery();
                        }
                    }

                    entry.AnyPropertyChanged = false;
                }

                foreach (int id in RemovedIDs)
                {
                    using (SqlCommand command = new SqlCommand(string.Format(DeleteQuery, TableName), Connection))
                    {

                        FillCommand(command, id);
                        command.ExecuteNonQuery();

                        RemovedIDs.Remove(id);
                    }
                }
            }
            finally
            {

                Connection.Close();
            }
        }
        public bool IsDirty()
        {

            if (RemovedIDs.Any())
            {
                return true;
            }

            foreach (T entry in Entries)
            {
                if (entry.AnyPropertyChanged)
                {
                    return true;
                }

                if (!StoredIDs.Contains(entry.ID))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetLastIDFromDB()
        {
            Connection.Open();
            try
            {

                using (var command = new SqlCommand($@"SELECT IDENT_CURRENT ('{ TableName }')", Connection))
                {
                    var ID = command.ExecuteScalar();
                    return ID is DBNull ? -1 : Convert.ToInt32(ID);
                }

            }
            finally
            {
                Connection.Close();
            }
        }

        private void FillCommand(SqlCommand command, T entry)
        {
            foreach (ColumnProperty property in propList)
            {
                object value = property.PropInfo.GetValue(entry);
                if (value is null)
                {
                    value = DBNull.Value;
                }

                SqlParameter param = command.Parameters.AddWithValue($"@{ property.ColumnName }", value);
                param.SqlDbType = property.DbType;
                param.Size = property.Size;
            }
        }

        private void FillCommand(SqlCommand command, int id)
        {
            command.Parameters.AddWithValue($"@{ propID.ColumnName }", id);
        }

        private T FromDatabase(SqlDataReader reader)
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            foreach (ColumnProperty property in propList)
            {
                object value = reader[property.ColumnName];
                if (value is DBNull)
                {
                    value = null;
                }

                property.PropInfo.SetValue(result, value);
            }

            result.AnyPropertyChanged = false;
            return result;
        }
    }
}
