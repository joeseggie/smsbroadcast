using System;
using System.ComponentModel.DataAnnotations;

namespace SmsBroadcast.Web.Models
{
    public class BroadcastScheduleViewModel
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [Display(Name="Broadcast Code")]
        public string Code { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [Display(Name="Run Once")]
        public bool RunOnce { get; set; }

        [Required]
        public bool Schedule { get; set; }

        [Display(Name="Date")]
        public DateTimeOffset ScheduleDate { get; set; }

        public string Repeat { get; set; }

        [Display(Name="Every")]
        public int Frequency { get; set; } = 1;

        public string Description { get; set; }
    }
}