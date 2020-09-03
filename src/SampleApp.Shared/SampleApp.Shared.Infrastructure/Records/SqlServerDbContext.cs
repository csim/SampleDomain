namespace SampleApp.Shared.Infrastructure.Records
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SampleApp.Orders.Client;
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions.Records;

    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
        {
        }

        public DbSet<OrderAuditRecord> OrderAudit { get; set; }

        public DbSet<OrderRecord> Orders { get; set; }

        public DbSet<ProductRecord> Product { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var recordBaseType = typeof(RecordBase);
            var scanAssemblies = new[] { typeof(OrdersClientModule).Assembly };

            foreach (Assembly assembly in scanAssemblies)
            {
                var recordTypes = assembly
                    .GetTypes()
                    .Where(t => t.Name != recordBaseType.Name && recordBaseType.IsAssignableFrom(t));

                foreach (var recordType in recordTypes)
                {
                    modelBuilder
                        .Entity(recordType)
                        .HasPartitionKey("PartitionKey")
                        .HasKey("Id");
                }
            }
        }
    }
}
