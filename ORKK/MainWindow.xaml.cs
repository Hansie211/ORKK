using ORKK.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ORKK {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        // private int orderID = OrderVault.GetLastIDFromDB() + 1;
        // private int cableID = CableChecklistVault.GetLastIDFromDB() + 1;

        private OrderObject activeOrder = null;
        private CableChecklistObject activeCableChecklist = null;

        public ICollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();

        public OrderObject ActiveOrder {
            get => activeOrder;
            set {
                activeOrder = value;
                ActiveCableChecklist = null;
                OnPropertyChanged( "ActiveOrder" );
                OnPropertyChanged( "Checklists" );
            }
        }

        public ObservableCollection<CableChecklistObject> Checklists {
            get => ActiveOrder == null ? null : DataVault.GetChildCableChecklists( ActiveOrder.ID );
        }

        public CableChecklistObject ActiveCableChecklist {
            get => activeCableChecklist;
            set {
                activeCableChecklist = value;
                OnPropertyChanged( "ActiveCableChecklist" );
            }
        }

        public IEnumerable<Damage> DamageTypes {
            get => Enum.GetValues( typeof( Damage ) ).Cast<Damage>();
        }

        public bool AnyOrders {
            get {
                return DataVault.Orders.Entries.Any();
            }
        }

        public MainWindow() {

            foreach ( OrderObject order in DataVault.Orders.Entries ) {

                MenuItems.Add( new MenuItem() { Header = order } );
            }

            DataContext = this;
            InitializeComponent();
        }

        protected void OnPropertyChanged( string propertyName = null ) {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        private void CloseWindow_Click( object sender, RoutedEventArgs e ) {
            Close();
        }

        private void NewChecklist_Click( object sender, RoutedEventArgs e ) {

            if ( ActiveOrder is null ) {
                return;
            }

            CableChecklistObject checklist = new CableChecklistObject( DataVault.CableChecklists.NextID(), ActiveOrder.ID, 0, 0, 0, 0, 0, 0, 0, 0);
            DataVault.CableChecklists.AddEntry( checklist );

            OnPropertyChanged( "Checklists" );
        }

        private void DeleteChecklist_Click( object sender, RoutedEventArgs e ) {

            if ( ActiveCableChecklist is null ) {
                return;
            }

            DataVault.CableChecklists.RemoveEntry( ActiveCableChecklist.ID );
            OnPropertyChanged( "Checklists" );
        }

        private void NewOrder_Click( object sender, RoutedEventArgs e ) {

            OrderObject order = new OrderObject( DataVault.Orders.NextID(), string.Empty, DateTime.Now, string.Empty, string.Empty, null, 0, string.Empty);
            DataVault.Orders.AddEntry( order );

            foreach ( MenuItem item in MenuItems ) {

                item.IsChecked = false;
            }
            MenuItems.Add( new MenuItem() { Header = order, IsChecked = true } );
            ActiveOrder = order;

            OnPropertyChanged( "MenuItems" );
        }

        private void SelectOrder_Click( object sender, RoutedEventArgs e ) {

            MenuItem menuItem           = (MenuItem)sender;
            OrderObject selectedOrder   = (OrderObject)menuItem.Header;

            ActiveOrder = selectedOrder;

            foreach ( MenuItem item in MenuItems ) {

                OrderObject order = (OrderObject)item.Header;
                item.IsChecked = ( order.ID == selectedOrder.ID );
            }
            OnPropertyChanged( "MenuItems" );
        }

        private void DeleteOrder_Click( object sender, RoutedEventArgs e ) {

            if ( ActiveOrder is null ) {
                return;
            }

            DataVault.Orders.RemoveEntry( ActiveOrder.ID );

            foreach ( MenuItem item in MenuItems ) {

                if ( ( (OrderObject)item.Header ).ID == ActiveOrder.ID ) {

                    MenuItems.Remove( item );
                    break;
                }
            }
            OnPropertyChanged( "MenuItems" );

            ActiveOrder = null;
        }

        private void Save_Click( object sender, RoutedEventArgs e ) {

            DataVault.SyncDBFromVaults();
        }

        private void Window_Closing( object sender, CancelEventArgs e ) {

            if ( !DataVault.IsDirty() ) {

                return;
            }

            switch ( MessageBox.Show( "Wijzigingen opslaan?", "Let op!", MessageBoxButton.YesNo ) ) {

                case MessageBoxResult.Yes:
                    DataVault.SyncDBFromVaults( false );
                    return;

                case MessageBoxResult.None:
                case MessageBoxResult.No:
                    return;
            }
        }
    }
}
