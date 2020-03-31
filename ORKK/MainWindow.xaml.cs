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
        int orderID = OrderVault.GetLastIDFromDB() + 1;
        int cableID = CableChecklistVault.GetLastIDFromDB() + 1;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<CableChecklistObject> Checklists
        {
            get
            {
                return ActiveOrder == null ? null : DataVault.GetChildCableChecklists(ActiveOrder.ID);
            }
        }

        private CableChecklistObject _ActiveCableChecklist = null;

        public CableChecklistObject ActiveCableChecklist
        {
            get => _ActiveCableChecklist;
            set
            {
                _ActiveCableChecklist = value;
                OnPropertyChanged("ActiveCableChecklist");
            }
        }

        public ObservableCollection<OrderObject> OrderList
        {
            get => OrderVault.GetOrders();
        }

        private OrderObject _ActiveOrder = null;

        public OrderObject ActiveOrder
        {
            get => _ActiveOrder;
            set
            {
                _ActiveOrder = value;
                ActiveCableChecklist = null;
                OnPropertyChanged("ActiveOrder");
                OnPropertyChanged("Checklists");
            }
        }

        public IEnumerable<Damage> DamageTypes
        {
            get
            {
                return Enum.GetValues(typeof(Damage)).Cast<Damage>();
            }
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
                OrderObject order = new OrderObject(orderID, string.Empty, DateTime.Now, string.Empty, string.Empty, 1, 0, string.Empty)
                {
                    AnyPropertyChanged = false
                };

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
