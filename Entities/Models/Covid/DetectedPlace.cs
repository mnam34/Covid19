using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Nơi phát hiện ca bệnh: 
    /// tại cộng đồng; tại khu cách ly; tại khu vực phong tỏa
    /// khám sàng lọc tại bệnh viện; cơ sở ý tế; cách ly tại nhà; xét nghiệm mở rộng
    /// </summary>
    public class DetectedPlace : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên nơi phát hiện ca bệnh")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
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
