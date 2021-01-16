using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Kafka
{
    public class DemoConsumerService : IHostedService
    {
        private readonly ILogger<DemoProducerService> _logger;
        private ClusterClient _cluster;

        public DemoConsumerService(ILogger<DemoProducerService> logger)
        {
            _logger = logger;
            _cluster = new ClusterClient(new Configuration
            {
                Seeds = "localhost:9092"
            }, new ConsoleLogger());
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            /*
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            using (var c = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = c.Consume(cts.Token);
                }
            }
            */
            _cluster.ConsumeFromLatest(topic: "demo");
            _cluster.MessageReceived += _cluster_MessageReceived;

            return Task.CompletedTask;
        }

        private void _cluster_MessageReceived(RawKafkaRecord record)
        {
            _logger.LogInformation(message: $"Received: {Encoding.UTF8.GetString(record.Value as byte[])}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
