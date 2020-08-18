﻿namespace SampleApp.Orders.Client
{
    using Microsoft.Extensions.DependencyInjection;

    public static class OrdersClientModule
    {
        public static IServiceCollection AddOrdersClient(this IServiceCollection services, OrdersClientOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
