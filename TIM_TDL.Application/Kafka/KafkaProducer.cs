using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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
        private readonly IProducer<Null, string> _Producer;
        private readonly string? topic;

        public KafkaProducer(IConfiguration config, ILogger logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _HostApplicationLifetime = hostApplicationLifetime;
            _Logger = logger.ForContext<KafkaProducer>();
            _Config = config;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _Config["Kafka:BootstrapServers"],
                EnableDeliveryReports = true,
                ClientId = Dns.GetHostName(),
                // Emit debug logs for message writer process, remove this setting in production
                Debug = "msg",
                // retry settings:
                // Receive acknowledgement from all sync replicas
                Acks = Acks.All,
                // Number of times to retry before giving up
                MessageSendMaxRetries = 3,
                // Duration to retry before next attempt
                RetryBackoffMs = 1000,
                // Set to true if you don't want to reorder messages on retry
                EnableIdempotence = true
            };
            topic = _Config["Kafka:Topic"];

            if(producerConfig.BootstrapServers.IsNullOrEmpty() || producerConfig.ClientId.IsNullOrEmpty() || topic.IsNullOrEmpty()) 
            {
                _Logger.Fatal("Kafka producer config error.");
                hostApplicationLifetime.StopApplication();
            }
            var producerConfig2 = new Dictionary<string, string> { { "bootstrap.servers", _Config["Kafka:BootstrapServers"]! } };
            _Producer = new ProducerBuilder<Null, string>(producerConfig2)
                .SetKeySerializer(Serializers.Null)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

        }

        public async Task AddToChatQueue(KafkaChatQueueMessage message)
        {  

            try
            {
                await _Producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(message) });
                Flag.CanConsume = true;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, "Error producing message");
            }
        }


    }
}
