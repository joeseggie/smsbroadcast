using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmsBroadcast.TaskRunner
{
    public class HangfireProcessingConfiguration
    {

        public static string GetHangfireSqlServerConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            return configuration.GetConnectionString("HangfireConnection");
        }

        internal static string GetDbSqlServerConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
