namespace SampleApp.Orders.Api
{
    using Microsoft.Extensions.DependencyInjection;

    public static class OrdersEndpointModule
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, OrdersEndpointOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
