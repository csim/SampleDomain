namespace SampleApp.Orders.Domain.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client.Events;
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions;
    using AutoMapper;

    public class OrderAuditHandler
        : IHandleMessages<OrderRecordAddedEvent>, IHandleMessages<OrderRecordUpdatedEvent>, IHandleMessages<OrderRecordDeletedEvent>
    {
        public OrderAuditHandler(IRecordRepository repository, IMapper mapper, ILogger<OrderAuditHandler> log)
        {
            _repository = repository;
            _mapper = mapper;
            _log = log;
        }

        private readonly ILogger<OrderAuditHandler> _log;

        private readonly IRecordRepository _repository;
        private readonly IMapper _mapper;

        public async Task Handle(OrderRecordAddedEvent message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Handle {message.GetType().Name}");

            message.Record.Id = Guid.NewGuid();

            var shadow = _mapper.Map<OrderShadow>(message.Record);

            await _repository.AddAsync(new OrderAuditRecord { Record = shadow, TransactionType = RecordTransactionType.Add, PartitionKey = "Customer1" });
        }

        public async Task Handle(OrderRecordUpdatedEvent message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Handle {message.GetType().Name}");

            var shadow = _mapper.Map<OrderShadow>(message.Record);

            await _repository.AddAsync(new OrderAuditRecord { Record = shadow, TransactionType = RecordTransactionType.Update, PartitionKey = "Customer1" });
        }

        public async Task Handle(OrderRecordDeletedEvent message, IMessageHandlerContext context)
        {
            _log.LogInformation($"Handle {message.GetType().Name}");

            var shadow = _mapper.Map<OrderShadow>(message.Record);

            await _repository.AddAsync(new OrderAuditRecord { Record = shadow, TransactionType = RecordTransactionType.Delete, PartitionKey = "Customer1" });
        }
    }
}
