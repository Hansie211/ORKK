using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORKK.Data {
    public abstract class DatabaseVaultObject {

        public int ID { get; set; }
        public bool AnyPropertyChanged { get; set; } = false;

    }
}
