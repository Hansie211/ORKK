using ORKK.Data;
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

            return false;

            //MainWindow mw = (MainWindow)App.Current.MainWindow;

            //if (mw.ActiveOrder == null)
            //{
            //    return false;
            //}

            //return ((OrderObject)value).ID == mw.ActiveOrder.ID;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException();
        }
    }
}
