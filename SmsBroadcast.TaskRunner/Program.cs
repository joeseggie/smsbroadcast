using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsBroadcast.TaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var hangfireSqlServerConnectionString = HangfireProcessingConfiguration.GetHangfireSqlServerConnectionString();
            GlobalConfiguration.Configuration.UseSqlServerStorage(hangfireSqlServerConnectionString);

            using (var hangfireServer = new BackgroundJobServer())
            {
                // TODO: Log starting of Hangfire background server.
                Console.WriteLine("Starting hangfire server");

                HangfireProcessing.HandleRequestAsync().GetAwaiter().GetResult();
            }
        }
    }
}
