using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ORKK.Data
{
    public class OrderObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isChecked;
        private string workInstruction;
        private DateTime dateExecution;
        private string cableSupplier;
        private string observations;
        private object signature;
        private int hoursInCompany;
        private string reasons;

        public bool AnyPropertyChanged { get; set; }

        public int ID { get; set; }

        public bool IsChecked 
        {
            get => isChecked;
            set => Set("IsChecked", ref isChecked, value);
        }

        public string WorkInstruction
        {
            get => workInstruction;
            set => Set("WorkInstruction", ref workInstruction, value);
        }

        public DateTime DateExecution
        {
            get => dateExecution;
            set => Set("DateExecution", ref dateExecution, value);
        }

        public string CableSupplier
        {
            get => cableSupplier;
            set => Set("CableSupplier", ref cableSupplier, value);
        }

        public string Observations
        {
            get => observations;
            set => Set("Observations", ref observations, value);
        }

        public object Signature
        {
            get => signature;
            set => Set("Signature", ref signature, value);
        }

        public int HoursInCompany
        {
            get => hoursInCompany;
            set => Set("HoursInCompany", ref hoursInCompany, value);
        }

        public string Reasons
        {
            get => reasons;
            set => Set("Reasons", ref reasons, value);
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
            AnyPropertyChanged = false;
        }

        public override string ToString()
        {
            return $"Order { ID }";
        }

        public void Set<T>(string propName, ref T oldValue, T newValue)
        {
            if (GetType().GetProperty(propName) == null)
            {
                throw new ArgumentException($"No property named '{propName}' on {GetType().FullName}");
            }

            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                oldValue = newValue;
                AnyPropertyChanged = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
