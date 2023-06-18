using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;

namespace TIM_TDL.Application.Kafka
{
    public class KafkaConsumer
    {
        private readonly IHostApplicationLifetime _HostApplicationLifetime;
        private readonly ILogger _Logger;
        private readonly IConfiguration _Config;
        private readonly string? topic;
        private readonly IConsumer<Null, string> _Consumer;

        public KafkaConsumer(IConfiguration config, ILogger logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _HostApplicationLifetime = hostApplicationLifetime;
            _Logger = logger.ForContext<KafkaConsumer>();
            _Config = config;

            topic = _Config["Kafka:Topic"];

            var cc = new ConsumerConfig
            {
                ClientId = Guid.NewGuid().ToString(),
                BootstrapServers = _Config["Kafka:BootstrapServers"],
                GroupId = _Config["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                SocketTimeoutMs = 30000

            };
            if(cc.BootstrapServers.IsNullOrEmpty() || cc.GroupId.IsNullOrEmpty()|| topic.IsNullOrEmpty())
            {
                _Logger.Fatal("Kafka consumer config error.");
                hostApplicationLifetime.StopApplication();
            }

            _Consumer = new ConsumerBuilder<Null, string>(cc)
                .SetKeyDeserializer(Deserializers.Null)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();
            _Consumer.Subscribe(topic);
        }

        public KafkaChatQueueMessage? GetChatClient()
        {
            if (!Flag.CanConsume) return null;
            KafkaChatQueueMessage? result = null;
            try
            {
                var message = _Consumer.Consume();
                if(message.IsPartitionEOF)
                {
                    Flag.CanConsume = false;
                    return null;
                }
                result = JsonConvert.DeserializeObject<KafkaChatQueueMessage>(message.Message.Value);

            }
            catch(TimeoutException)
            {
                Flag.CanConsume = false;
                return null;
            }
            catch (ConsumeException ex)
            {
                _Logger.Error(ex, "Error while consuming message");
            }
            finally
            {
                //_Consumer.Close();
            }
            return result;
        }
    }
}
