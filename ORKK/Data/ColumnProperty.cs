using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORKK.Data {
    public class ColumnProperty {

        public string PropName { get; }
        public string ColumnName { get; }
        public PropertyInfo PropInfo { get; }
        public SqlDbType DbType { get; }
        public ColumnProperty( Type referenceType, string propName, string columnName, SqlDbType dbType ) {

            this.PropName   = propName;
            this.ColumnName = columnName;
            this.DbType     = dbType;

            this.PropInfo   = referenceType.GetProperty( propName );

            if ( PropInfo is null ) {

                throw new NullReferenceException( $"Cannot get property { propName } for class { referenceType.FullName }." );
            }
        }
    }
}
