namespace SampleApp.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client.Commands;
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions;

    public class OrdersService
    {
        public OrdersService(ILogger<OrdersService> log, IRecordRepository repository, IMessageSession messageSession)
        {
            _log = log;
            _repository = repository;
            _messageSession = messageSession;
        }

        private readonly ILogger<OrdersService> _log;
        private readonly IRecordRepository _repository;
        private readonly IMessageSession _messageSession;

        public async Task<IEnumerable<OrderRecord>> Orders(int skip = 0, int take = 10000)
        {
            return await _repository
                .AsQueryable<OrderRecord>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<SubmitOrderResponse> SubmitOrderAsync()
        {
            var command = new SubmitOrderCommand { Number = DateTime.Now.Ticks };

            _log.LogInformation($"SubmitOrderCommand {command.Number}");

            var response = await _messageSession.Request<SubmitOrderResponse>(command);

            _log.LogInformation($"SubmitOrderResponse {response.Id}");

            return response;
        }
    }
}
