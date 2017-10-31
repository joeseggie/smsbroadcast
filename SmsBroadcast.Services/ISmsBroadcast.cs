using System;
using System.Threading.Tasks;
using SmsBroadcast.Infrastructure;
using SmsBroadcast.Infrastructure.Entities;

namespace SmsBroadcast.Services
{
    public interface ISmsBroadcast
    {
         Task<OperationResult> BroadcastOnce(string[] recipients, Schedule broadcastSchedule);
         Task<OperationResult> ScheduleBroadcast(string[] recipients, Schedule broadcastSchedule, DateTimeOffset broadcastDate);
         Task<OperationResult> DailyBroadcast(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> WeeklyBroadcast(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> MonthlyBroadcast(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> YearlyBroadcast(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate);
         Task<OperationResult> CancelBroadcast(string code);
    }
}