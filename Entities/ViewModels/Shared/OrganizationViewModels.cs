using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class DivisionGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long GroupKey { get; set; }
        public string GroupName { get; set; }
        public int? OrdinalNumber { get; set; }
    }
}
