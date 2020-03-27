using ORKK.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ORKK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<CableChecklistObject> Checklists
        {
            get
            {
                if (ActiveOrder == null)
                {
                    return null;
                }

                return DataVault.GetChildCableChecklists(ActiveOrder.ID);
            }
        }

        public CableChecklistObject SelectedChecklist;

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
                OnPropertyChanged("ActiveOrder");
                OnPropertyChanged("Checklists");
            }
        }

        public IList<Damage> DamageTypes
        {
            get
            {
                return Enum.GetValues(typeof(Damage)).Cast<Damage>().ToArray();
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

            CableChecklistObject checklist = new CableChecklistObject(cableID, ActiveOrder.ID, 0, 0, 0, 0, 0, 0, 0, 0);
            cableID++;
            CableChecklistVault.AddCableChecklist(checklist);
            OnPropertyChanged("Checklists");
        }

        private void DeleteChecklist_Click(object sender, RoutedEventArgs e)
        {
            CableChecklistVault.RemoveCableChecklist(SelectedChecklist.ID);
            OnPropertyChanged("Checklists");
        }

        int orderID = 15;
        int cableID = 20;

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderObject order = new OrderObject(orderID, string.Empty, DateTime.Now, string.Empty, string.Empty, null, 0, string.Empty);
            orderID++;
            OrderVault.AddOrder(order);
            ActiveOrder = order;
        }

        private void SelectOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderObject order = (OrderObject)((MenuItem)sender).Header;
            ActiveOrder = order;
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderObject order = ActiveOrder;
            ActiveOrder = null;
            OrderVault.RemoveOrder(order.ID);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
