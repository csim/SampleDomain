namespace SampleApp.Orders.Client
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Transport;

    public static class OrdersClientModule
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, OrdersClientOptions options)
        {
            return services
                .AddSingleton(options);
        }

        public static RoutingSettings<T> AddOrdersClient<T>(this RoutingSettings<T> routing) where T : TransportDefinition
        {
            var messageModuleType = typeof(OrdersClientModule);
            routing.RouteToEndpoint(messageModuleType.Assembly, messageModuleType.Namespace);

            return routing;
        }
    }
}
