using System;
using System.Collections.Generic;
using System.Text;

namespace SmsBroadcast.Core.Common
{
    public class Broadcast
    {
        public string From { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string CreatedBy { get; set; }

        public string Status { get; set; }

        public DateTimeOffset ScheduleDateTime { get; set; }
        public DateTimeOffset RepeatEndDate { get; set; }
    }
}
