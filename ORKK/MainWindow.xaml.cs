using ORKK.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
          
        private int orderID = OrderVault.GetLastIDFromDB() + 1;
        private int cableID = CableChecklistVault.GetLastIDFromDB() + 1;

        private OrderObject activeOrder = null;
        private CableChecklistObject activeCableChecklist = null;

        public ObservableCollection<OrderObject> OrderList
        {
            get => OrderVault.GetOrders();
        }

        public OrderObject ActiveOrder
        {
            get => activeOrder;
            set
            {
                activeOrder = value;
                ActiveCableChecklist = null;
                OnPropertyChanged("ActiveOrder");
                OnPropertyChanged("Checklists");
            }
        }

        public ObservableCollection<CableChecklistObject> Checklists
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

        public bool AnyOrders
        {
            get
            {
                return OrderList.Any();
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewChecklist_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveOrder is null)
            {
                return;
            }

            if (cableID != -1)
            {
                CableChecklistObject checklist = new CableChecklistObject(cableID, ActiveOrder.ID, 0, 0, 0, 0, 0, 0, 0, 0)
                {
                    AnyPropertyChanged = false
                };

                CableChecklistVault.AddCableChecklist(checklist);
                cableID++;
                OnPropertyChanged("Checklists");
            }
        }

        private void DeleteChecklist_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveCableChecklist is null)
            {
                return;
            }

            CableChecklistVault.RemoveCableChecklist(ActiveCableChecklist.ID);
            OnPropertyChanged("Checklists");
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            if (orderID != -1)
            {
                OrderObject order = new OrderObject(orderID, string.Empty, DateTime.Now, string.Empty, string.Empty, 1, 0, string.Empty);
                OrderVault.AddOrder(order);
                ActiveOrder = order;
                orderID++;
            }
        }

        private void SelectOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderObject order = (OrderObject)((MenuItem)sender).Header;
            ActiveOrder = order;
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveOrder is null)
            {
                return;
            }

            OrderVault.RemoveOrder(ActiveOrder.ID);
            ActiveOrder = null;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            OrderVault.SyncDBFromVault();
            CableChecklistVault.SyncDBFromVault();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            bool added = OrderVault.GetOrders().Where(x => !OrderVault.OrderIDs.Contains(x.ID)).Any() || CableChecklistVault.GetCableChecklists().Where(x => !CableChecklistVault.CableChecklistIDs.Contains(x.ID)).Any();
            bool removed = OrderVault.RemovedIDs.Any() || CableChecklistVault.RemovedIDs.Any();
            bool changed = OrderVault.GetOrders().Where(x => x.AnyPropertyChanged).Any() || CableChecklistVault.GetCableChecklists().Where(x => x.AnyPropertyChanged).Any();
            if (added || removed || changed)
            {
                switch (MessageBox.Show("Wijzigingen opslaan?", "Let op!", MessageBoxButton.YesNo))
                {
                    case MessageBoxResult.Yes:
                        OrderVault.SyncDBFromVault();
                        CableChecklistVault.SyncDBFromVault();
                        return;

                    case MessageBoxResult.None:
                    case MessageBoxResult.No:
                        return;
                }
            }
        }
    }
}
