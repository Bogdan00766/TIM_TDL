using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TIM_TDL.Application.Kafka
{
    public class KafkaConsumer
    {
        private readonly IHostApplicationLifetime _HostApplicationLifetime;
        private readonly ILogger _Logger;
        private readonly IConfiguration _Config;
        private readonly string? topic;
        private readonly IConsumer<Ignore, KafkaChatQueueMessage> _Consumer;

        public KafkaConsumer(IConfiguration config, ILogger logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _HostApplicationLifetime = hostApplicationLifetime;
            _Logger = logger.ForContext<KafkaConsumer>();
            _Config = config;

            topic = _Config["Kafka:Topic"];

            var cc = new ConsumerConfig
            {
                BootstrapServers = _Config["Kafka:BootstrapServer"],
                GroupId = _Config["Kafka:GroupId"],
            };
            if(cc.BootstrapServers == null || cc.GroupId == null || topic == null)
            {
                _Logger.Fatal("Kafka consumer config error.");
                hostApplicationLifetime.StopApplication();
            }

            _Consumer = new ConsumerBuilder<Ignore, KafkaChatQueueMessage>(cc).Build();           
        }

        public KafkaChatQueueMessage? GetChatClient()
        {
            KafkaChatQueueMessage? result = null;
            try
            {
                var message = _Consumer.Consume();
                result = message.Message.Value;

            }
            catch (ConsumeException ex)
            {
                _Logger.Error(ex, "Error while consuming message");
            }
            finally
            {
                _Consumer.Close();
            }
            return result;
        }
    }
}
