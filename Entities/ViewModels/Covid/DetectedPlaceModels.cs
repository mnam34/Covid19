using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class DetectedPlaceCreating
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên nơi phát hiện ca bệnh (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
    }
    public class DetectedPlaceUpdating
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên nơi phát hiện ca bệnh (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
    }
}
