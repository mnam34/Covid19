using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class TreatmentFacilityCreating
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên Cơ sở điều trị bệnh (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự!")]
        public string Address { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
    }
    public class TreatmentFacilityUpdating
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên Cơ sở điều trị bệnh (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự!")]
        public string Address { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Required(ErrorMessage = "Vui lòng nhập Thứ tự hiển thị!")]
        public int OrdinalNumber { get; set; }
    }
}
