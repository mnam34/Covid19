using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class Report1
    {
        public DateTime OutbreakDate { get; set; }
        public string EpidemicAreaName { get; set; }
        public string TotalF0 { get; set; }//Tổng số ca F0        
        public string IsCured { get; set; }//Đã khỏi bệnh
        public string UnderTreatment { get; set; }//Đang điều trị
        public string IsDead { get; set; }//Đã tử vong

        public string TotalFromFx { get; set; }//Tổng số ca F0 từ Fx
        public string TotalFromF1 { get; set; }//Tổng số ca F0 từ F1
        public string TotalFromF2 { get; set; }//Tổng số ca F0 từ F2
        public string TotalFromF3 { get; set; }//Tổng số ca F0 từ F3

        public string TotalF1 { get; set; }//Tổng số trường hợp F1
        public string TotalF2 { get; set; }//Tổng số trường hợp F2
        public string TotalF3 { get; set; }//Tổng số trường hợp F3

    }
    public class BarChartModel
    {
        public string Date { get; set; }
        public string Tooltip { get; set; }
        public int Data { get; set; }
    }

}
