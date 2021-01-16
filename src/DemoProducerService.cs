using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Kafka
{
    public class DemoProducerService : IHostedService
    {
        public readonly ILogger<DemoProducerService> _logger;
        private IProducer<Null, string> _producer;

        public DemoProducerService(ILogger<DemoProducerService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 100; i++)
            {
                var value = $"Hello world {i}";
                _logger.LogInformation(value);
                await _producer.ProduceAsync(topic: "demo", message: new Message<Null, string>()
                {
                    Value = value
                }, cancellationToken);

                _producer.Flush(timeout: TimeSpan.FromSeconds(10));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
