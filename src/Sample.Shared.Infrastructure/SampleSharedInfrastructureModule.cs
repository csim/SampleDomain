namespace Sample.Shared.Infrastructure
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sample.Domain.Records;
    using Sample.Shared.Abstractions;
    using Sample.Shared.Infrastructure.Data;

    public static class SampleSharedInfrastructureModule
    {
        public static readonly Type[] RecordTypes = { typeof(ToDoItemRecord) };

        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var moduleType = typeof(SampleSharedInfrastructureModule);
            var options = configuration
                    .GetSection(moduleType.Namespace)
                    .Get<SampleSharedInfrastructureOptions>()
                ?? new SampleSharedInfrastructureOptions();

            return services
                .AddSharedInfrastructure(options);
        }

        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, SampleSharedInfrastructureOptions options)
        {
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
                .AddSingleton(options);
        }
    }
}
