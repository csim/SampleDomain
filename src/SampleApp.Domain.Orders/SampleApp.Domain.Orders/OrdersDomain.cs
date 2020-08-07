namespace SampleApp.Domain.Orders
{
    using Microsoft.Extensions.DependencyInjection;

    public static class OrdersDomain
    {
        //public static IMapper Mapper { get; set; }

        public static IServiceCollection AddOrderingDomain(this IServiceCollection services, SampleOrderingDomainOptions options)
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
