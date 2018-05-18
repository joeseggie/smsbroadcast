using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SmsBroadcast.Core;
using SmsBroadcast.Core.Data;
using SmsBroadcast.Core.Entities;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SmsBroadcast.Core.Services
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
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"INSERT INTO Schedule ([Id], [CreatedBy], [Description], [From], [Message], [Status], [Subject], [Code], [ScheduleDateTime], [To]) VALUES ");
                using (_db)
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    foreach (var recipient in recipients)
                    {
                        if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                        {

                            broadcastSchedule.ScheduleDateTime = DateTimeOffset.Now;
                            broadcastSchedule.To = recipient;

                            queryBuilder.Append($"('{Guid.NewGuid()}', '{broadcastSchedule.CreatedBy}', '{broadcastSchedule.Description}', '{broadcastSchedule.From}', '{broadcastSchedule.Message}', '{broadcastSchedule.Status}', '{broadcastSchedule.Subject}', '{broadcastSchedule.Code}', '{broadcastSchedule.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{broadcastSchedule.To}'),");
                        }
                    }

                    queryBuilder.Length--; // Remove the last character (,) from the query string builder.
                    command.CommandText = $"{queryBuilder.ToString()};"; // Add the statement termination character (;)

                    await _db.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();
                    _db.Database.CloseConnection();

                    return new OperationResult { Success = true, Message = $"SUCCESS - {recipients.Count()} recipients to receive broadcast." };
                }
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
        public async Task<OperationResult> DailyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate)
        {
            try
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"INSERT INTO Schedule ([Id], [CreatedBy], [Description], [From], [Message], [Status], [Subject], [Code], [ScheduleDateTime], [To]) VALUES ");
                using (_db)
                using(var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    foreach (var recipient in recipients)
                    {
                        int frequencyCounter = 0;
                        var scheduleDateTime = broadcastDate.AddDays(frequencyCounter);

                        while (repeatEndDate > scheduleDateTime)
                        {
                            if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                            {
                                broadcastSchedule.ScheduleDateTime = scheduleDateTime;
                                broadcastSchedule.To = recipient;

                                queryBuilder.Append($"('{Guid.NewGuid()}', '{broadcastSchedule.CreatedBy}', '{broadcastSchedule.Description}', '{broadcastSchedule.From}', '{broadcastSchedule.Message}', '{broadcastSchedule.Status}', '{broadcastSchedule.Subject}', '{broadcastSchedule.Code}', '{broadcastSchedule.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{broadcastSchedule.To}'),");
                            }
                            frequencyCounter += 1;
                            scheduleDateTime = broadcastDate.AddDays(frequencyCounter);
                        }
                    }

                    queryBuilder.Length--; // Remove the last character (,) from the query string builder.
                    command.CommandText = $"{queryBuilder.ToString()};"; // Add the statement termination character (;)

                    await _db.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();
                    _db.Database.CloseConnection();

                    return new OperationResult { Success = true, Message = $"SUCCESS - {recipients.Count()} recipients to receive broadcast." };
                }
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
        public async Task<OperationResult> MonthlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate)
        {
            try
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"INSERT INTO Schedule ([Id], [CreatedBy], [Description], [From], [Message], [Status], [Subject], [Code], [ScheduleDateTime], [To]) VALUES ");
                using (_db)
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    foreach (var recipient in recipients)
                    {
                        int frequencyCounter = 0;
                        var scheduleDateTime = broadcastDate.AddMonths(frequencyCounter);

                        while (repeatEndDate > scheduleDateTime)
                        {
                            if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                            {
                                broadcastSchedule.ScheduleDateTime = scheduleDateTime;
                                broadcastSchedule.To = recipient;

                                queryBuilder.Append($"('{Guid.NewGuid()}', '{broadcastSchedule.CreatedBy}', '{broadcastSchedule.Description}', '{broadcastSchedule.From}', '{broadcastSchedule.Message}', '{broadcastSchedule.Status}', '{broadcastSchedule.Subject}', '{broadcastSchedule.Code}', '{broadcastSchedule.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{broadcastSchedule.To}'),");
                            }
                            frequencyCounter += 1;
                            scheduleDateTime = broadcastDate.AddDays(frequencyCounter);
                        }
                    }

                    queryBuilder.Length--; // Remove the last character (,) from the query string builder.
                    command.CommandText = $"{queryBuilder.ToString()};"; // Add the statement termination character (;)

                    await _db.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();
                    _db.Database.CloseConnection();

                    return new OperationResult { Success = true, Message = $"SUCCESS - {recipients.Count()} recipients to receive broadcast." };
                }
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
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"INSERT INTO Schedule ([Id], [CreatedBy], [Description], [From], [Message], [Status], [Subject], [Code], [ScheduleDateTime], [To]) VALUES ");
                using (_db)
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    foreach (var recipient in recipients)
                    {
                        if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                        {
                            broadcastSchedule.ScheduleDateTime = broadcastDate;
                            broadcastSchedule.To = recipient;

                            queryBuilder.Append($"('{Guid.NewGuid()}', '{broadcastSchedule.CreatedBy}', '{broadcastSchedule.Description}', '{broadcastSchedule.From}', '{broadcastSchedule.Message}', '{broadcastSchedule.Status}', '{broadcastSchedule.Subject}', '{broadcastSchedule.Code}', '{broadcastSchedule.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{broadcastSchedule.To}'),");
                        }
                    }

                    queryBuilder.Length--; // Remove the last character (,) from the query string builder.
                    command.CommandText = $"{queryBuilder.ToString()};"; // Add the statement termination character (;)

                    await _db.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();
                    _db.Database.CloseConnection();

                    return new OperationResult { Success = true, Message = $"SUCCESS - {recipients.Count()} recipients to receive broadcast." };
                }
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
        public async Task<OperationResult> WeeklyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate)
        {
            try
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"INSERT INTO Schedule ([Id], [CreatedBy], [Description], [From], [Message], [Status], [Subject], [Code], [ScheduleDateTime], [To]) VALUES ");
                using (_db)
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    foreach (var recipient in recipients)
                    {
                        int frequencyCounter = 0;
                        var scheduleDateTime = broadcastDate.AddDays(frequencyCounter * 7);

                        while (repeatEndDate > scheduleDateTime)
                        {
                            if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                            {
                                broadcastSchedule.ScheduleDateTime = scheduleDateTime;
                                broadcastSchedule.To = recipient;

                                queryBuilder.Append($"('{Guid.NewGuid()}', '{broadcastSchedule.CreatedBy}', '{broadcastSchedule.Description}', '{broadcastSchedule.From}', '{broadcastSchedule.Message}', '{broadcastSchedule.Status}', '{broadcastSchedule.Subject}', '{broadcastSchedule.Code}', '{broadcastSchedule.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{broadcastSchedule.To}'),");
                            }
                            frequencyCounter += 1;
                            scheduleDateTime = broadcastDate.AddDays(frequencyCounter);
                        }
                    }

                    queryBuilder.Length--; // Remove the last character (,) from the query string builder.
                    command.CommandText = $"{queryBuilder.ToString()};"; // Add the statement termination character (;)

                    await _db.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();
                    _db.Database.CloseConnection();

                    return new OperationResult { Success = true, Message = $"SUCCESS - {recipients.Count()} recipients to receive broadcast." };
                }
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
        public async Task<OperationResult> YearlyBroadcastAsync(string[] recipients, Schedule broadcastSchedule, int frequency, DateTimeOffset broadcastDate, DateTimeOffset repeatEndDate)
        {
            try
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"INSERT INTO Schedule ([Id], [CreatedBy], [Description], [From], [Message], [Status], [Subject], [Code], [ScheduleDateTime], [To]) VALUES ");
                using (_db)
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    foreach (var recipient in recipients)
                    {
                        int frequencyCounter = 0;
                        var scheduleDateTime = broadcastDate.AddYears(frequencyCounter);

                        while (repeatEndDate > scheduleDateTime)
                        {
                            if (Regex.IsMatch(recipient, "^71[0-9]{7}$"))
                            {
                                broadcastSchedule.ScheduleDateTime = scheduleDateTime;
                                broadcastSchedule.To = recipient;

                                queryBuilder.Append($"('{Guid.NewGuid()}', '{broadcastSchedule.CreatedBy}', '{broadcastSchedule.Description}', '{broadcastSchedule.From}', '{broadcastSchedule.Message}', '{broadcastSchedule.Status}', '{broadcastSchedule.Subject}', '{broadcastSchedule.Code}', '{broadcastSchedule.ScheduleDateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{broadcastSchedule.To}'),");
                            }
                            frequencyCounter += 1;
                            scheduleDateTime = broadcastDate.AddDays(frequencyCounter);
                        }
                    }

                    queryBuilder.Length--; // Remove the last character (,) from the query string builder.
                    command.CommandText = $"{queryBuilder.ToString()};"; // Add the statement termination character (;)

                    await _db.Database.OpenConnectionAsync();
                    await command.ExecuteNonQueryAsync();
                    _db.Database.CloseConnection();

                    return new OperationResult { Success = true, Message = $"SUCCESS - {recipients.Count()} recipients to receive broadcast." };
                }
            }
            catch (Exception error)
            {
                return new OperationResult{ Success = false, Message = $"FAILED with exception {error.Message}" };
            }
        }
    }
}