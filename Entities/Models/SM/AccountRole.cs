using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    /// <summary>
    /// Account thuộc nhóm quyền nào
    /// </summary>
    public class AccountRole : Entity
    {
        //Người dùng
        public long AccountId { get; set; }
        //Nhóm quyền
        public long RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\",AccountId : \"" + AccountId + "\", RoleId : \"" + RoleId + "\" }";
    }
}
