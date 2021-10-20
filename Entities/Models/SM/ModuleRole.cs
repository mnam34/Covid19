using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    /// <summary>
    /// Chức năng thuộc nhóm quyền
    /// </summary>
    public class ModuleRole : Entity
    {
        public long RoleId { get; set; }
        /// <summary>
        /// Mã của chức năng, lấy theo enum
        /// </summary>
        [StringLength(100)]
        public string ModuleCode { get; set; }
        [Column(TypeName = "tinyint")]
        public byte? Create { get; set; }
        [Column(TypeName = "tinyint")]
        public byte? Read { get; set; }
        [Column(TypeName = "tinyint")]
        public byte? Update { get; set; }
        [Column(TypeName = "tinyint")]
        public byte? Delete { get; set; }
        //Duyệt
        [Column(TypeName = "tinyint")]
        public byte? Verify { get; set; }
        //Xuất bản
        [Column(TypeName = "tinyint")]
        public byte? Publish { get; set; }

        [Column(TypeName = "tinyint")]
        public byte? Confirm { get; set; }

        [Column(TypeName = "tinyint")]
        public byte? Approve { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\",RoleId : \"" + RoleId + "\", ModuleCode : \"" + ModuleCode + "\", Create : \"" + Create + "\", Read : \"" + Read + "\", Update : \"" + Update + "\", Delete : \"" + Delete + "\" }";
    }
}
