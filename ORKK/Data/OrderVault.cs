using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ORKK.Data
{
    public static class OrderVault
    {
        public static readonly List<int> OrderIDs = new List<int>();
        public static readonly List<int> RemovedIDs = new List<int>();

        private static readonly BindingList<OrderObject> Orders = new BindingList<OrderObject>();
        private static readonly SqlConnection Connection = new SqlConnection($@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }");

        public static int Count => Orders.Count;

        public static OrderObject GetOrder(int id)
        {
            return Orders.FirstOrDefault(x => x.ID == id);
        }

        public static void AddOrder(OrderObject order)
        {
            Orders.Add(order);
        }

        public static void RemoveOrder(int id)
        {
            Orders.Remove(GetOrder(id));
            RemovedIDs.Add(id);
        }

        public static BindingList<OrderObject> GetOrders()
        {
            return Orders;
        }

        public static void FillVaultFromDB()
        {
            Connection.Open();
            using (var command = new SqlCommand(@"SELECT * FROM OrderTable", Connection))
            {
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        OrderObject order = new OrderObject(
                            reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2), reader.GetString(3),
                            reader.GetString(4), reader[5], reader.GetInt32(6), reader.GetString(7));

                        Orders.Add(order);
                        OrderIDs.Add(order.ID);
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

            string orderString = @"INSERT INTO OrderTable (Work_Instruction, Date_Execution, Cable_Supplier, Observations, Signature, Hours_In_Company, Reasons) VALUES (@Work_Instruction, @Date_Execution, @Cable_Supplier, @Observations, NULL, @Hours_In_Company, @Reasons)";
            SqlCommand command = new SqlCommand(orderString, Connection);

            command.Parameters.Add(new SqlParameter("@Work_Instruction", SqlDbType.NVarChar, 500));
            command.Parameters.Add(new SqlParameter("@Date_Execution", SqlDbType.DateTime));
            command.Parameters.Add(new SqlParameter("@Cable_Supplier", SqlDbType.NVarChar, 250));
            command.Parameters.Add(new SqlParameter("@Observations", SqlDbType.NVarChar, 500));
            //command.Parameters.Add(new SqlParameter("@Signature", SqlDbType.Image));
            command.Parameters.Add(new SqlParameter("@Hours_In_Company", SqlDbType.Int));
            command.Parameters.Add(new SqlParameter("@Reasons", SqlDbType.NVarChar, 500));

            try
            {
                foreach (OrderObject order in Orders)
                {
                    if (!OrderIDs.Contains(order.ID))
                    {
                        command.Parameters["@Work_Instruction"].Value = order.WorkInstruction;
                        command.Parameters["@Date_Execution"].Value = order.DateExecution;
                        command.Parameters["@Cable_Supplier"].Value = order.CableSupplier;
                        command.Parameters["@Observations"].Value = order.Observations;
                        // command.Parameters["@Signature"].Value = order.Signature ?? DBNull.Value;
                        command.Parameters["@Hours_In_Company"].Value = order.HoursInCompany;
                        command.Parameters["@Reasons"].Value = order.Reasons;
                        command.ExecuteNonQuery();
                        OrderIDs.Add(order.ID);
                    }
                    else
                    {
                        UpdateTable(order);
                    }
                }

                foreach (int id in RemovedIDs)
                {
                    command = new SqlCommand(@"DELETE FROM OrderTable WHERE Order_ID = @Order_ID", Connection);
                    command.Parameters.AddWithValue("@Order_ID", id);
                    command.ExecuteNonQuery();
                    RemovedIDs.Remove(id);
                }
            }
            finally
            {
                Connection.Close();
            }
        }

        private static void UpdateTable(OrderObject order)
        {
            string orderString = @"UPDATE OrderTable SET Work_Instruction = @Work_Instruction, Date_Execution = @Date_Execution, Cable_Supplier = @Cable_Supplier, Observations = @Observations, Signature = NULL, Hours_In_Company = @Hours_In_Company, Reasons = @Reasons WHERE Order_ID = @Order_ID";
            using (var command = new SqlCommand(orderString, Connection))
            {
                command.Parameters.AddWithValue("@Work_Instruction", order.WorkInstruction);
                command.Parameters.AddWithValue("@Date_Execution", order.DateExecution);
                command.Parameters.AddWithValue("@Cable_Supplier", order.CableSupplier);
                command.Parameters.AddWithValue("@Observations", order.Observations);
                //command.Parameters.Add("@Signature", SqlDbType.Image).Value = order.Signature ?? DBNull.Value;
                command.Parameters.AddWithValue("@Hours_In_Company", order.HoursInCompany);
                command.Parameters.AddWithValue("@Reasons", order.Reasons);
                command.Parameters.AddWithValue("@Order_ID", order.ID);
                command.ExecuteNonQuery();
            }
        }

        public static int GetLastIDFromDB()
        {
            Connection.Open();

            string orderString = "SELECT IDENT_CURRENT ('OrderTable')";
            using (var command = new SqlCommand(orderString, Connection))
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
}
