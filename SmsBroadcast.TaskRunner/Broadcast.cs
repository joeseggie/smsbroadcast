using System;
using System.Collections.Generic;
using System.Text;

namespace SmsBroadcast.TaskRunner
{
    public class Broadcast
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
