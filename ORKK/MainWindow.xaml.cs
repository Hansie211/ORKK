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

        //private ICollection<Cablechecklist> _Checklists = new ObservableCollection<Cablechecklist>();
        public IEnumerable<CableChecklistObject> Checklists {
            get {

                if ( ActiveOrder == null ) {
                    return null;
                }

                return DataVault.GetChildCableChecklists( ActiveOrder.ID );
            }
        }

        public ICollection<OrderObject> OrderList {
            get => OrderVault.GetOrders();
        }

        private OrderObject _ActiveOrder = null;
        public OrderObject ActiveOrder {
            get { return _ActiveOrder; }
            set {
                _ActiveOrder = value;
                OnPropertyChanged( "ActiveOrder" );
            }
        }

        public IList<Damage> DamageTypes {
            get {
                return Enum.GetValues( typeof( Damage ) ).Cast<Damage>().ToArray();
            }
        }

        public bool AnyOrders { get { return OrderList.Count > 0; } }

        public MainWindow() {

            this.DataContext = this;
            InitializeComponent();
        }

        private void CloseWindow_Click( object sender, RoutedEventArgs e ) {

            Close();
        }

        private void NewChecklist_Click( object sender, RoutedEventArgs e ) {


            //Checklists.Add( new Cablechecklist() );
        }

        private void DeleteChecklist_Click( object sender, RoutedEventArgs e ) {

        }

        int x = 0;

        private void NewOrder_Click( object sender, RoutedEventArgs e ) {

            //ActiveOrder = new OrderObject() { ID = x++, comment = "comment" };

            //OrderList.Add( ActiveOrder );
            //OnPropertyChanged( "AnyOrders" );
        }

        private void SelectOrder_Click( object sender, RoutedEventArgs e ) {

            OrderObject order = (OrderObject)((MenuItem)sender).Header;

            ActiveOrder = order;
            OnPropertyChanged( "ActiveOrder" );
            OnPropertyChanged( "Checklists" );
        }

        private void DeleteOrder_Click( object sender, RoutedEventArgs e ) {

        }

        private void Save_Click( object sender, RoutedEventArgs e ) {

        }

    }
}
