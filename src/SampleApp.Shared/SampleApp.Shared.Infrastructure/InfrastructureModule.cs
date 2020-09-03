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
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions.Blobs;
    using SampleApp.Shared.Abstractions.Records;
    using SampleApp.Shared.Infrastructure.Blobs;
    using SampleApp.Shared.Infrastructure.Blobs.Orders;
    using SampleApp.Shared.Infrastructure.Extensions;
    using SampleApp.Shared.Infrastructure.Records.Orders;
    using Serilog;

    public static class InfrastructureModule
    {
        public static IHostBuilder AddSharedInfrastructure(this IHostBuilder hostBuilder, IConfiguration configuration, string endpointName)
        {
            var workerEndpointName = "SampleApp.Shared.Worker";

            var messageRouteTable = new[] { new { EndpointName = workerEndpointName, typeof(OrdersClientModule).Assembly } };

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
                        //config.DisableFeature<AutoSubscribe>();
                        config.EnableCallbacks();
                        config.EnableInstallers();

                        //var routing = config
                        //    .UseTransport<SqlServerTransport>()
                        //    .ConnectionString("Server=localhost; Database=SampleApp; User Id=sampleapp;Password=sampleapp;")
                        //    .DefaultSchema("transport")
                        //    .Routing();

                        var routing = config
                            .UseTransport<LearningTransport>()
                            .Routing();

                        foreach (var route in messageRouteTable)
                        {
                            routing.RouteToEndpoint(route.Assembly, route.EndpointName);
                        }

                        return config;
                    });
        }

        public static void Initialize(IHost host)
        {
            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<OrdersDbContext>().Database.EnsureCreated();
        }

        private static IServiceCollection AddBlobRepository<TBlobRepository, TFileSystemImplementation, TAzureStorageImplementation>(
            this IServiceCollection services,
            BlobRepositoryOptions options)
            where TBlobRepository : class, IBlobRepository
            where TFileSystemImplementation : class, TBlobRepository
            where TAzureStorageImplementation : class, TBlobRepository
        {
            var implementationType = options.Mode == BlobRespositoryMode.FileSystem ? typeof(TFileSystemImplementation)
                : options.Mode == BlobRespositoryMode.AzureStorage ? typeof(TAzureStorageImplementation)
                : throw new ApplicationException($"Invalid BlobRepository mode ({options.Mode})");

            services
                .AddScoped(implementationType)
                .AddScoped(
                    serviceProvider =>
                    {
                        var instance = (TBlobRepository)serviceProvider.GetRequiredService(implementationType);

                        var instanceFileSys = instance as FileSystemBlobRepository;
                        instanceFileSys?.SetBasePath(options.Connection);

                        var instanceAzure = instance as AzureStorageBlobRepository;
                        instanceAzure?.SetConnection(options.Connection);

                        return instance;
                    });

            return services;
        }

        private static IServiceCollection AddRecordRepository<TRecordRepository, TCosmosDbContext, TCosmosImplementation>(
            this IServiceCollection services,
            RecordRepositoryOptions options)
            where TRecordRepository : class, IRecordRepository
            where TCosmosDbContext : DbContext
            where TCosmosImplementation : class, TRecordRepository
        {
            var connection = options.Connection.ParseSeparated();

            if (options.Mode == RecordRespositoryMode.Cosmos)
            {
                services
                    .AddDbContext<TCosmosDbContext>(
                        o => o.UseCosmos(
                            connection["AccountEndpoint"],
                            connection["AccountKey"],
                            connection["DatabaseName"]))
                    .AddScoped<TRecordRepository, TCosmosImplementation>();
            }
            else
            {
                throw new ApplicationException($"Invalid RecordRepository mode ({options.Mode})");
            }

            return services;
        }

        private static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var options = GetOptions<InfrastructureOptions>(configuration);

            var orderClientOptions = GetOptions<OrdersClientOptions>(configuration);

            return services
                .AddLogging(loggingOptions => loggingOptions.AddSerilog(Log.Logger, dispose: true))
                .AddSingleton(options)
                .AddScoped<FileSystemBlobRepository>()
                .AddScoped<AzureStorageBlobRepository>()
                .AddRecordRepository<IOrdersRecordRepository, OrdersDbContext, OrdersRecordRepository>(orderClientOptions.Records)
                .AddBlobRepository<IOrdersBlobRepository, OrdersFileSystemBlobRepository, OrdersAzureStorageBlobRepository>(orderClientOptions.Blobs);
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
