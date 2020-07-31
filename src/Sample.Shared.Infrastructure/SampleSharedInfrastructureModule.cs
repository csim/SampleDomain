namespace Sample.Shared.Infrastructure
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sample.Shared.Abstractions;
    using Sample.Shared.Infrastructure.Data;

    public static class SampleSharedInfrastructureModule
    {

        public static void UseSharedInfrastructure()
        {

            //context.Database.Migrate();
        }

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
            if (options.RecordDatabaseType == RecordDatabaseType.SqlLite)
            {
                services
                    .AddDbContext<AppDbContext>(o => o.UseSqlite(options.RecordDatabaseConnection));
            }
            else if (options.RecordDatabaseType == RecordDatabaseType.Cosmos)
            {
                services
                    .AddDbContext<AppDbContext>(o => o.UseCosmos("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "Sample"));
            }
            else
            {
                throw new ApplicationException($"Unknown RecordDatabaseType ({options.RecordDatabaseType})");
            }

            return services
                .AddScoped<IRecordRepository, RecordRepository>()
                .AddScoped<AppDbContext>()
                .AddSingleton(options);
        }
    }
}
