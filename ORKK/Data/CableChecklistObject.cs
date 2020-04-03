namespace ORKK.Data
{
    public class CableChecklistObject
    {
        public bool AnyPropertyChanged;

        private int rupture6D;
        private int rupture30D;
        private Damage damageOutside;
        private Damage damageRustCorrosion;
        private int reducedCableDiameter;
        private int positionMeasuringPoints;
        private Damage totalDamages;
        private int typeDamageRust;

        public int ID { get; set; }

        public int OrderID { get; set; }

        public int Rupture6D
        {
            get => rupture6D;
            set
            {
                rupture6D = value;
                AnyPropertyChanged = true;
            }
        }

        public int Rupture30D
        {
            get => rupture30D;
            set
            {
                rupture30D = value;
                AnyPropertyChanged = true;
            }
        }

        public Damage DamageOutside
        {
            get => damageOutside;
            set
            {
                damageOutside = value;
                AnyPropertyChanged = true;
            }
        }

        public Damage DamageRustCorrosion
        {
            get => damageRustCorrosion;
            set
            {
                damageRustCorrosion = value;
                AnyPropertyChanged = true;
            }
        }

        public int ReducedCableDiameter
        {
            get => reducedCableDiameter;
            set
            {
                reducedCableDiameter = value;
                AnyPropertyChanged = true;
            }
        }

        public int PositionMeasuringPoints
        {
            get => positionMeasuringPoints;
            set
            {
                positionMeasuringPoints = value;
                AnyPropertyChanged = true;
            }
        }

        public Damage TotalDamages
        {
            get => totalDamages;
            set
            {
                totalDamages = value;
                AnyPropertyChanged = true;
            }
        }

        public int TypeDamageRust
        {
            get => typeDamageRust;
            set
            {
                typeDamageRust = value;
                AnyPropertyChanged = true;
            }
        }

        public CableChecklistObject(int id, int orderId, int rupture6D, int rupture30D, int damageOutside, int damageRustCorrosion, int reducedCableDiamater, int positionMeasuringPoints, int totalDamages, int typeDamageRust)
        {
            ID = id;
            OrderID = orderId;
            Rupture6D = rupture6D;
            Rupture30D = rupture30D;
            DamageOutside = (Damage)damageOutside;
            DamageRustCorrosion = (Damage)damageRustCorrosion;
            ReducedCableDiameter = reducedCableDiamater;
            PositionMeasuringPoints = positionMeasuringPoints;
            TotalDamages = (Damage)totalDamages;
            TypeDamageRust = typeDamageRust;
            AnyPropertyChanged = false;
        }
    }
}
