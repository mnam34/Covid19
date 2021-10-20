using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    /// <summary>
    /// Hình thức cách ly
    ///  cấp 1 (cách ly tại nhà, tự theo dõi); 
    ///  cấp 2 (cách ly tại nhà, cơ quan y tế theo dõi); 
    ///  cấp 3 (cách ly tập trung).
    /// </summary>
    public class QuarantineType : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }

        public virtual ICollection<FCase> FCases { get; set; }

        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\", OrdinalNumber : \"" + OrdinalNumber + "\" }";
    }
}