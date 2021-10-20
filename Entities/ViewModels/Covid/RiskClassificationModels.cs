using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class RiskClassificationCreating
    {
        [Required(ErrorMessage = "Vui lòng nhập Mức độ nguy cơ!")]
        [Display(Name = "Mức độ nguy cơ nhiễm bệnh (*)")]
        [StringLength(200, ErrorMessage = "Mức độ nguy cơ không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
    }
    public class RiskClassificationUpdating
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Mức độ nguy cơ!")]
        [Display(Name = "Mức độ nguy cơ nhiễm bệnh (*)")]
        [StringLength(200, ErrorMessage = "Mức độ nguy cơ không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
    }
}
