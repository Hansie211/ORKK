using System.Data;

namespace ORKK.Data
{
    [TableName("CableChecklistTable")]
    public class CableChecklistVault : BaseVault<CableChecklistObject>
    {
        protected override void InitializePropList()
        {
            propList = new ColumnProperty[]
            {
                propID = new ColumnProperty(ObjectType, "ID", "Cable_ID",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "OrderID", "FK_Order_ID",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "Rupture6D", "Rupture_6D",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "Rupture30D", "Rupture_30D",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "DamageOutside", "Damage_Outsides",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "DamageRustCorrosion", "Damage_Rust_Corrosion",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "ReducedCableDiameter", "Reduced_Cable_Diameter",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "PositionMeasuringPoints", "Position_Measuring_Points",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "TotalDamages", "Total_Damages",  SqlDbType.Int),
                new ColumnProperty(ObjectType, "TypeDamageRust", "Type_Damage_Rust",  SqlDbType.Int),
            };
        }
    }
}
