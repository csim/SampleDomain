namespace Sample.Shared.Infrastructure.Data
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.EFCore.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Sample.Domain.Records;

    public class CosmosDbContext : DbContext
    {
        //private readonly IMediator _mediator;

        //public AppDbContext(DbContextOptions options) : base(options)
        //{
        //}

        public CosmosDbContext(DbContextOptions<CosmosDbContext> options) //, IMediator mediator)
            : base(options)
        {
            //_mediator = mediator;
        }

        public DbSet<ToDoItemRecord> ToDoItems { get; set; }

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

            var records = new[] { typeof(ToDoItemRecord) };

            foreach (var recordType in SampleSharedInfrastructureModule.RecordTypes)
            {
                modelBuilder
                    .Entity(recordType)
                    .ToContainer(recordType.Name.Replace("Record", ""))
                    .HasPartitionKey("PartitionKey");
            }

            modelBuilder
                .Entity<ToDoItemRecord>()
                .Ignore(_ => _.Events);

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

            // alternately this is built-in to EF Core 2.2
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
