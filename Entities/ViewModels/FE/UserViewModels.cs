using Common.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên hiển thị")]
        public string Name { get; set; }

        //[StringLength(200, ErrorMessage = "Đường dẫn ảnh đại diện không được vượt quá 200 ký tự!")]
        //[Display(Name = "Ảnh đại diện")]
        //public string ProfilePicture { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập!")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(20, ErrorMessage = "Tên đăng nhập không được vượt quá 20 ký tự!")]
        public string LoginName { get; set; }


        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Địa chỉ E-mail không hợp lệ!")]
        [Display(Name = "Địa chỉ E-mail")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số (từ 0 đến 9)!")]
        public string PhoneNumber { get; set; }
    }
    public class UserUpdate
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên hiển thị")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập!")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(20, ErrorMessage = "Tên đăng nhập không được vượt quá 20 ký tự!")]
        public string LoginName { get; set; }

        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Địa chỉ E-mail không hợp lệ!")]
        [Display(Name = "Địa chỉ E-mail")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email!")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số (từ 0 đến 9)!")]
        public string PhoneNumber { get; set; }

        
    }
    public class UserChangePassword
    {
        [Display(Name = "Mật khẩu hiện tại")]
        //[StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên!")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ!")]
        public string OldPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên!")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới!")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên!")]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới!")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không khớp nhau!")]
        public string ConfirmPassword { get; set; }

      
    }
   
}
