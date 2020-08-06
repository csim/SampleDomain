namespace Sample.Domain
{
    using Microsoft.Extensions.DependencyInjection;

    public static class SampleDomainModule
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, SampleDomainOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
