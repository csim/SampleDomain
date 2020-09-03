namespace SampleApp.Web.Data
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client.Commands;

    public class OrdersService
    {
        public OrdersService(
            IMessageSession messageSession,
            ILogger<OrdersService> log)
        {
            _messageSession = messageSession;
            _log = log;
        }

        private readonly ILogger<OrdersService> _log;
        private readonly IMessageSession _messageSession;

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
