using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmsBroadcast.Core.Common;
using SmsBroadcast.Core.Data;
using SmsBroadcast.Core.Entities;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmsBroadcast.Core.Services
{
    public class SmsBroadcastService : ISmsBroadcast
    {
        private readonly SmsBroadcastContext _db;
        ILogger<SmsBroadcastService> _logger;

        public SmsBroadcastService(SmsBroadcastContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Cancel broadcast schedule.
        /// </summary>
        /// <param name="code">Code of the broadcast schedule.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> CancelBroadcastAsync(string code)
        {
            try
            {
                var schedulesToDelete = _db.Schedules.Where(s => s.Code.ToLower() == code.ToLower());
                if (schedulesToDelete != null)
                {
                    _db.Schedules.RemoveRange(schedulesToDelete);
                    await _db.SaveChangesAsync();
                }

                return new OperationResult { Success = true, Message = $"SUCCESS - {schedulesToDelete.Count()} deleted" };
            }
            catch (Exception error)
            {
                return new OperationResult { Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }

        public async Task ProcessBroadcastAsync(Broadcast broadcast, string[] recipients, bool runOnce = true, ScheduleRepeat repeat = ScheduleRepeat.None, int frequency = 0)
        {
            if (runOnce)
            {
                foreach (var recipient in recipients)
                {
                    try
                    {
                        await ScheduleBroadcastAsync(new Schedule
                        {
                            Code = broadcast.Code,
                            CreatedBy = broadcast.CreatedBy,
                            Description = broadcast.Description,
                            From = broadcast.From,
                            Message = broadcast.Message,
                            ScheduleDateTime = broadcast.ScheduleDateTime,
                            Status = broadcast.Status,
                            Subject = broadcast.Subject,
                            To = recipient
                        });
                    }
                    catch (Exception error)
                    {
                        _logger.LogError(error, $"FAILED: Scheduling broadcast {broadcast.Code}:{broadcast.Subject} for {recipient} failed.");
                    }
                }
            }
            else
            {
                switch (repeat)
                {
                    case ScheduleRepeat.None:
                        break;
                    case ScheduleRepeat.Daily:
                        if (frequency >= 1)
                        {
                            var broadcastDate = broadcast.ScheduleDateTime;
                            while (broadcastDate <= broadcast.RepeatEndDate)
                            {
                                foreach (var recipient in recipients)
                                {
                                    try
                                    {
                                        await ScheduleBroadcastAsync(new Schedule
                                        {
                                            Code = broadcast.Code,
                                            CreatedBy = broadcast.CreatedBy,
                                            Description = broadcast.Description,
                                            From = broadcast.From,
                                            Message = broadcast.Message,
                                            ScheduleDateTime = broadcastDate,
                                            Status = broadcast.Status,
                                            Subject = broadcast.Subject,
                                            To = recipient
                                        });
                                    }
                                    catch (Exception error)
                                    {
                                        _logger.LogError(error, $"FAILED: Scheduling broadcast {broadcast.Code}:{broadcast.Subject} for {recipient} failed.");
                                    }
                                }

                                broadcastDate.AddDays(frequency);
                            }
                        }
                        break;
                    case ScheduleRepeat.Weekly:
                        if (frequency >= 1)
                        {
                            var broadcastDate = broadcast.ScheduleDateTime;
                            while (broadcastDate <= broadcast.RepeatEndDate)
                            {
                                foreach (var recipient in recipients)
                                {
                                    try
                                    {
                                        await ScheduleBroadcastAsync(new Schedule
                                        {
                                            Code = broadcast.Code,
                                            CreatedBy = broadcast.CreatedBy,
                                            Description = broadcast.Description,
                                            From = broadcast.From,
                                            Message = broadcast.Message,
                                            ScheduleDateTime = broadcastDate,
                                            Status = broadcast.Status,
                                            Subject = broadcast.Subject,
                                            To = recipient
                                        });
                                    }
                                    catch (Exception error)
                                    {
                                        _logger.LogError(error, $"FAILED: Scheduling broadcast {broadcast.Code}:{broadcast.Subject} for {recipient} failed.");
                                    }
                                }

                                broadcastDate.AddDays(frequency * 7);
                            }
                        }
                        break;
                    case ScheduleRepeat.Monthly:
                        if (frequency >= 1)
                        {
                            var broadcastDate = broadcast.ScheduleDateTime;
                            while (broadcastDate <= broadcast.RepeatEndDate)
                            {
                                foreach (var recipient in recipients)
                                {
                                    try
                                    {
                                        await ScheduleBroadcastAsync(new Schedule
                                        {
                                            Code = broadcast.Code,
                                            CreatedBy = broadcast.CreatedBy,
                                            Description = broadcast.Description,
                                            From = broadcast.From,
                                            Message = broadcast.Message,
                                            ScheduleDateTime = broadcastDate,
                                            Status = broadcast.Status,
                                            Subject = broadcast.Subject,
                                            To = recipient
                                        });
                                    }
                                    catch (Exception error)
                                    {
                                        _logger.LogError(error, $"FAILED: Scheduling broadcast {broadcast.Code}:{broadcast.Subject} for {recipient} failed.");
                                    }
                                }

                                broadcastDate.AddDays(frequency * 30);
                            }
                        }
                        break;
                    case ScheduleRepeat.Yearly:
                        if (frequency >= 1)
                        {
                            var broadcastDate = broadcast.ScheduleDateTime;
                            while (broadcastDate <= broadcast.RepeatEndDate)
                            {
                                foreach (var recipient in recipients)
                                {
                                    try
                                    {
                                        await ScheduleBroadcastAsync(new Schedule
                                        {
                                            Code = broadcast.Code,
                                            CreatedBy = broadcast.CreatedBy,
                                            Description = broadcast.Description,
                                            From = broadcast.From,
                                            Message = broadcast.Message,
                                            ScheduleDateTime = broadcastDate,
                                            Status = broadcast.Status,
                                            Subject = broadcast.Subject,
                                            To = recipient
                                        });
                                    }
                                    catch (Exception error)
                                    {
                                        _logger.LogError(error, $"FAILED: Scheduling broadcast {broadcast.Code}:{broadcast.Subject} for {recipient} failed.");
                                    }
                                }

                                broadcastDate.AddDays(frequency * 365);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public async Task ScheduleBroadcastAsync(Schedule schedule)
        {
            await _db.Schedules.AddAsync(schedule);
            await _db.SaveChangesAsync();
        }
    }
}