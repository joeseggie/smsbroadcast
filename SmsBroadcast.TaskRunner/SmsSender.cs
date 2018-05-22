using Microsoft.Extensions.Configuration;
using SmsBroadcast.TaskRunner.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmsBroadcast.TaskRunner
{
    public class SmsSender : ISmsSender
    {
        public IConfiguration Configuration { get; }

        public SmsSender()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");

            Configuration = builder.Build();
        }

        [DisplayName("Sending broadcast {0}")]
        public async Task SendMessageAsync(string broadcast, string from, string to, string message, Guid broadcastId)
        {
            using (var httpClient = new HttpClient())
            {
                message = message.Replace(' ', '+');

                httpClient.BaseAddress = new Uri($"{SmsSenderApiBaseUrl()}");
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                var requestResponse = await httpClient.GetAsync($"cgi-bin/sendsms?to=256{to}&from={from}&text={message}&username={SmsSenderApiUsername()}&password={SmsSenderApiPassword()}&smsc={SmsSenderApiSmsc()}");
                var sendSmsResponse = await requestResponse.Content.ReadAsStringAsync();

                var operationResult = new OperationResult();

                if (sendSmsResponse.ToLower() == "0: Accepted for delivery".ToLower())
                {
                    Console.WriteLine($"{sendSmsResponse} - {requestResponse.RequestMessage.RequestUri}");
                    await BroadcastsRepository.MarkBroadcastsAsSentAsync(broadcastId);
                }
                else
                {
                    Console.WriteLine($"{sendSmsResponse} - {requestResponse.RequestMessage.RequestUri}");
                    await BroadcastsRepository.MarkBroadcastsAsFailedAsync(broadcastId);
                }
            }
        }

        private string SmsSenderApiBaseUrl() => $"{Configuration["SmsSender:BaseUrl"]}";

        private string SmsSenderApiUsername() => $"{Configuration["SmsSender:Username"]}";

        private string SmsSenderApiPassword() => $"{Configuration["SmsSender:Password"]}";

        private string SmsSenderApiSmsc() => $"{Configuration["SmsSender:SMSC"]}";
    }
}
