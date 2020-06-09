using ORKK.Data.Objects;
using System.Data;
using System.Windows.Data;

namespace ORKK.Data.Vaults
{
    [TableName("OrderTable")]
    public class OrderVault : BaseVault<OrderObject> {
        protected override void InitializePropList(){
            propList = new ColumnProperty[]
            {
                propID = new ColumnProperty( ObjectType, "ID", "Order_ID",  SqlDbType.Int),
                new ColumnProperty( ObjectType, "WorkInstruction", "Work_Instruction",  SqlDbType.NVarChar, 500),
                new ColumnProperty( ObjectType, "DateExecution", "Date_Execution",  SqlDbType.DateTime ),
                new ColumnProperty( ObjectType, "CableSupplier", "Cable_Supplier",  SqlDbType.NVarChar, 250 ),
                new ColumnProperty( ObjectType, "Observations", "Observations",  SqlDbType.NVarChar, 500 ),
                new ColumnProperty( ObjectType, "Signature", "Signature",  SqlDbType.VarBinary),
                new ColumnProperty( ObjectType, "HoursInCompany", "Hours_In_Company",  SqlDbType.Int ),
                new ColumnProperty( ObjectType, "Reasons", "Reasons",  SqlDbType.NVarChar, 500 ),
            };
        }
    }
}
