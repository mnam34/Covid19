using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    public class SystemConfig : Entity
    {
        [Display(Name = "Tham số")]
        public string Key { get; set; }
        [Display(Name = "Giá trị")]
        public string Value { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [NotMapped]
        public override string Describe => "{ Id : \"" + Id + "\", Key : \"" + Key + "\", Value : \"" + Value + "\" }";
    }
}
