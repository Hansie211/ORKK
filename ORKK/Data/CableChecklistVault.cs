using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ORKK.Data
{
    public static class CableChecklistVault
    {
        public static readonly List<int> RemovedIDs = new List<int>();
        public static readonly List<int> CableChecklistIDs = new List<int>();

        private static readonly ObservableCollection<CableChecklistObject> CableChecklists = new ObservableCollection<CableChecklistObject>();
        private static readonly SqlConnection Connection = new SqlConnection($@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }");

        public static int Count => CableChecklists.Count;

        public static CableChecklistObject GetCableChecklist(int id)
        {
            return CableChecklists.FirstOrDefault(x => x.ID == id);
        }

        public static void AddCableChecklist(CableChecklistObject checklist)
        {
            CableChecklists.Add(checklist);
        }

        public static void RemoveCableChecklist(int id)
        {
            CableChecklists.Remove(GetCableChecklist(id));
            RemovedIDs.Add(id);
        }

        public static ObservableCollection<CableChecklistObject> GetCableChecklists()
        {
            return CableChecklists;
        }

        public static void FillVaultFromDB()
        {
            Connection.Open();
            using (var command = new SqlCommand(@"SELECT * FROM CableChecklistTable", Connection))
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CableChecklistObject cableChecklist = new CableChecklistObject(
                            reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3),
                            reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7),
                            reader.GetInt32(8), reader.GetInt32(9));

                        CableChecklists.Add(cableChecklist);
                        CableChecklistIDs.Add(cableChecklist.ID);
                    }

                    reader.Close();
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public static void SyncDBFromVault()
        {
            Connection.Open();

            string checklistString = @"INSERT INTO CableChecklistTable (FK_Order_ID, Rupture_6D, Rupture_30D, Damage_Outsides, Damage_Rust_Corrosion, Reduced_Cable_Diameter, Position_Measuring_Points, Total_Damages, Type_Damage_Rust) VALUES (@FK_Order_ID, @Rupture_6D, @Rupture_30D, @Damage_Outsides, @Damage_Rust_Corrosion, @Reduced_Cable_Diameter, @Position_Measuring_Points, @Total_Damages, @Type_Damage_Rust)";
            SqlCommand command = new SqlCommand(checklistString, Connection);

            command.Parameters.Add(new SqlParameter("@FK_Order_ID", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Rupture_6D", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Rupture_30D", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Damage_Outsides", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Damage_Rust_Corrosion", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Reduced_Cable_Diameter", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Position_Measuring_Points", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Total_Damages", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Type_Damage_Rust", SqlDbType.Int));

            try
            {
                foreach (CableChecklistObject cableChecklist in CableChecklists)
                {
                    if (!CableChecklistIDs.Contains(cableChecklist.ID))
                    {
                        command.Parameters["@FK_Order_ID"].Value = cableChecklist.OrderID;
                        command.Parameters["@Rupture_6D"].Value = cableChecklist.Rupture6D;
                        command.Parameters["@Rupture_30D"].Value = cableChecklist.Rupture30D;
                        command.Parameters["@Damage_Outsides"].Value = cableChecklist.DamageOutside;
                        command.Parameters["@Damage_Rust_Corrosion"].Value = cableChecklist.DamageRustCorrosion;
                        command.Parameters["@Reduced_Cable_Diameter"].Value = cableChecklist.ReducedCableDiameter;
                        command.Parameters["@Position_Measuring_Points"].Value = cableChecklist.PositionMeasuringPoints;
                        command.Parameters["@Total_Damages"].Value = cableChecklist.TotalDamages;
                        command.Parameters["@Type_Damage_Rust"].Value = cableChecklist.TypeDamageRust;
                        command.ExecuteNonQuery();
                        CableChecklistIDs.Add(cableChecklist.ID);
                    }
                    else
                    {
                        UpdateTable(cableChecklist);
                    }
                }

                foreach (int id in RemovedIDs)
                {
                    command = new SqlCommand(@"DELETE FROM CableChecklistTable WHERE Cable_ID = @Cable_ID", Connection);
                    command.Parameters.AddWithValue("@Cable_ID", id);
                    command.ExecuteNonQuery();
                    RemovedIDs.Remove(id);
                }
            }
            finally
            {
                Connection.Close();
            }
        }

        private static void UpdateTable(CableChecklistObject checklist)
        {
            string checklistString = @"UPDATE CableChecklistTable SET FK_Order_ID = @FK_Order_ID, Rupture_6D = @Rupture_6D, Rupture_30D = @Rupture_30D, Damage_Outsides = @Damage_Outsides, Damage_Rust_Corrosion = @Damage_Rust_Corrosion, Reduced_Cable_Diameter = @Reduced_Cable_Diameter, Position_Measuring_Points = @Position_Measuring_Points, Total_Damages = @Total_Damages, Type_Damage_Rust = @Type_Damage_Rust WHERE Cable_ID = @Cable_ID";
            using (var command = new SqlCommand(checklistString, Connection))
            {
                command.Parameters.AddWithValue("@FK_Order_ID", checklist.OrderID);
                command.Parameters.AddWithValue("@Rupture_6D", checklist.Rupture6D);
                command.Parameters.AddWithValue("@Rupture_30D", checklist.Rupture30D);
                command.Parameters.AddWithValue("@Damage_Outsides", checklist.DamageOutside);
                command.Parameters.AddWithValue("@Damage_Rust_Corrosion", checklist.DamageRustCorrosion);
                command.Parameters.AddWithValue("@Reduced_Cable_Diameter", checklist.ReducedCableDiameter);
                command.Parameters.AddWithValue("@Position_Measuring_Points", checklist.PositionMeasuringPoints);
                command.Parameters.AddWithValue("@Total_Damages", checklist.TotalDamages);
                command.Parameters.AddWithValue("@Type_Damage_Rust", checklist.TypeDamageRust);
                command.Parameters.AddWithValue("@Cable_ID", checklist.ID);
                command.ExecuteNonQuery();
            }
        }

        public static int GetLastIDFromDB()
        {
            Connection.Open();

            string checklistString = "SELECT IDENT_CURRENT ('CableChecklistTable')";
            using (var command = new SqlCommand(checklistString, Connection))
            {
                try
                {
                    var ID = command.ExecuteScalar();
                    return ID is DBNull ? -1 : Convert.ToInt32(ID);
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }

    public enum Damage
    {
        Geen,
        Laag,
        Gemiddeld,
        Hoog,
    }
}
