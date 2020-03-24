using ORKK.Dummy;
using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window {

        public List<Cablechecklist> Checklists { get; set; } = new List<Cablechecklist>();
        public MainWindow() {

            this.DataContext = this;

            Cablechecklist cablechecklist;

            cablechecklist = Cablechecklist.Next();
            cablechecklist.OrderID = 100;
            cablechecklist.rupture_6d = 10;

            Checklists.Add( cablechecklist );

            cablechecklist = Cablechecklist.Next();
            cablechecklist.OrderID = 101;
            cablechecklist.rupture_6d = 11;

            Checklists.Add( cablechecklist );

            InitializeComponent();
        }

        private void CloseWindow_Click( object sender, RoutedEventArgs e ) {

            Close();
        }

    }
}
