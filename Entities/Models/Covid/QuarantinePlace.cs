using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Khu cách ly tập trung/khách sạn/trường học/tại nhà đối với Fx
    /// </summary>
    public class QuarantinePlace : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên khu cách ly tập trung")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự!")]
        public string Address { get; set; }

        [Display(Name = "Sức chứa (bao nhiêu người)")]
        [RegularExpression(@"^[0-9]*", ErrorMessage = "Vui lòng chỉ nhập số nguyên dương!")]
        [Column(TypeName = "int")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        [Display(Name = "Ngày bắt đầu hoạt động (*)")]
        public DateTime OpenDate { get; set; }

        [Display(Name = "Ngày ngừng hoạt động")]
        public DateTime? CloseDate { get; set; }

        public bool IsClosed { get; set; }//Đã ngừng hoạt động
        public int ReOpenTime { get; set; }//Số lần mở cửa trở lại

        [Display(Name = "Phường/Xã")]
        public long CommuneId { get; set; }

        [ForeignKey("CommuneId")]
        public virtual Commune Commune { get; set; }

        public virtual ICollection<FCase> FCases { get; set; }

        public long DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        public long ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\", Address : \"" + Address + "\", CommuneId : \"" + CommuneId + "\" }";
    }
}