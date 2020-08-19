namespace SampleApp.Shared.Infrastructure.Data
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SampleApp.Orders.Client.Records;

    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options) //, IMediator mediator)
            : base(options)
        {
            //_mediator = mediator;
        }

        //public DbSet<OrderItemRecord> OrderItemss { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            //if (_mediator == null) return result;

            // dispatch events only if save was successful
            //var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
            //    .Select(e => e.Entity)
            //    .Where(e => e.Events.Any())
            //    .ToArray();

            //foreach (var entity in entitiesWithEvents)
            //{
            //    var events = entity.Events.ToArray();
            //    entity.Events.Clear();
            //    foreach (var domainEvent in events)
            //    {
            //        await _mediator.Publish(domainEvent).ConfigureAwait(false);
            //    }
            //}

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var recordType in new[] { typeof(OrderRecord), typeof(OrderItemRecord), typeof(OrderAuditRecord) })
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
