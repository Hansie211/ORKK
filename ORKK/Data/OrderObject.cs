using System;

namespace ORKK.Data
{
    public class OrderObject
    {
        public bool AnyPropertyChanged;

        private string workInstruction;
        private DateTime dateExecution;
        private string cableSupplier;
        private string observations;
        private object signature;
        private int hoursInCompany;
        private string reasons;

        public int ID { get; private set; }

        public string WorkInstruction
        {
            get => workInstruction;
            set
            {
                workInstruction = value;
                AnyPropertyChanged = true;
            }
        }

        public DateTime DateExecution
        {
            get => dateExecution;
            set
            {
                dateExecution = value;
                AnyPropertyChanged = true;
            }
        }

        public string CableSupplier
        {
            get => cableSupplier;
            set
            {
                cableSupplier = value;
                AnyPropertyChanged = true;
            }
        }

        public string Observations
        {
            get => observations;
            set
            {
                observations = value;
                AnyPropertyChanged = true;
            }
        }

        public object Signature
        {
            get => signature;
            set
            {
                signature = value;
                AnyPropertyChanged = true;
            }
        }

        public int HoursInCompany
        {
            get => hoursInCompany;
            set
            {
                hoursInCompany = value;
                AnyPropertyChanged = true;
            }
        }

        public string Reasons
        {
            get => reasons;
            set
            {
                reasons = value;
                AnyPropertyChanged = true;
            }
        }

        public OrderObject(int id, string workInstruction, DateTime dateExecution, string cableSupplier, string observations, object signature, int hoursInCompany, string reasons)
        {
            ID = id;
            WorkInstruction = workInstruction;
            DateExecution = dateExecution;
            CableSupplier = cableSupplier;
            Observations = observations;
            Signature = signature;
            HoursInCompany = hoursInCompany;
            Reasons = reasons;
        }

        public override string ToString()
        {
            return $"Order { ID }";
        }
    }

}
