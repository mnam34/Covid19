using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.ViewModels
{
    public class FCaseCreate
    {
        [Display(Name = "Mã định danh của bộ y tế")]
        [StringLength(12, ErrorMessage = "Mã không được vượt quá 12 ký tự!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Họ và tên (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [Display(Name = "Địa chỉ (*)")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        [Display(Name = "Ngày xác nhận (công bố) là F0 (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string F0Date { get; set; }
        [Display(Name = "Lịch trình di chuyển")]
        public string MovingRoute { get; set; }//Lịch trình di chuyển.
        [Display(Name = "Lịch sử tiếp xúc")]
        public string ContactHistory { get; set; }//Lịch sử tiếp xúc.


        [Display(Name = "Thuộc Khu vực/vùng/điểm dịch bệnh nào (*)")]
        [Required(ErrorMessage = "Vui lòng chọn Khu vực/vùng/điểm dịch!")]
        public long EpidemicAreaId { get; set; }


        [Display(Name = "Thuộc vùng/khu vực dịch bệnh liên quan")]//Ví vụ chuỗi ca bệnh tại NGa Sơn có liên quan đến chuỗi ca bệnh tại Hợp Lực
        public long? EpidemicAreaRelatedId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Nơi phát hiện ca bệnh (nguồn lây)!")]
        [Display(Name = "Nơi phát hiện ca bệnh (nguồn lây) (*)")]//Đối với F0//Nguồn lây
        public long DetectedPlaceId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Nơi điều trị!")]
        [Display(Name = "Nơi điều trị (*)")]// đối với F0
        public long TreatmentFacilityId { get; set; }


        //[Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        //[Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        //public long CommuneId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        //[Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        //public long DistrictId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        //[Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        //public long ProvinceId { get; set; }


        //[Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        [Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        public long AddressCommuneId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        //[Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        public long AddressDistrictId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        //[Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        public long AddressProvinceId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm sinh!")]
        [Display(Name = "Năm sinh (*)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        public int YearOfBirth { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(35, ErrorMessage = "Mã không được vượt quá 35 ký tự!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Yếu tố dịch tễ")]
        public string Epidemiology { get; set; }

    }
    public class FCaseUpdate
    {
        public long Id { get; set; }
        [Display(Name = "Mã định danh của bộ y tế")]
        [StringLength(12, ErrorMessage = "Mã không được vượt quá 12 ký tự!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Họ và tên (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [Display(Name = "Địa chỉ (*)")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        [Display(Name = "Ngày xác nhận (công bố) là F0 (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string F0Date { get; set; }
        [Display(Name = "Lịch trình di chuyển")]
        public string MovingRoute { get; set; }//Lịch trình di chuyển.
        [Display(Name = "Lịch sử tiếp xúc")]
        public string ContactHistory { get; set; }//Lịch sử tiếp xúc.


        [Display(Name = "Thuộc Khu vực/vùng/điểm dịch bệnh nào (*)")]
        [Required(ErrorMessage = "Vui lòng chọn Khu vực/vùng/điểm dịch!")]
        public long EpidemicAreaId { get; set; }


        [Display(Name = "Thuộc vùng/khu vực dịch bệnh liên quan")]//Ví vụ chuỗi ca bệnh tại NGa Sơn có liên quan đến chuỗi ca bệnh tại Hợp Lực
        public long? EpidemicAreaRelatedId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Nơi phát hiện ca bệnh (nguồn lây)!")]
        [Display(Name = "Nơi phát hiện ca bệnh (nguồn lây) (*)")]//Đối với F0//Nguồn lây
        public long DetectedPlaceId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Nơi điều trị!")]
        [Display(Name = "Nơi điều trị (*)")]// đối với F0
        public long TreatmentFacilityId { get; set; }


        //[Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        //[Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        //public long CommuneId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        //[Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        //public long DistrictId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        //[Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        //public long ProvinceId { get; set; }


        //[Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        [Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        public long AddressCommuneId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        //[Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        public long AddressDistrictId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        //[Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        public long AddressProvinceId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm sinh!")]
        [Display(Name = "Năm sinh (*)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        public int YearOfBirth { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(35, ErrorMessage = "Mã không được vượt quá 35 ký tự!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Yếu tố dịch tễ")]
        public string Epidemiology { get; set; }

    }
    public class FCaseConfirm
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Date { get; set; }
    }
    public class FCaseTestResult
    {
        public long Id { get; set; }
        public bool IsPositive { get; set; }//Dương tính.
        [Display(Name = "Chi tiết kết quả xét nghiệm")]
        public string ResultDetail { get; set; }
        [Display(Name = "Ngày nhận mẫu/ ngày xét nghiệm")]
        public string TestDate { get; set; }
        [Display(Name = "Ngày trả kết quả")]
        public string ResultDate { get; set; }
        [Display(Name = "Nhiệt độ đo được")]
        public string Temperature { get; set; }
    }
    public class FCaseTestResultUpdate
    {
        public long Id { get; set; }
        public bool IsPositive { get; set; }//Dương tính.
        [Display(Name = "Chi tiết kết quả xét nghiệm")]
        public string ResultDetail { get; set; }
        [Display(Name = "Ngày nhận mẫu/ ngày xét nghiệm")]
        public string TestDate { get; set; }
        [Display(Name = "Ngày trả kết quả")]
        public string ResultDate { get; set; }
        [Display(Name = "Nhiệt độ đo được")]
        public string Temperature { get; set; }
    }

    public class FxCaseCreate
    {
        public long FCaseId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Họ và tên (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [Display(Name = "Địa chỉ (*)")]
        public string Address { get; set; }

        [Display(Name = "Lịch trình di chuyển")]
        public string MovingRoute { get; set; }//Lịch trình di chuyển.
        [Display(Name = "Lịch sử tiếp xúc")]
        public string ContactHistory { get; set; }//Lịch sử tiếp xúc.


        [Display(Name = "Ngày bắt đầu giám sát/cách ly")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string MonitorStartDate { get; set; }
        [Display(Name = "Ngày kết thúc giám sát/cách ly")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string MonitorEndDate { get; set; }

        [Display(Name = "Hình thức cách ly")]// đối với Fx
        public long? QuarantineTypeId { get; set; }

        [Display(Name = "Thời gian cách ly (bao nhiêu ngày)")]// đối với Fx
        public int QuarantineDays { get; set; }

        [Display(Name = "Nơi cách ly")]// đối với Fx
        public long? QuarantinePlaceId { get; set; }

        //[Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        [Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        public long AddressCommuneId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        [Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        public long AddressDistrictId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        [Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        public long AddressProvinceId { get; set; }

        public string ParrentName { get; set; }
        public int NextLevel { get; set; }

        [Display(Name = "Phân luồng nguy cơ")]// đối với Fx
        public long? RiskClassificationId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm sinh!")]
        [Display(Name = "Năm sinh (*)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        public int YearOfBirth { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(35, ErrorMessage = "Mã không được vượt quá 35 ký tự!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Yếu tố dịch tễ")]
        public string Epidemiology { get; set; }

        [Display(Name = "Ngày cuối tiếp xúc với F cấp trên")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string FxContactDate { get; set; }//Ngày cuối tiếp xúc với Fx
        [Required(ErrorMessage = "Vui lòng nhập ngày truy vết!")]
        [Display(Name = "Ngày Truy vết được (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string TracingDate { get; set; }//Ngày Truy vết được
    }
    public class FxCaseUpdate
    {
        public long FCaseId { get; set; }
        public long FxCaseId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Họ và tên (*)")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [Display(Name = "Địa chỉ (*)")]
        public string Address { get; set; }

        [Display(Name = "Lịch trình di chuyển")]
        public string MovingRoute { get; set; }//Lịch trình di chuyển.
        [Display(Name = "Lịch sử tiếp xúc")]
        public string ContactHistory { get; set; }//Lịch sử tiếp xúc.


        [Display(Name = "Ngày bắt đầu giám sát/cách ly")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string MonitorStartDate { get; set; }
        [Display(Name = "Ngày kết thúc giám sát/cách ly")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string MonitorEndDate { get; set; }

        [Display(Name = "Hình thức cách ly")]// đối với Fx
        public long? QuarantineTypeId { get; set; }

        [Display(Name = "Thời gian cách ly (bao nhiêu ngày)")]// đối với Fx
        public int QuarantineDays { get; set; }

        [Display(Name = "Nơi cách ly")]// đối với Fx
        public long? QuarantinePlaceId { get; set; }

        //[Display(Name = "Thuộc địa phận Phường/Xã (*)")]
        [Required(ErrorMessage = "Vui lòng chọn phường/xã!")]
        public long AddressCommuneId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Quận/huyện!")]
        [Display(Name = "Thuộc địa phận Quận/huyện (*)")]
        public long AddressDistrictId { get; set; }
        //[Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành!")]
        [Display(Name = "Thuộc địa phận Tỉnh/Thành phố (*)")]
        public long AddressProvinceId { get; set; }

        public string ParrentName { get; set; }
        public int NextLevel { get; set; }
        [Display(Name = "Phân luồng nguy cơ")]// đối với Fx
        public long? RiskClassificationId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm sinh!")]
        [Display(Name = "Năm sinh (*)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        public int YearOfBirth { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(35, ErrorMessage = "Mã không được vượt quá 35 ký tự!")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Yếu tố dịch tễ")]
        public string Epidemiology { get; set; }

        [Display(Name = "Ngày cuối tiếp xúc với F cấp trên")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string FxContactDate { get; set; }//Ngày cuối tiếp xúc với Fx
        [Required(ErrorMessage = "Vui lòng nhập ngày truy vết!")]
        [Display(Name = "Ngày Truy vết được (*)")]
        [RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$", ErrorMessage = "Ngày không hợp lệ!")]
        public string TracingDate { get; set; }//Ngày Truy vết được
    }
    public class FCaseAdditionalInfo
    {
        public bool IsFx2F0 { get; set; }//Là ca Fx chuyển thành F0
        public long F0Id { get; set; }//Là F0 nào (fx thành f0 từ f0 nào)//F0 trực tiếp
        public long F0Foretell { get; set; }//Id của F0 chỉ điểm

        public string F0Name { get; set; }//F0 trực tiếp
        public string F0ForetellName { get; set; }//F0 chỉ điểm
        public string FromLevel { get; set; }//Từ F mấy thành F0
    }
}
