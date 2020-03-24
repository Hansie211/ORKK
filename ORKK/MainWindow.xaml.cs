using ORKK.Dummy;
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

namespace ORKK {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged( string propertyName = null ) {

            if ( PropertyChanged == null ) {
                return;
            }

            PropertyChanged.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        private ICollection<Cablechecklist> _Checklists = new ObservableCollection<Cablechecklist>();
        public ICollection<Cablechecklist> Checklists {
            get { return _Checklists; }
            set {
                _Checklists = value;
                OnPropertyChanged( "Checklists" );
            }
        }

        private ICollection<Order> _OrderList = new ObservableCollection<Order>();
        public ICollection<Order> OrderList {
            get { return _OrderList; }
            set {
                _OrderList = value;
                OnPropertyChanged( "OrderList" );
            }
        }

        private Order _ActiveOrder = null;
        public Order ActiveOrder {
            get { return _ActiveOrder; }
            set {
                _ActiveOrder = value;
                OnPropertyChanged( "ActiveOrder" );
            }
        }

        public bool AnyOrders { get { return OrderList.Count > 0; } }

        public MainWindow() {

            this.DataContext = this;

            Cablechecklist cablechecklist;

            cablechecklist = Cablechecklist.Next();
            cablechecklist.OrderID      = 100;
            cablechecklist.rupture_6d   = 10;

            Checklists.Add( cablechecklist );

            cablechecklist = Cablechecklist.Next();
            cablechecklist.OrderID      = 101;
            cablechecklist.rupture_6d   = 11;

            Checklists.Add( cablechecklist );

            InitializeComponent();


        }

        private void CloseWindow_Click( object sender, RoutedEventArgs e ) {

            Close();
        }

        private void NewChecklist_Click( object sender, RoutedEventArgs e ) {


            Checklists.Add( new Cablechecklist() );
        }

        private void DeleteChecklist_Click( object sender, RoutedEventArgs e ) {

        }

        int x = 0;

        private void NewOrder_Click( object sender, RoutedEventArgs e ) {

            ActiveOrder = new Order() { ID = x++, comment = "comment" };

            OrderList.Add( ActiveOrder );
            OnPropertyChanged( "AnyOrders" );
        }

        private void SelectOrder_Click( object sender, RoutedEventArgs e ) {

            Order order = (Order)((MenuItem)sender).Header;
            Console.WriteLine( order );

            OnPropertyChanged( "OrderList" );
        }

        private void DeleteOrder_Click( object sender, RoutedEventArgs e ) {

        }

        private void Save_Click( object sender, RoutedEventArgs e ) {

        }

    }
}
