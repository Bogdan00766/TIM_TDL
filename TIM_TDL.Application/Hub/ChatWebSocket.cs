using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TIM_TDL.Application.Kafka;
using TIM_TDL.Domain.Models;

namespace TIM_TDL.Application.WebSocket
{
    public class ChatWebSocket : Hub
    {
        private readonly ILogger _Logger;
        private readonly KafkaProducer _KafkaProducer;
        private readonly KafkaConsumer _KafkaConsumer;

        private static Dictionary<string, string> _Queue = new();

        public ChatWebSocket(ILogger logger, KafkaProducer kafkaProducer, KafkaConsumer kafkaConsumer)
        {
            _Logger = logger.ForContext<ChatWebSocket>();
            _KafkaProducer = kafkaProducer;
            _KafkaConsumer = kafkaConsumer;
        }

        public async Task SendMessage(int senderId, string message, string chatId)
        {
            if (message.IsNullOrEmpty()) return;

            var chatMessage = new ChatMessage
            {
                Content = message,
                SenderId = senderId,
                ChatId = chatId
            };

            await Clients.Group(chatId).SendAsync(JsonConvert.SerializeObject(chatMessage));
        }

        public async Task RegisterToQueue(int id, string question)
        {
            var guid = Guid.NewGuid().ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, guid);

            KafkaChatQueueMessage kafkaChatQueueMessage = new KafkaChatQueueMessage
            {
                ConnectorId = id,
                Question = question,
                WSGroupId = guid
            };

            await _KafkaProducer.AddToChatQueue(kafkaChatQueueMessage);

            _Queue.Add(Context.ConnectionId, guid);
            await Clients.Group(guid).SendAsync("Sucessfully added to queue");


        }

        public async Task GetAdminChat(int adminId)
        {
            _Logger.Debug("GetAdminChat started");
            KafkaChatQueueMessage? message;
            ChatMessage? chatMessage;
            while (true)
            { 
                _Logger.Debug("Another while loop iteration");
                message = _KafkaConsumer.GetChatClient();
                if (message == null)
                {
                    _Logger.Debug("Empty queue");
                    chatMessage = new ChatMessage
                    {
                        Content = "QUEUE IS EMPTY",
                        SenderId = adminId,
                        ChatId = null
                    };
                    await Clients.Caller.SendAsync(JsonConvert.SerializeObject(chatMessage));
                    return;
                }
                if (_Queue.ContainsValue(message.WSGroupId))
                {
                    _Logger.Debug("NIE PUSTA");
                    await Groups.AddToGroupAsync(Context.ConnectionId, message.WSGroupId);
                    break;
                }
                
            }
            chatMessage = new ChatMessage
            {
                Content = "How can i help you?",
                SenderId = adminId,
                ChatId = message == null ? "Brak nowych wiadomości" : message.WSGroupId
            };
            _Logger.Debug("WYSYŁAM");
            if (message != null)
                await Clients.Group(message!.WSGroupId).SendAsync(JsonConvert.SerializeObject(chatMessage));
            else
                await Clients.Caller.SendAsync(JsonConvert.SerializeObject(chatMessage));
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
             _Queue.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class ChatMessage
    {
        public int? SenderId { get; set; }
        public string? Content { get; set; }
        public string? ChatId { get; set; }
    }
}
