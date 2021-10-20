using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class QuarantinePlaceCreating
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên khu cách ly tập trung (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự!")]
        public string Address { get; set; }

        [Display(Name = "Sức chứa (bao nhiêu người)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]        
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        [Display(Name = "Ngày bắt đầu hoạt động (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string OpenDate { get; set; }
        [Display(Name = "Ngày ngừng hoạt động")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string CloseDate { get; set; }

        [Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        [Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        public long CommuneId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        [Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        public long DistrictId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        [Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        public long ProvinceId { get; set; }
    }
    public class QuarantinePlaceUpdating
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên khu cách ly tập trung (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự!")]
        public string Address { get; set; }

        [Display(Name = "Sức chứa (bao nhiêu người)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        [Display(Name = "Ngày bắt đầu hoạt động (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string OpenDate { get; set; }
        [Display(Name = "Ngày ngừng hoạt động")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string CloseDate { get; set; }

        [Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        [Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        public long CommuneId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        [Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        public long DistrictId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        [Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        public long ProvinceId { get; set; }
    }
}
