using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIM_TDL.Application.WebSocket
{
    public class ChatWebSocket : Hub
    {
            //dodawanie uz do listy ocz. - odróżnić połączonych użytkowników
            //ondisconnected
            //mogę wywołać jako klient websocketowe (handle onConnected)
         
        //wysłać handshake do websocketu bo hub nie ogarnia
        //handshake klienta (postmana) z websocketem (aplikacja)

        //tworzenie grup i łączenie do nich




        public async Task HandleConnectedAsync(string str)
        {
            ChatMessage chatMessage = new ChatMessage {
                SenderId = 1,
                Content = str

            }; //do dto klase


            await Clients.All.SendAsync(JsonConvert.SerializeObject(chatMessage), Context.ConnectionId); //moze byc caller - samodzielne tworzenie grup - rejestracja do czatu + metoda admina get chat 
            //gry rejestracja to id grupy trafia na kafke

        }

        //join_chat(token) ->tworzy grupe ->group id będzie lądować na kafce. Admin start chat as Admin. z kafki będzie brać id grupy
    }

    public class ChatMessage
    {
        public int SenderId { get; set; }
        public string Content { get; set; }
    }
}
