namespace Sample.Web
{
    using Microsoft.Extensions.DependencyInjection;

    public static class SampleWebModule
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, SampleWebOptions options)
        {
            return services
                .AddSingleton(options);
        }
    }
}
