using System;
using System.Data;
using System.Reflection;

namespace ORKK.Data
{
    public class ColumnProperty
    {
        public string PropName { get; }

        public string ColumnName { get; }

        public PropertyInfo PropInfo { get; }

        public SqlDbType DbType { get; }

        public int Size { get; }

        public ColumnProperty(Type referenceType, string propName, string columnName, SqlDbType dbType, int size = -1)
        {
            PropName = propName;
            ColumnName = columnName;
            DbType = dbType;
            Size = size;

            PropInfo = referenceType.GetProperty(propName);
            if (PropInfo is null)
            {
                throw new NullReferenceException($"Cannot get property { propName } for class { referenceType.FullName }.");
            }
        }
    }
}
