namespace ORKK.Data
{
    public class CableChecklistObject : DatabaseVaultObject
    {
        private int rupture6D;
        private int rupture30D;
        private Damage damageOutside;
        private Damage damageRustCorrosion;
        private int reducedCableDiameter;
        private int positionMeasuringPoints;
        private Damage totalDamages;
        private int typeDamageRust;

        public int OrderID { get; set; }

        public int Rupture6D
        {
            get => rupture6D;
            set => Set("Rupture6D", ref rupture6D, value);
        }

        public int Rupture30D
        {
            get => rupture30D;
            set => Set("Rupture30D", ref rupture30D, value);
        }

        public Damage DamageOutside
        {
            get => damageOutside;
            set => Set("DamageOutside", ref damageOutside, value);
        }

        public Damage DamageRustCorrosion
        {
            get => damageRustCorrosion;
            set => Set("DamageRustCorrosion", ref damageRustCorrosion, value);
        }

        public int ReducedCableDiameter
        {
            get => reducedCableDiameter;
            set => Set("ReducedCableDiameter", ref reducedCableDiameter, value);
        }

        public int PositionMeasuringPoints
        {
            get => positionMeasuringPoints;
            set => Set("PositionMeasuringPoints", ref positionMeasuringPoints, value);
        }

        public Damage TotalDamages
        {
            get => totalDamages;
            set => Set("TotalDamages", ref totalDamages, value);
        }

        public int TypeDamageRust
        {
            get => typeDamageRust;
            set => Set("TypeDamageRust", ref typeDamageRust, value);
        }

        public CableChecklistObject()
        {

        }

        public CableChecklistObject(int id, int orderId, int rupture6D, int rupture30D, Damage damageOutside, Damage damageRustCorrosion, int reducedCableDiamater, int positionMeasuringPoints, Damage totalDamages, int typeDamageRust)
        {
            ID = id;
            OrderID = orderId;
            Rupture6D = rupture6D;
            Rupture30D = rupture30D;
            DamageOutside = damageOutside;
            DamageRustCorrosion = damageRustCorrosion;
            ReducedCableDiameter = reducedCableDiamater;
            PositionMeasuringPoints = positionMeasuringPoints;
            TotalDamages = totalDamages;
            TypeDamageRust = typeDamageRust;
            AnyPropertyChanged = false;
        }
    }
}
