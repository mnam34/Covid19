using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{

    /// <summary>
    /// Tài liệu, giấy tờ liên quan đến ca F
    /// </summary>
    public class FCaseDocument : Entity
    {
        [Display(Name = "Tệp tài liệu liên quan!")]
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public DateTime DocumentDate { get; set; }

        public long FCaseId { get; set; }
        [ForeignKey("FCaseId")]
        public virtual FCase FCase { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", FCaseId : \"" + FCaseId + "\", DocumentPath : \"" + DocumentPath + "\", CreateBy : \"" + CreateBy + "\" }";
    }
}