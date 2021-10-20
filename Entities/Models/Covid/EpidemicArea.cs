using Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Khu vực/vùng/điểm dịch
    /// </summary>
    [DropDown(ValueField = "Id", TextField = "Name")]
    public class EpidemicArea : Entity
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [Display(Name = "Tên Khu vực/vùng/điểm dịch")]
        [StringLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        [Display(Name = "Ngày khởi phát dịch")]
        public DateTime OutbreakDate { get; set; }

        [Display(Name = "Ngày phong tỏa")]
        public DateTime? LockdownDate { get; set; }
        [Display(Name = "Ngày hết phong tỏa")]
        public DateTime? UnLockdownDate { get; set; }
        [Display(Name = "Ngày công bố hết dịch")]
        public DateTime? EndDate { get; set; }

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
        public override string Describe => "{ Id : \"" + Id + "\", Name : \"" + Name + "\", CommuneId : \"" + CommuneId + "\" }";
    }
}
