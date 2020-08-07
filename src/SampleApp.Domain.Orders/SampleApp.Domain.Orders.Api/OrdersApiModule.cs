namespace SampleApp.Domain.Orders.Api
{
    using Microsoft.Extensions.DependencyInjection;

    public static class OrdersApiModule
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, OrdersApiOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
