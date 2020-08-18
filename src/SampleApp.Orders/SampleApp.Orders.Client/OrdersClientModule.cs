namespace SampleApp.Orders.Client
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Transport;

    public static class OrdersClientModule
    {
        public static IServiceCollection AddOrdersClient(this IServiceCollection services, OrdersClientOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
