using System;
using System.ComponentModel.DataAnnotations;
namespace Entities
{
    public class AuditLog 
    {
        [Key]
        public Guid AuditLogId { get; set; }

        [Required]
        public long AccountId { get; set; }

        [Required]
        public DateTime EventDate { get; set; }
        public string EventDateDetail { get; set; }

        [Required]
        [MaxLength(1)]//CRUD
        public string EventType { get; set; }

        [Required]
        [MaxLength(100)]
        public string TableName { get; set; }

        [Required]
        public string RecordKey { get; set; }

        [Required]
        [MaxLength(100)]
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public string IpAddress { get; set; }

        //public string Describe()
        //{
        //    return "Logging";
        //}
    }
}
