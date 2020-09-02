namespace SampleApp.Orders.Client
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using SampleApp.Orders.Client.Data;
    using SampleApp.Shared.Abstractions;

    public static class OrdersClientModule
    {
        public static IServiceCollection AddOrdersClient(this IServiceCollection services, OrdersClientOptions options)
        {
            var assembly = typeof(OrdersClientModule).Assembly;

            return services
                .AddAutoMapper(assembly)
                .AddScoped<IOrdersRecordRepository>();
                .AddSingleton(options);
        }
    }
}
