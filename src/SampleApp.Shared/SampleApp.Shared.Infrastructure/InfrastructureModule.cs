namespace SampleApp.Shared.Infrastructure
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SampleApp.Shared.Abstractions;
    using SampleApp.Shared.Infrastructure.Blob;
    using SampleApp.Shared.Infrastructure.Data;

    public static class InfrastructureModule
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, InfrastructureOptions options)
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
            else if (options.RecordRepositoryMode == RecordRepositoryMode.InMemory)
            {
                services
                    .AddDbContext<CosmosDbContext>(
                        o => o.UseInMemoryDatabase(databaseName: "Sample")
                    )
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
