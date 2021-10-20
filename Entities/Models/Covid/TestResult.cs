using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Kết quả các đợt xét nghiệm.
    /// </summary>
    public class TestResult : Entity
    {
        public bool IsPositive { get; set; }//Dương tính.
        [Display(Name = "Chi tiết kết quả xét nghiệm")]
        public string ResultDetail { get; set; }
        [Display(Name = "Nhiệt độ đo được")]
        public string Temperature { get; set; }
        [Display(Name = "Ngày nhận mẫu/ ngày xét nghiệm")]
        public DateTime TestDate { get; set; }
        [Display(Name = "Ngày trả kết quả")]
        public DateTime ResultDate { get; set; }

        public long FCaseId { get; set; }
        [ForeignKey("FCaseId")]
        public virtual FCase FCase { get; set; }

        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", ResultDetail : \"" + ResultDetail + "\", FCaseId : \"" + FCaseId + "\", IsPositive : \"" + IsPositive + "\" }";
    }
}
