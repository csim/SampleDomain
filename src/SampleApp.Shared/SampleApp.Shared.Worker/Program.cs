namespace SampleApp.Shared.Worker
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using SampleApp.Orders.Client;
    using SampleApp.Orders.Domain;
    using SampleApp.Shared.Infrastructure;

    public class Program
    {
        private static IConfigurationRoot _configuration;

        public static async Task Main(string[] args)
        {
            if (args.Length > 0) Environment.SetEnvironmentVariable("SAMPLEAPP_ENVIRONMENT", args[0]);

            var env = Environment.GetEnvironmentVariable("SAMPLEAPP_ENVIRONMENT") ?? "Development";
            env = env.ToLower();

            var endpointName = typeof(Program).Namespace;
            if (!string.IsNullOrEmpty(endpointName)) Console.Title = $"{endpointName} [{env}]";

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables("SAMPLEAPP_")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config/appsettings.{env}.secrets.json", optional: true, reloadOnChange: true)
                .Build();

            var ordersDomainOptions = GetOptions<OrdersDomainOptions>(_configuration);
            var ordersClientOptions = GetOptions<OrdersClientOptions>(_configuration);

            var host = Host
                .CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .AddSharedInfrastructure(_configuration, endpointName)
                .ConfigureServices(
                    services =>
                    {
                        services
                            .AddOrdersDomain(ordersDomainOptions)
                            .AddOrdersClient(ordersClientOptions);
                    })
                .ConfigureAppConfiguration(
                    (hostingContext, builder) =>
                    {
                        hostingContext.HostingEnvironment.EnvironmentName = env;
                    })
                .Build();

            InfrastructureModule.Initialize(host, _configuration);

            await host.RunAsync();
        }

        private static TOptions GetOptions<TOptions>(IConfiguration config) where TOptions : class, new()
        {
            return config
                    .GetSection(typeof(TOptions).Namespace)
                    .Get<TOptions>()
                ?? new TOptions();
        }
    }
}
