namespace Sample.Web
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Sample.Shared.Infrastructure.Data;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    }).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }

            host.Run();
        }
    }
}
