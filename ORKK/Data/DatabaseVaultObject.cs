using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ORKK.Data
{
    public abstract class DatabaseVaultObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; set; }

        public bool AnyPropertyChanged { get; set; }

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
