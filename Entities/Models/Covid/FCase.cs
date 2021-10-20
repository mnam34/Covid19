using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class FCase : Entity
    {

        [Display(Name = "Mã định danh của bộ y tế")]
        [StringLength(12, ErrorMessage = "Mã không được vượt quá 12 ký tự!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Năm sinh")]
        public int YearOfBirth { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(35, ErrorMessage = "Mã không được vượt quá 35 ký tự!")]
        public string PhoneNumber { get; set; }
        public string ShortName { get; set; }//Tên viết tắt để ẩn tên thật

        [Display(Name = "Yếu tố dịch tễ")]
        public string Epidemiology { get; set; }
        public DateTime? FxContactDate { get; set; }//Ngày cuối tiếp xúc với Fx
        public DateTime? TracingDate { get; set; }//Ngày truy vết

        public bool IsF0 { get; set; }//Là ca F0
        public DateTime? F0Date { get; set; }//Ngày phát hiện/khẳng định là ca F0
        public bool IsFx2F0 { get; set; }//Là ca Fx chuyển thành F0
        public int FromLevel { get; set; }//Từ F mấy trở thành F0
        public long? F0Id { get; set; }//Là F0 nào (fx thành f0 từ f0 nào)//F0 trực tiếp
        public long? F0Foretell { get; set; }//Id của F0 chỉ điểm
        public bool IsCured { get; set; }//Đã khỏi bệnh
        public DateTime? CuredDate { get; set; }//Ngày khỏi bệnh
        public bool IsSuspected { get; set; }//Là ca nghi ngờ
        public DateTime? SuspectedDate { get; set; }//Ngày nghi ngờ
        public bool IsSafe { get; set; }//Là ca nghi ngờ nhưng đã khẳng định an toàn không nhiễm bệnh
        public DateTime? ConfirmSafeDate { get; set; }//Ngày khẳng định an toàn

        public bool IsDeath { get; set; }//Là ca đã tử vong
        public DateTime? DeathDate { get; set; }//Ngày tử vong

        public DateTime? MonitorStartDate { get; set; }//Ngày bắt đầu giám sát/cách ly đối với F1, F2...
        public DateTime? MonitorEndDate { get; set; }//Ngày kết thúc giám sát/cách ly đối với F1, F2...


        public string Address { get; set; }//Địa chỉ.
        public long AddressCommuneId { get; set; }
        public long AddressDistrictId { get; set; }
        public long AddressProvinceId { get; set; }

        public string MovingRoute { get; set; }//Lịch trình di chuyển.
        public string ContactHistory { get; set; }//Lịch sử tiếp xúc.


        [Display(Name = "Thuộc vùng/khu vực dịch bệnh nào")]
        public long EpidemicAreaId { get; set; }
        [ForeignKey("EpidemicAreaId")]
        public virtual EpidemicArea EpidemicArea { get; set; }

        [Display(Name = "Thuộc vùng/khu vực dịch bệnh liên quan")]//Ví vụ chuỗi ca bệnh tại NGa Sơn có liên quan đến chuỗi ca bệnh tại Hợp Lực
        public long? EpidemicAreaRelatedId { get; set; }

        [Display(Name = "Nơi phát hiện ca bệnh")]//Đối với F0//Nguồn lây
        public long? DetectedPlaceId { get; set; }
        [ForeignKey("DetectedPlaceId")]
        public virtual DetectedPlace DetectedPlace { get; set; }

        [Display(Name = "Hình thức cách ly")]// đối với Fx
        public long? QuarantineTypeId { get; set; }
        [ForeignKey("QuarantineTypeId")]
        public virtual QuarantineType QuarantineType { get; set; }
        [Display(Name = "Thời gian cách ly (bao nhiêu ngày)")]// đối với Fx
        public int QuarantineDays { get; set; }

        [Display(Name = "Nơi cách ly")]// đối với Fx
        public long? QuarantinePlaceId { get; set; }
        [ForeignKey("QuarantinePlaceId")]
        public virtual QuarantinePlace QuarantinePlace { get; set; }

        [Display(Name = "Nơi điều trị")]// đối với F0
        public long? TreatmentFacilityId { get; set; }
        [ForeignKey("TreatmentFacilityId")]
        public virtual TreatmentFacility TreatmentFacility { get; set; }

        public virtual ICollection<TestResult> TestResults { get; set; }//Kết quả các đợt xét nghiệm/điều trị
        public virtual ICollection<FCaseDocument> FCaseDocuments { get; set; }//Tài liệu giấy tờ liên quan

        public int Level { get; set; }//Là F mấy
        public int LevelInitial { get; set; }//Là F lúc đầu

        public long? FCaseId { get; set; }
        [ForeignKey("FCaseId")]
        public virtual FCase ParentFCase { get; set; }//F cấp trên
        //Danh sách F liên quan
        public virtual ICollection<FCase> FCases { get; set; }


        public long CommuneId { get; set; }
        [ForeignKey("CommuneId")]
        public virtual Commune Commune { get; set; }

        public long DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        public long ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        public long? RiskClassificationId { get; set; }
        [ForeignKey("RiskClassificationId")]
        public virtual RiskClassification RiskClassification { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Code : \"" + Code + "\", Name : \"" + Name + "\", Address : \"" + Address + "\", EpidemicAreaId : \"" + EpidemicAreaId + "\" }";
    }
}
//Nhập: Ca nghi ngờ hoặc ca F0;
//Nếu nhập ca nghi ngờ, thì nhập ngày nghi ngờ, sau đó sẽ có lịch sử chuyển từ nghi ngờ sang khẳng định

//Quản lý ca nghi nhiễm
//Quản lý ca bệnh F0
//Quản lý các địa điểm cách ly.
//nơi điều trị: tại nhà, bv dã chiến. bv huyện, bv tỉnh
//Ca dương tính trong ngày tại cộng đồng; tại khu vực phong tỏa; tại khu cách ly; cách ly tại nhà; khám sàng lọc tại bệnh viện; mở rộng xét nghiệm.