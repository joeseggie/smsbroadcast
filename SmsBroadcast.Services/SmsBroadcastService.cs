using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SmsBroadcast.Infrastructure;
using SmsBroadcast.Infrastructure.Data;
using SmsBroadcast.Infrastructure.Entities;

namespace SmsBroadcast.Services
{
    public class SmsBroadcastService : ISmsBroadcast
    {
        private readonly SmsBroadcastContext _db;

        public SmsBroadcastService(SmsBroadcastContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Schedule one time broadcast.
        /// </summary>
        /// <param name="recepients">List of recepients.</param>
        /// <param name="broadcastSchedule">Broadcast schedule details.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> BroadcastOnceAsync(string[] recipients, Schedule broadcastSchedule)
        {
            try
            {
                List<Schedule> schedules = new List<Schedule>();

                foreach (var recipient in recipients)
                {
                    if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                    {
                        var scheduleToAdd = new Schedule();
                        scheduleToAdd = broadcastSchedule;
                        scheduleToAdd.ScheduleDateTime = DateTimeOffset.Now;
                        scheduleToAdd.To = recipient;

                        schedules.Add(scheduleToAdd);
                    }
                }

                await _db.Schedules.AddRangeAsync(schedules);
                await _db.SaveChangesAsync();

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedules.Count} recipients to receive broadcast." };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
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
                if(schedulesToDelete != null)
                {
                    _db.Schedules.RemoveRange(schedulesToDelete);
                    await _db.SaveChangesAsync();
                }

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedulesToDelete.Count()} deleted" };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }

        /// <summary>
        /// Schedule a daily broadcast.
        /// </summary>
        /// <param name="recipients">List of recipients.</param>
        /// <param name="broadcastSchedule">Broadcast schedule details.</param>
        /// <param name="frequency">Frequency of the schedule occurence.</param>
        /// <param name="broadcastDate">Broadcast start date and time.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> DailyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate)
        {
            int frequencyCounter = 0;
            try
            {
                List<Schedule> schedules = new List<Schedule>();

                while (frequencyCounter < frequency)
                {
                    foreach (var recipient in recipients)
                    {
                        if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                        {
                            var scheduleToAdd = new Schedule();
                            scheduleToAdd = broadcastSchedule;
                            scheduleToAdd.ScheduleDateTime = broadcastDate.AddDays((double)frequencyCounter);
                            scheduleToAdd.To = recipient;

                            schedules.Add(scheduleToAdd);
                        }
                    }

                    frequencyCounter += 1;
                }

                await _db.Schedules.AddRangeAsync(schedules);
                await _db.SaveChangesAsync();

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedules.Count} recipients to receive broadcast." };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }

        /// <summary>
        /// Schedule a monthly broadcast.
        /// </summary>
        /// <param name="recipients">List of recipients.</param>
        /// <param name="broadcastSchedule">Broadcast schedule details.</param>
        /// <param name="frequency">Frequency of the schedule occurence.</param>
        /// <param name="broadcastDate">Broadcast start date and time.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> MonthlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate)
        {
            int frequencyCounter = 0;
            try
            {
                List<Schedule> schedules = new List<Schedule>();

                while (frequencyCounter < frequency)
                {
                    foreach (var recipient in recipients)
                    {
                        if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                        {
                            var scheduleToAdd = new Schedule();
                            scheduleToAdd = broadcastSchedule;
                            scheduleToAdd.ScheduleDateTime = broadcastDate.AddMonths(frequencyCounter);
                            scheduleToAdd.To = recipient;

                            schedules.Add(scheduleToAdd);
                        }
                    }

                    frequencyCounter += 1;
                }

                await _db.Schedules.AddRangeAsync(schedules);
                await _db.SaveChangesAsync();

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedules.Count} recipients to receive broadcast." };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }

        /// <summary>
        /// Schedule a one time future broadcast.
        /// </summary>
        /// <param name="recipients">List of recipients</param>
        /// <param name="broadcastSchedule">Broadcast schedule details.</param>
        /// <param name="broadcastDate">Broadcast start date and time.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> ScheduleBroadcastAsync(string[] recipients, Schedule broadcastSchedule, DateTimeOffset broadcastDate)
        {
            try
            {
                List<Schedule> schedules = new List<Schedule>();
                foreach (var recipient in recipients)
                {
                    if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                    {
                        var scheduleToAdd = new Schedule();
                        scheduleToAdd = broadcastSchedule;
                        scheduleToAdd.ScheduleDateTime = broadcastDate;
                        scheduleToAdd.To = recipient;

                        schedules.Add(scheduleToAdd);
                    }
                }

                await _db.Schedules.AddRangeAsync(schedules);
                await _db.SaveChangesAsync();

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedules.Count} recipients to receive broadcast." };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }

        /// <summary>
        /// Schedule a weekly broadcast.
        /// </summary>
        /// <param name="recipients">List of recipients.</param>
        /// <param name="broadcastSchedule">Broadcast schedule details.</param>
        /// <param name="frequency">Frequency of the schedule occurence.</param>
        /// <param name="broadcastDate">Broadcast start date and time.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> WeeklyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate)
        {
            int frequencyCounter = 0;
            try
            {
                List<Schedule> schedules = new List<Schedule>();

                while (frequencyCounter < frequency)
                {
                    foreach (var recipient in recipients)
                    {
                        if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                        {
                            var scheduleToAdd = new Schedule();
                            scheduleToAdd = broadcastSchedule;
                            scheduleToAdd.ScheduleDateTime = broadcastDate.AddDays(frequencyCounter * 7.0);
                            scheduleToAdd.To = recipient;

                            schedules.Add(scheduleToAdd);
                        }
                    }

                    frequencyCounter += 1;
                }

                await _db.Schedules.AddRangeAsync(schedules);
                await _db.SaveChangesAsync();

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedules.Count} recipients to receive broadcast." };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }

        /// <summary>
        /// Schedule a yearly broadcast.
        /// </summary>
        /// <param name="recipients">List of recipients.</param>
        /// <param name="broadcastSchedule">Broadcast schedule details.</param>
        /// <param name="frequency">Frequency of the schedule occurence.</param>
        /// <param name="broadcastDate">Broadcast start date and time.</param>
        /// <returns>Boolean value indicating whether the operation was a success or not.</returns>
        public async Task<OperationResult> YearlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate)
        {
            int frequencyCounter = 0;
            try
            {
                List<Schedule> schedules = new List<Schedule>();

                while (frequencyCounter < frequency)
                {
                    foreach (var recipient in recipients)
                    {
                        if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                        {
                            var scheduleToAdd = new Schedule();
                            scheduleToAdd = broadcastSchedule;
                            scheduleToAdd.ScheduleDateTime = broadcastDate.AddYears(frequencyCounter);
                            scheduleToAdd.To = recipient;

                            schedules.Add(scheduleToAdd);
                        }
                    }

                    frequencyCounter += 1;
                }

                await _db.Schedules.AddRangeAsync(schedules);
                await _db.SaveChangesAsync();

                return new OperationResult{ Success = true, Message = $"SUCCESS - {schedules.Count} recipients to receive broadcast." };
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }
    }
}