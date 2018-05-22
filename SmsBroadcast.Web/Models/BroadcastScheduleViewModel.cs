using System;
using System.ComponentModel.DataAnnotations;

namespace SmsBroadcast.Web.Models
{
    public class BroadcastScheduleViewModel
    {
        [Required]
        [MaxLength(50)]
        public string From { get; set; }

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name="Broadcast Code")]
        public string Code { get; set; }

        [Required]
        [MaxLength(160)]
        public string Message { get; set; }

        [Required]
        [Display(Name="Run Once")]
        public bool RunOnce { get; set; }

        [Display(Name="Date")]
        public DateTimeOffset ScheduleDate { get; set; }

        [Display(Name="Until")]
        public DateTimeOffset RepeatEndDate { get; set; }
        
        public string Repeat { get; set; }

        [Display(Name="Every")]
        public int Frequency { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}