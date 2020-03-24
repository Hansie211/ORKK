using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORKK.Dummy {

    public enum Damage {
        geen,
        gering,
        gemiddeld,
        hoog,
        zeer_hoog,
    }

    class Cablechecklist {

        private static int _ID = 0;
        public static Cablechecklist Next() {

            Cablechecklist result = new Cablechecklist() { ID = _ID++ };
            return result;
        }

        public int ID { get; set; }
        public int OrderID { get; set; }
        public int rupture_6d { get; set; }
        public int rupture_30d { get; set; }
        public Damage damage_outside { get; set; }
        public Damage damage_rust_corrosion { get; set; }
        public int reduced_cable_diameter { get; set; }
        public int position_meassuring_point { get; set; }
        public Damage damage_total { get; set; }
        public int type_damage_corrosion { get; set; }
    }
}
