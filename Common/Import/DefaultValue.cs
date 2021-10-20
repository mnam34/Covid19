using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Import
{
    public class DefaultValue
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Expression<Func<object, object>> Express { get; set; }
        public DefaultValue(string name, object value)
        {
            PropertyName = name;
            Value = value;
        }
        public DefaultValue(string name, Expression<Func<object, object>> express)
        {
            PropertyName = name;
            Express = express;
        }
    }
}
