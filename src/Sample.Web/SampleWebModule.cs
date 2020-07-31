namespace Sample.Web
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class SampleWebModule
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
        {
            var moduleType = typeof(SampleWebModule);
            var options = configuration
                    .GetSection(moduleType.Namespace)
                    .Get<SampleWebOptions>()
                ?? new SampleWebOptions();

            return services
                .AddWeb(options);
        }

        public static IServiceCollection AddWeb(this IServiceCollection services, SampleWebOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
