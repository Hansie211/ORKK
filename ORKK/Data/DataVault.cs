using System.Collections.ObjectModel;
using System.Linq;

namespace ORKK.Data {
    public static class DataVault {

        public static OrderVault Orders                         = new OrderVault();
        public static CableChecklistVault CableChecklists       = new CableChecklistVault();

        public static void FillVaults() {

            Orders.FillVaultFromDB();
            CableChecklists.FillVaultFromDB();
        }

        public static void SyncDBFromVaults( bool checkDirty = true ) {

            Orders.SyncDBFromVault( checkDirty );
            CableChecklists.SyncDBFromVault( checkDirty );
        }

        public static bool IsDirty() {

            return Orders.IsDirty() || CableChecklists.IsDirty();
        }

        public static OrderObject GetParentOrderOf( int cableChecklistId ) {

            return Orders.GetEntry( CableChecklists.GetEntry( cableChecklistId ).OrderID );
        }

        public static ObservableCollection<CableChecklistObject> GetChildCableChecklists( int orderId ) {

            return new ObservableCollection<CableChecklistObject>( CableChecklists.Entries.Where( x => x.OrderID == orderId ) );
        }
    }
}
