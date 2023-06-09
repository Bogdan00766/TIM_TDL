using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TIM_TDL.Application.Kafka
{
    public class KafkaProducer
    {
        private readonly IHostApplicationLifetime _HostApplicationLifetime;
        private readonly ILogger _Logger;
        private readonly IConfiguration _Config;
        private readonly IProducer<Null, KafkaChatQueueMessage> _Producer;
        private readonly string? topic;

        public KafkaProducer(IConfiguration config, ILogger logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _HostApplicationLifetime = hostApplicationLifetime;
            _Logger = logger.ForContext<KafkaProducer>();
            _Config = config;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _Config["Kafka:BootstrapServer"],
            };
            topic = _Config["Kafka:Topic"];

            if(producerConfig.BootstrapServers == null || producerConfig.ClientId == null || topic == null) 
            {
                _Logger.Fatal("Kafka producer config error.");
                hostApplicationLifetime.StopApplication();
            }

            _Producer = new ProducerBuilder<Null, KafkaChatQueueMessage>(producerConfig).Build();

        }

        public async Task AddToChatQueue(int connectorId, string question)
        {
            var message = new KafkaChatQueueMessage
            {
                ConnectorId = connectorId,
                Question = question
            };
            try
            {
                await _Producer.ProduceAsync(topic, new Message<Null, KafkaChatQueueMessage> { Value = message });
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, "Error producing message");
            }
        }


    }
}
