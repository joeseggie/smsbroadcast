using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SmsBroadcast.TaskRunner.Repositories
{
    internal class BroadcastsRepository
    {
        internal static async Task<IEnumerable<Broadcast>> GetBroadcastsToSendAsync()
        {
            using (var db = new SqlConnection(HangfireProcessingConfiguration.GetDbSqlServerConnectionString()))
            {
                var query = $"SELECT TOP 100 [Id], [From], [To], [Message], [Code] FROM Schedule WHERE [Status] = 'PENDING' and ScheduleDateTime < GETDATE();";
                var command = db.CreateCommand();
                command.CommandText = query;

                await db.OpenAsync();
                var broadcastsReader = await command.ExecuteReaderAsync();

                var broadcasts = new List<Broadcast>();

                if (broadcastsReader.HasRows)
                {
                    while (await broadcastsReader.ReadAsync())
                    {
                        var broadcastToSend = new Broadcast
                        {
                            Id = broadcastsReader.GetGuid(0),
                            From = broadcastsReader.GetString(1),
                            To = broadcastsReader.GetString(2),
                            Message = broadcastsReader.GetString(3),
                            Code = broadcastsReader.GetString(4)
                        };
                        broadcasts.Add(broadcastToSend);
                    }
                }

                return broadcasts;
            }
        }

        internal static async Task MarkBroadcastsAsSentAsync(Guid broadcastId)
        {
            using (var db = new SqlConnection(HangfireProcessingConfiguration.GetDbSqlServerConnectionString()))
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"UPDATE Schedule SET [Status] = 'SENT' WHERE [Id] = '{broadcastId}';");

                var command = db.CreateCommand();
                command.CommandText = $"{queryBuilder.ToString()}";

                await db.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        internal static async Task MarkBroadcastsAsFailedAsync(Guid broadcastId)
        {
            using (var db = new SqlConnection(HangfireProcessingConfiguration.GetDbSqlServerConnectionString()))
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"UPDATE Schedule SET [Status] = 'FAILED' WHERE [Id] = '{broadcastId}';");

                var command = db.CreateCommand();
                command.CommandText = $"{queryBuilder.ToString()}";

                await db.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
