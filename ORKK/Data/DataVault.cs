using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ORKK.Data
{
    public static class DataVault
    {
        public static void FillVaults()
        {
            OrderVault.FillVaultFromDB();
            CableChecklistVault.FillVaultFromDB();
        }

        public static OrderObject GetParentOrderOf(int cableChecklistId)
        {
            return OrderVault.GetOrder(CableChecklistVault.GetCableChecklist(cableChecklistId).OrderID);
        }

        public static BindingList<CableChecklistObject> GetChildCableChecklists(int orderId)
        {
            return new BindingList<CableChecklistObject>(CableChecklistVault.GetCableChecklists().Where(x => x.OrderID == orderId).ToList());
        }
    }
}
