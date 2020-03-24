using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ORKK.Dummy {
    public class Order {

        private static int _ID = 0;

        public static Order Next() {

            Order result = new Order() { ID = _ID++ };
            return result;
        }

        public int ID { get; set; }
        public string instruction { get; set; }
        public DateTime date_execution { get; set; }
        public string cable_supplier { get; set; }
        public string comment { get; set; }
        public BitmapImage signature { get; set; }
        public int company_hour_count { get; set; }
        public string reason { get; set; }

        public override string ToString() {
            return $"Order {ID}";
        }
    }
}
