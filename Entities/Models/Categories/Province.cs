using Common.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Tỉnh/Thành phố trực thuộc trung ương
    /// </summary>
    [DropDown(ValueField = "Id", TextField = "Name")]
    public class Province : Entity
    {
        [Display(Name = "Mã")]
        [StringLength(12, ErrorMessage = "Mã không được vượt quá 12 ký tự!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Ngừng sử dụng")]
        public bool Unused { get; set; }

        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<FCase> FCases { get; set; }
        public virtual ICollection<EpidemicArea> EpidemicAreas { get; set; }
        public virtual ICollection<QuarantinePlace> QuarantinePlaces { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Code : \"" + Code + "\", Name : \"" + Name + "\" }";
    }
}