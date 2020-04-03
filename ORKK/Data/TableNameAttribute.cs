using System;

namespace ORKK.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string Value { get; }

        public TableNameAttribute(string value)
        {
            Value = value;
        }
    }
}
