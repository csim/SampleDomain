namespace SampleApp.Web
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using SampleApp.Orders.Client;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            var ns = typeof(Program).Namespace;

            var host = Host
                    .CreateDefaultBuilder(args)
                    .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                    .UseNServiceBus(
                        ctx =>
                        {
                            var config = new EndpointConfiguration(ns);
                            config.UseTransport<LearningTransport>();
                            config.DefineCriticalErrorAction(OnCriticalError);

                            config
                                .UseTransport<LearningTransport>()
                                .Routing()
                                .AddOrdersClient();

                            return config;
                        })
                    .ConfigureWebHostDefaults(
                        webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        })
                ;

            host.Build().Run();
        }

        private static async Task OnCriticalError(ICriticalErrorContext context)
        {
            var fatalMessage = "The following critical error was "
                + $"encountered: {Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. "
                + $"StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";

            EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);

            try
            {
                await context.Stop().ConfigureAwait(false);
            }
            finally
            {
                Environment.FailFast(fatalMessage, context.Exception);
            }
        }
    }
}
