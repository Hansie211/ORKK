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

        private ObservableCollection<CableChecklistObject> _ChecklistList = null;
        public ObservableCollection<CableChecklistObject> ChecklistList {
            get { return _ChecklistList; }
            set {
                _ChecklistList = value;
                OnPropertyChanged( "ChecklistList" );
            }
        }

        public ObservableCollection<OrderObject> OrderList { get; } = new ObservableCollection<OrderObject>( OrderVault.GetOrders() );


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

        private bool SaveOrder() {

            if ( ActiveOrder == null ) {
                return true;
            }

            // Can save?
            // return true;

            return false;
        }

        private void SelectNewOrder( OrderObject orderObject ) {

            if ( orderObject.ID == ActiveOrder?.ID ) {

                return;
            }

            if ( !SaveOrder() ) {

                switch ( MessageBox.Show( "Fouten in order, kan niet opslaan. Wijzigingen verwerpen?", "Let op!", MessageBoxButton.YesNo ) ) {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.None:
                    case MessageBoxResult.No:
                        return;
                }
            }

            ActiveOrder = orderObject;

            ChecklistList = new ObservableCollection<CableChecklistObject>( DataVault.GetChildCableChecklists( ActiveOrder.ID ) );
            OnPropertyChanged( "OrderList" );
        }

        private void CloseWindow_Click( object sender, RoutedEventArgs e ) {

            Close();
        }

        private void NewChecklist_Click( object sender, RoutedEventArgs e ) {

            if ( ActiveOrder == null ) {

                return;
            }

            ChecklistList.Add( new CableChecklistObject( -1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ) );
            OnPropertyChanged( "ChecklistList" );
        }

        private void DeleteChecklist_Click( object sender, RoutedEventArgs e ) {

            OnPropertyChanged( "ChecklistList" );
        }

        private void NewOrder_Click( object sender, RoutedEventArgs e ) {

            OrderObject newOrder = new OrderObject( -1, null, DateTime.Now, null, null, null, 0, null );
            OrderList.Add( newOrder );

            if ( ActiveOrder == null ) {

                SelectNewOrder( newOrder );
            }

            OnPropertyChanged( "OrderList" );
            OnPropertyChanged( "AnyOrders" );
        }

        private void SelectOrder_Click( object sender, RoutedEventArgs e ) {

            MenuItem menuItem               = (MenuItem)sender;
            OrderObject menuItemOrder       = (OrderObject)menuItem.Header;

            if ( menuItemOrder.ID == ActiveOrder?.ID ) {

                return;
            }

            SelectNewOrder( menuItemOrder );
        }

        private void DeleteOrder_Click( object sender, RoutedEventArgs e ) {

            OnPropertyChanged( "OrderList" );
        }

        private void Save_Click( object sender, RoutedEventArgs e ) {

        }

        private void Canvas_ManipulationStarted( object sender, ManipulationStartedEventArgs e ) {

            Canvas canvas = (Canvas)sender;

            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush( Colors.Red );
            Canvas.SetLeft( el, e.ManipulationOrigin.X );
            Canvas.SetTop( el, e.ManipulationOrigin.Y );
            canvas.Children.Add( el );
        }
    }
}
