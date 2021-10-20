using System.ComponentModel.DataAnnotations;
namespace Entities.ViewModels
{
    public class EpidemicAreaCreating
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên khu vực/vùng/điểm khởi phát dịch (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày khởi phát!")]
        [Display(Name = "Ngày khởi phát dịch (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string OutbreakDate { get; set; }
        [Display(Name = "Ngày phong tỏa")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string LockdownDate { get; set; }
        [Display(Name = "Ngày hết phong tỏa")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string UnLockdownDate { get; set; }
        [Display(Name = "Ngày công bố hết dịch")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string EndDate { get; set; }

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
    public class EpidemicAreaUpdating
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên khu vực/vùng/điểm khởi phát dịch (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày khởi phát!")]
        [Display(Name = "Ngày khởi phát dịch (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string OutbreakDate { get; set; }
        [Display(Name = "Ngày phong tỏa")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string LockdownDate { get; set; }
        [Display(Name = "Ngày hết phong tỏa")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string UnLockdownDate { get; set; }
        [Display(Name = "Ngày công bố hết dịch")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string EndDate { get; set; }

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
    public class EpidemicAreaList
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
