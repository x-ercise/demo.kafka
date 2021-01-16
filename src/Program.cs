using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Kafka
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);

            host.ConfigureServices((context, services) =>
            {
                services.AddHostedService<DemoConsumerService>();
                services.AddHostedService<DemoProducerService>();
            });

            return host;
        }
    }

    
}
