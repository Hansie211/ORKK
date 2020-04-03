using ORKK.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ORKK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private OrderObject activeOrder = null;
        private CableChecklistObject activeCableChecklist = null;

        public BindingList<OrderObject> OrderList
        {
            get => DataVault.Orders.Entries;
        }

        public bool AnyOrders
        {
            get => DataVault.Orders.Entries.Any();
        }

        public OrderObject ActiveOrder
        {
            get => activeOrder;
            set
            {
                activeOrder = value;
                if (!(activeOrder is null))
                {
                    OrderList.ToList().ForEach(x => x.IsChecked = false);
                    activeOrder.IsChecked = true;
                }

                ActiveCableChecklist = null;
                OnPropertyChanged("ActiveOrder");
                OnPropertyChanged("Checklists");
            }
        }

        public BindingList<CableChecklistObject> Checklists
        {
            get => ActiveOrder == null ? null : DataVault.GetChildCableChecklists(ActiveOrder.ID);
        }

        public CableChecklistObject ActiveCableChecklist
        {
            get => activeCableChecklist;
            set
            {
                activeCableChecklist = value;
                OnPropertyChanged("ActiveCableChecklist");
            }
        }

        public IEnumerable<Damage> DamageTypes
        {
            get => Enum.GetValues(typeof(Damage)).Cast<Damage>();
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void NewChecklist_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveOrder is null)
            {
                return;
            }

            CableChecklistObject checklist = new CableChecklistObject(DataVault.CableChecklists.NextID(), ActiveOrder.ID, 0, 0, 0, 0, 0, 0, 0, 0);
            DataVault.CableChecklists.AddEntry(checklist);
            OnPropertyChanged("Checklists");
        }

        private void DeleteChecklist_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveCableChecklist is null)
            {
                return;
            }

            DataVault.CableChecklists.RemoveEntry(ActiveCableChecklist.ID);
            OnPropertyChanged("Checklists");
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderObject order = new OrderObject(DataVault.Orders.NextID(), string.Empty, DateTime.Now, string.Empty, string.Empty, null, 0, string.Empty);
            DataVault.Orders.AddEntry(order);
            ActiveOrder = order;
        }

        private void SelectOrder_Click(object sender, RoutedEventArgs e)
        {
            ActiveOrder = (OrderObject)(sender as MenuItem).Header;
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveOrder is null)
            {
                return;
            }

            DataVault.Orders.RemoveEntry(ActiveOrder.ID);
            ActiveOrder = null;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DataVault.SyncDBFromVaults();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataVault.IsDirty())
            {
                switch (MessageBox.Show("Er zijn onopgeslagen wijzigingen! Wilt u deze wijzigingen opslaan?", "Let op!", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Yes:
                        DataVault.SyncDBFromVaults(false);
                        return;

                    case MessageBoxResult.None:
                    case MessageBoxResult.No:
                        return;

                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
