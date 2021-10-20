using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    /// <summary>
    /// Nhóm quyền
    /// </summary>
    public class Role : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập tên nhóm quyền!")]
        [Display(Name = "Tên nhóm quyền")]
        [StringLength(100, ErrorMessage = "Tên nhóm quyền không được vượt quá 100 ký tự!")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập mã nhóm quyền!")]
        [Display(Name = "Mã nhóm quyền")]
        [StringLength(20, ErrorMessage = "Mã nhóm quyền không được vượt quá 20 ký tự!")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(200, ErrorMessage = "Mô tả nhóm quyền không được vượt quá 200 ký tự!")]
        public string Description { get; set; }
        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        public virtual ICollection<ModuleRole> ModuleRoles { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\", Code : \"" + Code + "\" }";
    }
}
