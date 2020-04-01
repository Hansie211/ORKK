using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORKK.Data {

    [AttributeUsage( AttributeTargets.Class )]
    public class TableNameAttribute : Attribute {
        public string Value { get; }

        public TableNameAttribute( string value ) {

            this.Value = value;
        }
    }

}
