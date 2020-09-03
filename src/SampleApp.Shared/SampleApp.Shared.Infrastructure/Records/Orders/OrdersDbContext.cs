namespace SampleApp.Shared.Infrastructure.Records.Orders
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SampleApp.Orders.Client;
    using SampleApp.Shared.Abstractions.Records;

    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }

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
                        .ToContainer(recordType.Name.Replace("Record", ""))
                        .HasPartitionKey("PartitionKey")
                        .HasKey("Id");
                }
            }
        }
    }
}
