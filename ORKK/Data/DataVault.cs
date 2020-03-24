using System.Collections.Generic;
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

        public static IEnumerable<CableChecklistObject> GetChildCableChecklists(int orderId)
        {
            return CableChecklistVault.GetCableChecklists().Where(x => x.OrderID == orderId);
        }
    }
}
