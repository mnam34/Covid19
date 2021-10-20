using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Phân loại nguy cơ:
    /// Nguy cơ cao
    /// Nguy cơ vừa (trung bình)
    /// Nguy cơ thấp
    /// </summary>
    public class RiskClassification : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập Mức độ nguy cơ!")]
        [Display(Name = "Mức độ nguy cơ nhiễm bệnh (*)")]
        [StringLength(200, ErrorMessage = "Mức độ nguy cơ không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }

        public virtual ICollection<FCase> FCases { get; set; }

        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\" }";
    }
}
