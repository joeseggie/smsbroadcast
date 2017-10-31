using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmsBroadcast.Infrastructure.Entities
{
    [Table("ScheduledBroadcast")]
    public class ScheduledBroadcast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Column(TypeName="varchar(50)")]
        public string From { get; set; }

        [Required]
        [MaxLength(9)]
        [Column(TypeName="varchar(9)")]
        public string To { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName="varchar(100)")]
        public string Subject { get; set; }
        
        [Required]
        [MaxLength(160)]
        [Column(TypeName="varchar(160)")]
        public string Message { get; set; }

        [Required]
        [MaxLength(500)]
        [Column(TypeName="varchar(500)")]
        public string Description { get; set; }

        [Required]
        [MaxLength(9)]
        [Column(TypeName="varchar(9)")]
        public string CreatedBy { get; set; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName="char(10)")]
        public string Status { get; set; }

        [Required]
        [Column(TypeName="datetimeoffset")]
        public DateTimeOffset Schedule { get; set; }

        [Required]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}