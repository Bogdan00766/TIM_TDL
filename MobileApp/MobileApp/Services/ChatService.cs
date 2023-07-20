using Microsoft.AspNetCore.SignalR.Client;
using MobileApp.Dtos.Chat;
using MobileApp.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileApp.Services
{
    class ChatService
    {
        private HubConnection _Connection;
        private string _ChatId;

        public ChatService()
        {
            _Connection = new HubConnectionBuilder()
                .WithUrl("http://192.168.77.201:5004/api/chat", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(CurrentUser.AccessToken);
                    options.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // bypass SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                                    (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        return message;
                    };
                    options.Headers.Add("Authorization", CurrentUser.AccessToken);
                })
                .WithAutomaticReconnect()
                .Build();


        }
        public void SetChatId(string chatId)
        {
            _ChatId = chatId;
        }
        public async Task<bool> ConnectAsync(Action<string> handler)
        {
            try
            {
                await _Connection.StartAsync();
                _Connection.On<string>("Tomek", handler);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SendMessageAsync(string content)
        {

            try
            {
                await _Connection.InvokeAsync("SendMessage", CurrentUser.GetUserId() ,content, _ChatId);
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        public async Task RegisterToQueueAsync()
        {
            try
            {
                await _Connection.InvokeAsync("RegisterToQueue", CurrentUser.GetUserId(), " ");
            }
            catch (Exception ex)
            {

            }
            }
    }
}
