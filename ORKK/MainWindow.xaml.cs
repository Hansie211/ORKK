using ORKK.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        public ObservableCollection<CableChecklistObject> Checklists {
            get {
                if ( ActiveOrder == null ) {
                    return null;
                }

                return DataVault.GetChildCableChecklists( ActiveOrder.ID );
            }
        }

        public sealed class MenuOrderItem {

            public OrderObject Order { get; }
            public bool IsSelected {

                get {

                    Console.WriteLine( $"Update for { Order }" );

                    MainWindow mw = (MainWindow)App.Current.MainWindow;

                    if ( mw.ActiveOrder == null ) {
                        return false;
                    }

                    return Order.ID == mw.ActiveOrder.ID;
                }
                set {
                    return;
                }
            }

            public MenuOrderItem( OrderObject order ) {

                this.Order = order;
            }

            public override string ToString() {
                return $"Order { Order.ID }";
            }
        }

        private CableChecklistObject _ActiveCableChecklist = null;

        public CableChecklistObject ActiveCableChecklist {
            get => _ActiveCableChecklist;
            set {
                _ActiveCableChecklist = value;
                OnPropertyChanged( "ActiveChecklistItem" );
            }
        }

        public ObservableCollection<OrderObject> OrderList {
            get => OrderVault.GetOrders();
        }
        public ICollection<MenuOrderItem> MenuOrderList { get; set; } = new ObservableCollection<MenuOrderItem>();

        private OrderObject _ActiveOrder = null;

        public OrderObject ActiveOrder {
            get => _ActiveOrder;
            set {
                _ActiveOrder = value;
                OnPropertyChanged( "ActiveOrder" );
                OnPropertyChanged( "Checklists" );
            }
        }

        public IList<Damage> DamageTypes {
            get {
                return Enum.GetValues( typeof( Damage ) ).Cast<Damage>().ToArray();
            }
        }

        public bool AnyOrders {
            get {
                return OrderList.Any();
            }
        }

        public MainWindow() {

            foreach ( OrderObject order in OrderList ) {

                MenuOrderList.Add( new MenuOrderItem( order ) );
            }

            DataContext = this;
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

            OnPropertyChanged( "OrderList" );
        }

        private void CloseWindow_Click( object sender, RoutedEventArgs e ) {

            Close();
        }

        private void NewChecklist_Click( object sender, RoutedEventArgs e ) {
            if ( ActiveOrder is null ) {
                return;
            }

            CableChecklistObject checklist = new CableChecklistObject(cableID, ActiveOrder.ID, 0, 0, 0, 0, 0, 0, 0, 0);
            cableID++;
            CableChecklistVault.AddCableChecklist( checklist );
            OnPropertyChanged( "Checklists" );
        }

        private void DeleteChecklist_Click( object sender, RoutedEventArgs e ) {
            if ( ActiveCableChecklist is null ) {
                return;
            }

            CableChecklistVault.RemoveCableChecklist( ActiveCableChecklist.ID );
            OnPropertyChanged( "Checklists" );
        }

        int orderID = 15;
        int cableID = 10;

        private void NewOrder_Click( object sender, RoutedEventArgs e ) {
            OrderObject order = new OrderObject(orderID, string.Empty, DateTime.Now, string.Empty, string.Empty, null, 0, string.Empty);
            orderID++;
            OrderVault.AddOrder( order );
            ActiveOrder = order;

            MenuOrderList.Add( new MenuOrderItem( order ) );

            OnPropertyChanged( "OrderList" );
            OnPropertyChanged( "AnyOrders" );
            OnPropertyChanged( "MenuOrderList" );

        }

        private void SelectOrder_Click( object sender, RoutedEventArgs e ) {

            MenuItem menuItem           = (MenuItem)sender;
            MenuOrderItem menuOrderItem = (MenuOrderItem)menuItem.Header;

            OrderObject order   = menuOrderItem.Order;
            ActiveOrder         = order;

            OnPropertyChanged( "MenuOrderList" );
        }

        private void DeleteOrder_Click( object sender, RoutedEventArgs e ) {
            OrderObject order = ActiveOrder;
            ActiveOrder = null;
            OrderVault.RemoveOrder( order.ID );

            foreach ( MenuOrderItem item in MenuOrderList ) {

                if ( item.Order.ID == order.ID ) {

                    MenuOrderList.Remove( item );
                    break;
                }
            }

            OnPropertyChanged( "OrderList" );
            OnPropertyChanged( "AnyOrders" );
            OnPropertyChanged( "MenuOrderList" );
        }

        private void Save_Click( object sender, RoutedEventArgs e ) {

        }

        private void Canvas_ManipulationStarted( object sender, ManipulationStartedEventArgs e ) {
            Canvas canvas = (Canvas)sender;

            Ellipse el = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.Red)
            };

            Canvas.SetLeft( el, e.ManipulationOrigin.X );
            Canvas.SetTop( el, e.ManipulationOrigin.Y );
            canvas.Children.Add( el );
        }

        private void MainMenu_SubmenuOpened( object sender, RoutedEventArgs e ) {

            MenuItem menu = (MenuItem)sender;

            foreach ( MenuItem sub in menu.Items ) {

                sub.GetBindingExpression( MenuItem.IsCheckedProperty ).UpdateTarget();
            }

            //( (MenuItem)sender ).GetBindingExpression( MenuItem.IsCheckedProperty ).UpdateTarget();
        }
    }
}
