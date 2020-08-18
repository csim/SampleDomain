namespace SampleApp.Shared.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using SampleApp.Orders.Client;
    using SampleApp.Shared.Abstractions;
    using SampleApp.Shared.Infrastructure.Blob;
    using SampleApp.Shared.Infrastructure.Data;
    using Serilog;

    public static class InfrastructureModule
    {
        public static IHostBuilder AddSharedInfrastructure(this IHostBuilder hostBuilder, IConfiguration configuration, string endpointName)
        {
            var workerEndpointName = "SampleApp.Shared.Worker";

            var routeTable = new[] { new { EndpointName = workerEndpointName, typeof(OrdersClientModule).Assembly } };

            return hostBuilder
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureLogging((ctx, logging) => logging.AddSerilog(Log.Logger, dispose: true))
                .ConfigureServices(
                    services =>
                    {
                        services
                            .AddSharedInfrastructure(configuration)
                            .AddLogging(options => options.AddSerilog(Log.Logger, dispose: true));
                    })
                .UseNServiceBus(
                    ctx =>
                    {
                        var config = new EndpointConfiguration(endpointName);
                        config.DefineCriticalErrorAction(OnCriticalError);
                        config.MakeInstanceUniquelyAddressable(Guid.NewGuid().ToString("N"));
                        config.EnableCallbacks();

                        var routing = config
                            .UseTransport<LearningTransport>()
                            .Routing();

                        foreach (var route in routeTable)
                        {
                            routing.RouteToEndpoint(route.Assembly, route.EndpointName);
                        }

                        return config;
                    });
        }

        public static void Initialize(IHost host, IConfiguration configuration)
        {
            var options = GetOptions<InfrastructureOptions>(configuration);

            if (options.RecordRepositoryMode == RecordRepositoryMode.Cosmos)
            {
                using var scope = host.Services.CreateScope();
                scope.ServiceProvider.GetRequiredService<CosmosDbContext>().Database.EnsureCreated();
            }
        }

        private static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var options = GetOptions<InfrastructureOptions>(configuration);

            if (options.RecordRepositoryMode == RecordRepositoryMode.SqlLite)
            {
                services
                    .AddDbContext<CosmosDbContext>(o => o.UseSqlite(options.SqlLiteConnection));
            }
            else if (options.RecordRepositoryMode == RecordRepositoryMode.Cosmos)
            {
                services
                    .AddDbContext<CosmosDbContext>(
                        o => o.UseCosmos(
                            options.CosmosConnection.AccountEndpoint,
                            options.CosmosConnection.AccountKey,
                            options.CosmosConnection.DatabaseName))
                    .AddScoped<IRecordRepository, CosmosRecordRepository>();
            }
            else if (options.RecordRepositoryMode == RecordRepositoryMode.InMemory)
            {
                services
                    .AddDbContext<CosmosDbContext>(o => o.UseInMemoryDatabase(databaseName: "Sample"))
                    .AddScoped<IRecordRepository, CosmosRecordRepository>();
            }
            else
            {
                throw new ApplicationException($"Unknown RecordDatabaseType ({options.RecordRepositoryMode})");
            }


            if (options.BlobRespositoryMode == BlobRespositoryMode.AzureStorage)
            {
                services
                    .AddScoped<AzureStorageBlobRepository>()
                    .AddScoped<IBlobRepository, AzureStorageBlobRepository>(
                        serviceProvider =>
                        {
                            var instance = serviceProvider.GetRequiredService<AzureStorageBlobRepository>();
                            instance.SetConnection(options.AzureStorageAccountConnection);
                            return instance;
                        });
            }
            else if (options.BlobRespositoryMode == BlobRespositoryMode.FileSystem)
            {
                services
                    .AddScoped<FileSystemBlobRepository>()
                    .AddScoped<IBlobRepository, FileSystemBlobRepository>(
                        serviceProvider =>
                        {
                            var instance = serviceProvider.GetRequiredService<FileSystemBlobRepository>();
                            instance.SetBasePath(options.FileSystemBlobBasePath);
                            return instance;
                        });
            }

            return services
                .AddLogging(loggingOptions => loggingOptions.AddSerilog(Log.Logger, dispose: true))
                .AddSingleton(options);
        }

        private static TOptions GetOptions<TOptions>(IConfiguration config) where TOptions : class, new()
        {
            return config
                    .GetSection(typeof(TOptions).Namespace)
                    .Get<TOptions>()
                ?? new TOptions();
        }

        private static async Task OnCriticalError(ICriticalErrorContext context)
        {
            var fatalMessage = "The following critical error was "
                + $"encountered: {Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. "
                + $"StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";

            //EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);

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
