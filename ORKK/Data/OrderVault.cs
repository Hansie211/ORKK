﻿using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ORKK.Data
{
    public static class OrderVault
    {
        private static readonly ObservableCollection<OrderObject> Orders = new ObservableCollection<OrderObject>();

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
            string connString = $@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ Path.GetFullPath($@"{AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }";
            using (var conn = new SqlConnection(connString))
            {
                string orderString = @"SELECT * FROM OrderTable";
                using (var command = new SqlCommand(orderString, conn))
                {
                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    try {
                        while ( reader.Read() ) {

                            OrderObject orderObject = new OrderObject(reader.GetInt32(0), reader.GetString(1),
                                                                  reader.GetDateTime(2), reader.GetString(3),
                                                                  reader.GetString(4), reader[5], reader.GetInt32(6),
                                                                  reader.GetString(7));

                            Orders.Add( orderObject );
                        }

                    } finally {
                        reader.Close();
                    }
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

        public object Image { get; set; }

        public int HoursInCompany { get; set; }

        public string Reasons { get; set; }

        private ObservableCollection<CableChecklistObject> CableChecklists;

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
            CableChecklists = new ObservableCollection<CableChecklistObject>();
        }

        public ObservableCollection<CableChecklistObject> GetCableChecklists()
        {
            return CableChecklists;
        }

        public override string ToString() {

            if ( ID < 0 ) {

                return $"Order (unsaved)";
            }

            return $"Order { ID }";
        }

    }
}
