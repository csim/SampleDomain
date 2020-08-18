namespace SampleApp.Web.Data
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client;
    using SampleApp.Orders.Client.Commands;

    public class OrdersService
    {
        public OrdersService(ILogger<OrdersService> log, IMessageSession messageSession)
        {
            _log = log;
            _messageSession = messageSession;
        }

        private readonly ILogger<OrdersService> _log;
        private readonly IMessageSession _messageSession;

        public async Task SubmitOrderAsync()
        {
            var command = new SubmitOrderCommand { Number = DateTime.Now.Ticks };

            _log.LogInformation(command.Number.ToString());

            await _messageSession.Send(typeof(OrdersClientModule).Namespace, command);
        }
    }
}
