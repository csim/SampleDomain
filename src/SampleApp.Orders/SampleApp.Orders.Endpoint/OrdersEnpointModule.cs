namespace SampleApp.Orders.Endpoint
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Transport;

    public static class OrdersEnpointModule
    {
        //public static IMapper Mapper { get; set; }

        public static IServiceCollection AddOrdersDomain(this IServiceCollection services, OrdersEndpointOptions options)
        {
            //var automapperConfiguration = new MapperConfiguration(
            //    cfg =>
            //    {
            //        cfg.CreateMap<ProductRecord, CompactProductRecord>();
            //    });

            //Mapper = automapperConfiguration.CreateMapper();

            return services
                .AddSingleton(options);
        }
    }
}
