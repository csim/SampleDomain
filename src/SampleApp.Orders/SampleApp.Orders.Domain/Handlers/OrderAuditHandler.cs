namespace SampleApp.Orders.Domain.Handlers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client.Commands;
    using SampleApp.Orders.Client.Data;
    using SampleApp.Orders.Client.Events;
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions;

    public class OrderAuditHandler : IHandleMessages<OrderRecordAuditCommand>
    {
        public OrderAuditHandler(IOrdersRecordRepository repository, IMapper mapper, ILogger<OrderAuditHandler> log)
        {
            _repository = repository;
            _mapper = mapper;
            _log = log;
        }

        private readonly ILogger<OrderAuditHandler> _log;
        private readonly IMapper _mapper;
        private readonly IOrdersRecordRepository _repository;

        public async Task Handle(OrderRecordAuditCommand message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Handle {message.GetType().Name} {message.Record.Id}");

            var shadow = _mapper.Map<OrderShadow>(message.Record);

            await _repository.AddAsync(
                new OrderAuditRecord { Record = shadow, TransactionType = message.TransactionType, PartitionKey = "Customer1" });
        }
    }
}
