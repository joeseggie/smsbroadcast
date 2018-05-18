using System;
using System.Threading.Tasks;
using SmsBroadcast.Core;
using SmsBroadcast.Core.Entities;

namespace SmsBroadcast.Core.Services
{
    public interface ISmsBroadcast
    {
         Task<OperationResult> BroadcastOnceAsync(string[] recipients, Schedule broadcastSchedule);
         Task<OperationResult> ScheduleBroadcastAsync(string[] recipients, Schedule broadcastSchedule, DateTimeOffset broadcastDate);
         Task<OperationResult> DailyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate);
         Task<OperationResult> WeeklyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate);
         Task<OperationResult> MonthlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate);
         Task<OperationResult> YearlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate);
         Task<OperationResult> CancelBroadcastAsync(string code);
    }
}