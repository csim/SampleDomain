namespace Sample.Domain
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sample.Shared.Abstractions;

    public static class SampleDomainModule
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            var moduleType = typeof(SampleDomainModule);
            var options = configuration
                    .GetSection(moduleType.Namespace)
                    .Get<SampleDomainOptions>()
                ?? new SampleDomainOptions();

            return services
                .AddDomain(options);
        }

        public static IServiceCollection AddDomain(this IServiceCollection services, SampleDomainOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
