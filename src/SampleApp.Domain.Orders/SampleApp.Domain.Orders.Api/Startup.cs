namespace SampleApp.Orders.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SampleApp.Orders.Domain;
    using SampleApp.Shared.Infrastructure;
    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var infraOptions = GetOptions<InfrastructureOptions>();
            var webOptions = GetOptions<OrdersEndpointOptions>();
            var domainOptions = GetOptions<SampleOrderingDomainOptions>();

            services
                .AddControllers();
            services
                .AddSharedInfrastructure(infraOptions)
                .AddOrdersDomain(domainOptions)
                .AddWeb(webOptions)
                .AddLogging(options => options.AddSerilog(Log.Logger, dispose: true));
        }

        private TOptions GetOptions<TOptions>() where TOptions : class, new()
        {
            return _configuration
                    .GetSection(typeof(TOptions).Namespace)
                    .Get<TOptions>()
                ?? new TOptions();
        }
    }
}
