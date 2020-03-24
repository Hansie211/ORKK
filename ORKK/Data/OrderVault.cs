using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace ORKK.Data
{
    public static class OrderVault
    {
        private static readonly List<OrderObject> Orders = new List<OrderObject>();

        public static int Count => Orders.Count;

        public static OrderObject GetOrder(int id)
        {
            return Orders.FirstOrDefault(x => x.ID == id);
        }

        public static void RemoveOrder(int id)
        {
            Orders.Remove(GetOrder(id));
        }

        public static ReadOnlyCollection<OrderObject> GetOrders()
        {
            return Orders.AsReadOnly();
        }

        public static void FillVault()
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=D:\VS Projects\ORKK\ORKK\Main.mdf";
            using (var conn = new SqlConnection(connString))
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
                    }
                }
            }
        }
    }

    public class OrderObject
    {
        public int ID;
        public string WorkInstruction;
        public DateTime DateExecution;
        public string CableSupplier;
        public string Observations;
        public object Image;
        public int HoursInCompany;
        public string Reasons;

        private List<CableChecklistObject> CableChecklists;

        public OrderObject(int id, string workInstruction, DateTime dateExecution, string cableSupplier, string observations, object image, int hoursInCompany, string reasons)
        {
            ID = id;
            WorkInstruction = workInstruction;
            DateExecution = dateExecution;
            CableSupplier = cableSupplier;
            Observations = observations;
            Image = image;
            HoursInCompany = hoursInCompany;
            Reasons = reasons;
            CableChecklists = new List<CableChecklistObject>();
        }

        public List<CableChecklistObject> GetCableChecklists()
        {
            return CableChecklists;
        }
    }
}
