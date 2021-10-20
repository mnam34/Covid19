using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class AccountCreate
    {
        [Required(ErrorMessage = "Vui lòng nhập Họ và tên!")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập Địa chỉ Email!")]
        [Display(Name = "Địa chỉ Email")]
        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Địa chỉ E-mail không hợp lệ!")]
        [StringLength(150, ErrorMessage = "Địa chỉ E-mail không được vượt quá 150 ký tự!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên truy cập!")]
        [Display(Name = "Tên truy cập")]
        [StringLength(20, ErrorMessage = "Tên truy cập không được vượt quá 20 ký tự!")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Đơn vị Phòng/Trạm")]
        public long? DivisionId { get; set; }
        [Display(Name = "Chức vụ")]
        public long? PositionId { get; set; }
    }
    public class AccountUpdate
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ và tên!")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập Địa chỉ Email!")]
        [Display(Name = "Địa chỉ Email")]
        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'‌​*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[‌​a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Địa chỉ E-mail không hợp lệ!")]
        [StringLength(150, ErrorMessage = "Địa chỉ E-mail không được vượt quá 150 ký tự!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên truy cập!")]
        [Display(Name = "Tên truy cập")]
        [StringLength(20, ErrorMessage = "Tên truy cập không được vượt quá 20 ký tự!")]
        public string LoginName { get; set; }
        [Display(Name = "Đơn vị Phòng/Trạm")]
        public long? DivisionId { get; set; }
        [Display(Name = "Chức vụ")]
        public long? PositionId { get; set; }
    }
    public class AccountChangePassword
    {
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới!")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu!")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp, vui lòng nhập lại!")]
        public string ConfirmPassword { get; set; }
    }
}
