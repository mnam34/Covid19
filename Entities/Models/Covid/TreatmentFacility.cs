using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Cơ sở điều trị bệnh (tại nhà, bệnh viện dã chiến, bệnh viện...)
    /// </summary>
    public class TreatmentFacility : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự!")]
        public string Address { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }

        public virtual ICollection<FCase> FCases { get; set; }

        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\", Address : \"" + Address + "\", OrdinalNumber : \"" + OrdinalNumber + "\" }";
    }
}