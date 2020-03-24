using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ORKK {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=D:\VS Projects\ORKK\ORKK\Main.mdf";
            
            using (var conn = new SqlConnection(connString))
            {
                string sqlString = @"SELECT Work_Instruction, Date_Execution FROM OrderTable";
                using (var command = new SqlCommand(sqlString, conn))
                {
                    conn.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["Work_Instruction"]);
                        Console.WriteLine(reader["Date_Execution"]);
                    }
                }
            }
        }
    }
}
