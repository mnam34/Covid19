using Common.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    [DropDown(ValueField = "Id", TextField = "Name")]
    public class Account : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên!")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên truy cập!")]
        [Display(Name = "Tên truy cập")]
        [StringLength(20, ErrorMessage = "Tên truy cập không được vượt quá 20 ký tự!")]
        public string LoginName { get; set; }

        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Địa chỉ E-mail không hợp lệ!")]
        [Display(Name = "Địa chỉ E-mail!")]
        [StringLength(150, ErrorMessage = "Địa chỉ E-mail không được vượt quá 150 ký tự!")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { get; set; }

        [Display(Name = "Được phép truy cập")]
        public bool AccessRight { get; set; }

        [Display(Name = "Họ và tên")]
        public string RealName { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string ProfilePicture { get; set; }
        public bool IsManageAccount { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<AccountRole> AccountRoles { get; set; }

        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\", LoginName : \"" + LoginName + "\", Email : \"" + Email + "\", AccessRight : \"" + AccessRight + "\",  RealName : \"" + RealName + "\", IsManageAccount : \"" + IsManageAccount + "\" }";
    }
}
