using SmsBroadcast.Core.Common;
using SmsBroadcast.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SmsBroadcast.Core.Services
{
    public interface ISmsBroadcast
    {
        Task<OperationResult> CancelBroadcastAsync(string code);

        Task ProcessBroadcastAsync(Broadcast broadcast, string[] recipients, bool runOnce = true, ScheduleRepeat repeat = ScheduleRepeat.None, int frequency = 0);

        Task ScheduleBroadcastAsync(Schedule schedule);
    }
}