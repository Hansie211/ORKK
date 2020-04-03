using ORKK.Data;
using ORKK.Data.Objects;
using ORKK.Data.Vaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private Point signaturePoint = new Point();

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
                if (!(activeOrder is null))
                {
                    activeOrder.Signature = CanvasToByteArray(signatureCanvas);
                }

                activeOrder = value;
                if (!(activeOrder is null))
                {
                    ByteArrayToCanvas(signatureCanvas, activeOrder.Signature);
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
            OrderObject order = new OrderObject(DataVault.Orders.NextID(), string.Empty, DateTime.Now, string.Empty, string.Empty, new byte[10], 0, string.Empty);
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

        private void SignatureCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                signaturePoint = e.GetPosition((IInputElement)sender);
            }
        }

        private void SignatureCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = signaturePoint.X,
                    Y1 = signaturePoint.Y,
                    X2 = e.GetPosition((IInputElement)sender).X,
                    Y2 = e.GetPosition((IInputElement)sender).Y
                };

                signaturePoint = e.GetPosition((IInputElement)sender);
                ((Canvas)sender).Children.Add(line);
            }
        }

        private void SignatureCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                signaturePoint = new Point();
            }
        }

        private void SignatureCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                signaturePoint = e.GetPosition((IInputElement)sender);
            }
        }

        private static byte[] CanvasToByteArray(Canvas canvas)
        {
            IEnumerable<Line> lines = canvas.Children.OfType<Line>();
            List<LineObject> table = new List<LineObject>(lines.Count());

            foreach (Line line in lines)
            {
                table.Add(new LineObject(line));
            }

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, table);

                return stream.ToArray();
            }
        }

        private static void ByteArrayToCanvas(Canvas canvas, byte[] input)
        {
            canvas.Children.Clear();
            if (input is null)
            {
                return;
            }

            using (MemoryStream stream = new MemoryStream(input))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                List<LineObject> table = (List<LineObject>)formatter.Deserialize(stream);

                foreach (LineObject data in table)
                {
                    canvas.Children.Add(data.AsLine());
                }
            }
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
