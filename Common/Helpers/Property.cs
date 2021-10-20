using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class Property
    {
        public string DataType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public bool IsKey { get; set; }
        public bool IsAutoNumber { get; set; }
        public string ClassName { get; set; }
    }
}
