using Hangfire;
using SmsBroadcast.TaskRunner.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmsBroadcast.TaskRunner
{
    class HangfireProcessing
    {
        public static async Task HandleRequestAsync()
        {
            while (true)
            {
                var broadcastsToBeSent = await BroadcastsRepository.GetBroadcastsToSendAsync();

                var smsSendSuccess = new List<Guid>();
                var smsSendFailure = new List<Guid>();

                var smsSender = new SmsSender();

                foreach (var broadcast in broadcastsToBeSent)
                {
                    BackgroundJob.Enqueue(() => smsSender.SendMessageAsync($"{broadcast.Code} to {broadcast.To}", broadcast.From, broadcast.To, broadcast.Message, broadcast.Id));
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
