namespace SampleApp.Orders.Endpoint.Handlers
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using SampleApp.Orders.Client.Commands;

    public class SubmitOrderHandler : IHandleMessages<SubmitOrderCommand>
    {
        public SubmitOrderHandler(ILogger<SubmitOrderHandler> log)
        {
            _log = log;
        }

        private readonly IEndpointInstance _endpoint;
        private readonly ILogger<SubmitOrderHandler> _log;

        public Task Handle(SubmitOrderCommand message, IMessageHandlerContext context)
        {
            _log.LogInformation(message.Number.ToString());

            return Task.CompletedTask;
        }
    }
}
