namespace SampleApp.Orders.Domain
{
    using Microsoft.Extensions.DependencyInjection;

    public static class OrdersDomainModule
    {
        //public static IMapper Mapper { get; set; }

        public static IServiceCollection AddOrdersDomain(this IServiceCollection services, OrdersDomainOptions options)
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
