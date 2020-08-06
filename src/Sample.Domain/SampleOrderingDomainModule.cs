namespace Sample.Ordering.Domain
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Sample.Ordering.Domain.Records;

    public static class SampleOrderingDomainModule
    {
        public static IMapper Mapper { get; set; }

        public static IServiceCollection AddOrderingDomain(this IServiceCollection services, SampleOrderingDomainOptions options)
        {
            var automapperConfiguration = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ProductRecord, ProductCompact>();
                });

            Mapper = automapperConfiguration.CreateMapper();

            return services
                .AddSingleton(options);
        }
    }
}
