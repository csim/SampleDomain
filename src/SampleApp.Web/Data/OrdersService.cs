namespace SampleApp.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client;
    using SampleApp.Orders.Client.Commands;
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions;

    public class OrdersService
    {
        public OrdersService(ILogger<OrdersService> log, IServiceProvider serviceProvider)
        {
            _log = log;
            _serviceProvider = serviceProvider;
        }

        private readonly ILogger<OrdersService> _log;
        private readonly IServiceProvider _serviceProvider;

        public async Task<IEnumerable<OrderRecord>> Orders(int skip = 0, int take = 10000)
        {
            var repo = _serviceProvider.GetRequiredService<IRecordRepository>();

            return await repo.AsQueryable<OrderRecord>().ToListAsync();
        }

        public async Task<SubmitOrderResponse> SubmitOrderAsync()
        {
            var command = new SubmitOrderCommand { Number = DateTime.Now.Ticks };

            _log.LogInformation($"SubmitOrderCommand SubmitOrderResponse {command.Number}");

            var messageSession = _serviceProvider.GetRequiredService<IMessageSession>();

            return await messageSession.Request<SubmitOrderResponse>(command);
        }
    }
}
