namespace SampleApp.Orders.Domain.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client.Commands;
    using SampleApp.Orders.Client.Events;
    using SampleApp.Orders.Client.Records;

    public class SubmitOrderHandler : IHandleMessages<SubmitOrderCommand>
    {
        public SubmitOrderHandler(IOrdersRecordRepository repository, ILogger<SubmitOrderHandler> log)
        {
            _repository = repository;
            _log = log;
        }

        private readonly ILogger<SubmitOrderHandler> _log;

        private readonly IOrdersRecordRepository _repository;

        public async Task Handle(SubmitOrderCommand message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Handle {message.GetType().Name} {message.Number}");

            var id = Guid.NewGuid();
            var timeStamp = DateTime.UtcNow;

            await _repository.AddAsync(new OrderRecord { Id = id, Number = message.Number, PartitionKey = "Customer1" });

            _log.LogInformation($"Publish OrderSubmittedEvent {id}");
            await context.Publish(new OrderSubmittedEvent { Id = id, Timestamp = timeStamp });

            _log.LogInformation($"Reply SubmitOrderResponse {id}");
            await context.Reply(new SubmitOrderResponse { Id = id, Timestamp = timeStamp });
        }
    }
}
