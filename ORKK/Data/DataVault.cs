using System.Collections.ObjectModel;
using System.Linq;

namespace ORKK.Data
{
    public static class DataVault
    {
        public static void FillVaults()
        {
            OrderVault.FillVault();
            CableChecklistVault.FillVault();
        }

        public static OrderObject GetParentOrderOf(int cableChecklistId)
        {
            return OrderVault.GetOrder(CableChecklistVault.GetCableChecklist(cableChecklistId).OrderID);
        }

        public static ObservableCollection<CableChecklistObject> GetChildCableChecklists(int orderId)
        {
            return new ObservableCollection<CableChecklistObject>(CableChecklistVault.GetCableChecklists().Where(x => x.OrderID == orderId));
        }
    }
}
