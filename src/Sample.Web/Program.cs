namespace Sample.Web
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Sample.Infrastructure.Data;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Environment.SetEnvironmentVariable("ATLAS_ENVIRONMENT", args[0]);
            }

            var hostBuilder = Host
                    .CreateDefaultBuilder(args)
                    .UseSerilog(
                        (hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration
                                .ReadFrom
                                .Configuration(hostingContext.Configuration);

                        })
                    .ConfigureAppConfiguration(
                        (hostingContext, builder) =>
                        {
                            var env = Environment.GetEnvironmentVariable("ATLAS_ENVIRONMENT");

                            hostingContext.HostingEnvironment.EnvironmentName = env;

                            builder
                                .AddEnvironmentVariables("ATLAS_")
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"config/appsettings.{env}.secrets.json", optional: true, reloadOnChange: true);
                        })
                    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                ;

            hostBuilder.Build().Run();

            
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                .Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CosmosDbContext>();
                context.Database.EnsureCreated();
            }

            host.Run();
        }
    }
}
