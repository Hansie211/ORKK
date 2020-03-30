using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ORKK.Data
{
    public static class OrderVault
    {
        private static readonly ObservableCollection<OrderObject> Orders = new ObservableCollection<OrderObject>();

        private static readonly List<int> OrderIDs = new List<int>();

        private static readonly string ConnString = $@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }";

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
        }

        public static ObservableCollection<OrderObject> GetOrders()
        {
            return Orders;
        }

        public static void FillVaultFromDB()
        {
            using (var conn = new SqlConnection(ConnString))
            {
                string orderString = @"SELECT * FROM OrderTable";
                using (var command = new SqlCommand(orderString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        OrderObject orderObject = new OrderObject(reader.GetInt32(0), reader.GetString(1),
                                                                  reader.GetDateTime(2), reader.GetString(3),
                                                                  reader.GetString(4), reader[5], reader.GetInt32(6),
                                                                  reader.GetString(7));

                        Orders.Add(orderObject);
                        OrderIDs.Add(orderObject.ID);
                    }
                }
            }
        }

        public static void FillDBFromVault()
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();

                string orderString = @"INSERT INTO OrderTable (Work_Instruction, Date_Execution, Cable_Supplier, Observations, Signature, Hours_In_Company, Reasons) VALUES (@Work_Instruction, @Date_Execution, @Cable_Supplier, @Observations, NULL, @Hours_In_Company, @Reasons)";
                using (var command = new SqlCommand(orderString, conn))
                {
                    command.Parameters.Add(new SqlParameter("@Work_Instruction", SqlDbType.NVarChar, 500));
                    command.Parameters.Add(new SqlParameter("@Date_Execution", SqlDbType.DateTime));
                    command.Parameters.Add(new SqlParameter("@Cable_Supplier", SqlDbType.NVarChar, 250));
                    command.Parameters.Add(new SqlParameter("@Observations", SqlDbType.NVarChar, 500));
                    //command.Parameters.Add(new SqlParameter("@Signature", SqlDbType.Image));
                    command.Parameters.Add(new SqlParameter("@Hours_In_Company", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@Reasons", SqlDbType.NVarChar, 500));

                    try
                    {
                        foreach (OrderObject order in GetOrders())
                        {
                            if (!OrderIDs.Contains(order.ID))
                            {
                                //command.Parameters["@Order_ID"] = order.ID;
                                command.Parameters["@Work_Instruction"].Value = order.WorkInstruction;
                                command.Parameters["@Date_Execution"].Value = order.DateExecution;
                                command.Parameters["@Cable_Supplier"].Value = order.CableSupplier;
                                command.Parameters["@Observations"].Value = order.Observations;
                                //command.Parameters["@Signature"].Value = order.Signature;
                                command.Parameters["@Hours_In_Company"].Value = order.HoursInCompany;
                                command.Parameters["@Reasons"].Value = order.Reasons;
                                command.ExecuteNonQuery();
                                OrderIDs.Add(order.ID);
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static bool InDatabase(int id) => OrderIDs.Contains(id);

        public static void UpdateTable(OrderObject order)
        {
            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();

                string orderString = @"UPDATE OrderTable SET Work_Instruction=@Work_Instruction, Date_Execution=@Date_Execution, Cable_Supplier=@Cable_Supplier, Observations=@Observations, Signature=@Signature, Hours_In_Company=@Hours_In_Company, Reasons=@Reasons WHERE Order_ID=@Order_ID";
                using (var command = new SqlCommand(orderString, conn))
                {
                    command.Parameters.AddWithValue("@Work_Instruction", order.WorkInstruction);
                    command.Parameters.AddWithValue("@Date_Execution", order.DateExecution);
                    command.Parameters.AddWithValue("@Cable_Supplier", order.CableSupplier);
                    command.Parameters.AddWithValue("@Observations", order.Observations);
                    command.Parameters.Add("@Signature", SqlDbType.Image).Value = order.Signature ?? DBNull.Value;
                    command.Parameters.AddWithValue("@Hours_In_Company", order.HoursInCompany);
                    command.Parameters.AddWithValue("@Reasons", order.Reasons);
                    command.Parameters.AddWithValue("@Order_ID", order.ID);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static int GetLastID()
        {
            string connString = $@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }";
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                string orderString = "SELECT IDENT_CURRENT ('OrderTable')";
                using (var command = new SqlCommand(orderString, conn))
                {
                    var ID = command.ExecuteScalar();
                    return ID is DBNull ? -1 : Convert.ToInt32(ID);
                }
            }
        }
    }

    public class OrderObject
    {
        public int ID { get; set; }

        public string WorkInstruction { get; set; }

        public DateTime DateExecution { get; set; }

        public string CableSupplier { get; set; }

        public string Observations { get; set; }

        public object Signature { get; set; }

        public int HoursInCompany { get; set; }

        public string Reasons { get; set; }

        private ObservableCollection<CableChecklistObject> CableChecklists;

        public OrderObject(int id, string workInstruction, DateTime dateExecution, string cableSupplier, string observations, object signature, int hoursInCompany, string reasons)
        {
            ID = id;
            WorkInstruction = workInstruction;
            DateExecution = dateExecution;
            CableSupplier = cableSupplier;
            Observations = observations;
            Signature = signature;
            HoursInCompany = hoursInCompany;
            Reasons = reasons;
            CableChecklists = new ObservableCollection<CableChecklistObject>();
        }

        public ObservableCollection<CableChecklistObject> GetCableChecklists()
        {
            return CableChecklists;
        }

        public override string ToString()
        {
            return $"Order { ID }";
        }
    }
}
