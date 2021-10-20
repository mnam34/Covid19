using Common.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Quận/Huyện/TP trực thuộc tỉnh/TP trung ương
    /// </summary>
    [DropDown(ValueField = "Id", TextField = "Name")]
    public class District : Entity
    {
        [Display(Name = "Mã")]
        [StringLength(12, ErrorMessage = "Mã không được vượt quá 12 ký tự!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Ngừng sử dụng")]
        public bool Unused { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public long ProvinceId { get; set; }

        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        public virtual ICollection<Commune> Communes { get; set; }
        public virtual ICollection<FCase> FCases { get; set; }
        public virtual ICollection<EpidemicArea> EpidemicAreas { get; set; }
        public virtual ICollection<QuarantinePlace> QuarantinePlaces { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Code : \"" + Code + "\", Name : \"" + Name + "\", ProvinceId : \"" + ProvinceId + "\" }";
    }
}
