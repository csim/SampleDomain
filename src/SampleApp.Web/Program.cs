namespace SampleApp.Web
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using SampleApp.Shared.Infrastructure;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0) Environment.SetEnvironmentVariable("SAMPLEAPP_ENVIRONMENT", args[0]);

            var env = Environment.GetEnvironmentVariable("SAMPLEAPP_ENVIRONMENT") ?? "Development";
            env = env.ToLower();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables("SAMPLEAPP_")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config/appsettings.{env}.secrets.json", optional: true, reloadOnChange: true)
                .Build();

            var host = Host
                    .CreateDefaultBuilder(args)
                    .AddSharedInfrastructure(configuration, typeof(Program).Namespace)
                    .ConfigureWebHostDefaults(
                        webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        })
                ;

            host.Build().Run();
        }
    }
}
