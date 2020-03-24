using ORKK.Dummy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ORKK.Converters {
    class IsSelectedConverter : IValueConverter {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            MainWindow mw = (MainWindow)App.Current.MainWindow;
            return ( (Order)value ).ID == mw.ActiveOrder.ID;
            //throw new NotImplementedException();
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException();
        }
    }
}
