using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên truy cập!")]
        [Display(Name = "Tên truy cập")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}
