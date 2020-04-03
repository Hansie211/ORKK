using System.Data;

namespace ORKK.Data
{
    [TableName("OrderTable")]
    public class OrderVault : BaseVault<OrderObject>
    {
        protected override void InitializePropList()
        {
            propList = new ColumnProperty[]
            {
                propID = new ColumnProperty( ObjectType, "ID", "Order_ID",  SqlDbType.Int),
                new ColumnProperty( ObjectType, "WorkInstruction", "Work_Instruction",  SqlDbType.NVarChar ),
                new ColumnProperty( ObjectType, "DateExecution", "Date_Execution",  SqlDbType.DateTime ),
                new ColumnProperty( ObjectType, "CableSupplier", "Cable_Supplier",  SqlDbType.NVarChar ),
                new ColumnProperty( ObjectType, "Observations", "Observations",  SqlDbType.NVarChar ),
                new ColumnProperty( ObjectType, "Signature", "Signature",  SqlDbType.Image ),
                new ColumnProperty( ObjectType, "HoursInCompany", "Hours_In_Company",  SqlDbType.Int ),
                new ColumnProperty( ObjectType, "Reasons", "Reasons",  SqlDbType.NVarChar ),
            };
        }
    }
}
