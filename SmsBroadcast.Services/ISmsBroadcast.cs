using System;
using System.Threading.Tasks;
using SmsBroadcast.Infrastructure;
using SmsBroadcast.Infrastructure.Entities;

namespace SmsBroadcast.Services
{
    public interface ISmsBroadcast
    {
         Task<OperationResult> BroadcastOnceAsync(string[] recipients, Schedule broadcastSchedule);
         Task<OperationResult> ScheduleBroadcastAsync(string[] recipients, Schedule broadcastSchedule, DateTimeOffset broadcastDate);
         Task<OperationResult> DailyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> WeeklyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> MonthlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> YearlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> CancelBroadcastAsync(string code);
    }
}