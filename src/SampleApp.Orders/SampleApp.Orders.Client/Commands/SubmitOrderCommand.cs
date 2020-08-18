namespace SampleApp.Orders.Client.Commands
{
    using NServiceBus;

    public class SubmitOrderCommand : ICommand
    {
        public long Number { get; set; }
    }
}
