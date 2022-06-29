
using Autofac.Extensions.DependencyInjection;
namespace Quotes.Import.Service
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(c => c.AddConsole()) 
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>().CaptureStartupErrors(true); });
    }
}