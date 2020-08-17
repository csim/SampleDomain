namespace SampleApp.Orders.Endpoint
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using SampleApp.Orders.Domain;
    using SampleApp.Shared.Infrastructure;
    using Serilog;

    public class Program
    {
        private static IConfigurationRoot _configuration;
        private static string _namespace;

        public void ConfigureServices(IServiceCollection services)
        {
            var infraOptions = GetOptions<InfrastructureOptions>();
            var domainOptions = GetOptions<OrdersDomainOptions>();

            services
                .AddSharedInfrastructure(infraOptions)
                .AddOrdersDomain(domainOptions)
                .AddLogging(options => options.AddSerilog(Log.Logger, dispose: true));
        }

        public static async Task Main(string[] args)
        {
            if (args.Length > 0) Environment.SetEnvironmentVariable("SAMPLEAPP_ENVIRONMENT", args[0]);

            var env = Environment.GetEnvironmentVariable("SAMPLEAPP_ENVIRONMENT") ?? "Development";
            env = env.ToLower();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables("SAMPLEAPP_")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config/appsettings.{env}.secrets.json", optional: true, reloadOnChange: true)
                .Build();

            _namespace = typeof(Program).Namespace;
            if (!string.IsNullOrEmpty(_namespace)) Console.Title = _namespace;

            var host = Host
                .CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(_configuration))
                .ConfigureLogging((ctx, logging) => logging.AddSerilog(Log.Logger, dispose: true))
                .UseNServiceBus(
                    ctx =>
                    {
                        var config = new EndpointConfiguration(_namespace);
                        config.UseTransport<LearningTransport>();
                        config.DefineCriticalErrorAction(OnCriticalError);

                        return config;
                    })
                .ConfigureAppConfiguration(
                    (hostingContext, builder) =>
                    {
                        hostingContext.HostingEnvironment.EnvironmentName = env;
                    })
                .Build();

            await host.RunAsync();
        }

        private TOptions GetOptions<TOptions>() where TOptions : class, new()
        {
            return _configuration
                    .GetSection(typeof(TOptions).Namespace)
                    .Get<TOptions>()
                ?? new TOptions();
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
