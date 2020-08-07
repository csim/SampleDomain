namespace Sample.Ordering.Domain
{
    using Microsoft.Extensions.DependencyInjection;
    using Sample.Ordering.Domain.Records;

    public static class SampleOrderingDomainModule
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
