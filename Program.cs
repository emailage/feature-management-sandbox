using System;
using Amazon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FeatureManagementSandbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("ClientID", Guid.NewGuid().ToString());
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
          Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddFeatureFlagsConfiguration();
                config.Build();
            });
    }
}
