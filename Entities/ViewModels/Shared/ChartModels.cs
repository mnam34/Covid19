using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class OrgChart
    {
        public string id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public IEnumerable<OrgChart> children { get; set; }
        public string img { get; set; }
        public string className { get; set; }
        public bool collapsed { get; set; }
    }
    public class HypertreeChart
    {
        public string id { get; set; }
        public string name { get; set; }
        public IEnumerable<HypertreeChart> children { get; set; }
        public IEnumerable<HypertreeChartData> data { get; set; }
    }
    public class HypertreeChartData
    {
        public string band { get; set; }
        public string relation { get; set; }
    }

    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
        public JsTreeStateModel state { get; set; }
        public string icon { get; set; }
    }
    public class JsTreeStateModel
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
}
