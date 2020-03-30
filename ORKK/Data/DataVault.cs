using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ORKK.Data {
    public static class DataVault {

        private static readonly string databaseFilename = System.IO.Path.GetFullPath( $@"{System.AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf" );
        private static readonly string connString = $@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ databaseFilename }";

        public static readonly SqlConnection connection = new SqlConnection( connString );

        static DataVault() {

            connection.Open();
        }

        public static void FillVaults() {
            OrderVault.FillVault();
            CableChecklistVault.FillVault();
        }

        public static OrderObject GetParentOrderOf( int cableChecklistId ) {
            return OrderVault.GetOrder( CableChecklistVault.GetCableChecklist( cableChecklistId ).OrderID );
        }

        public static IEnumerable<CableChecklistObject> GetChildCableChecklists( int orderId ) {
            return CableChecklistVault.GetCableChecklists().Where( x => x.OrderID == orderId );
        }
    }
}
