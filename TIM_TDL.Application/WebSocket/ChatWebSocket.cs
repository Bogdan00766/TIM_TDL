using Microsoft.AspNetCore.SignalR;
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
            await Clients.All.SendAsync(str, Context.ConnectionId); //moze byc caller

        }
        //join_chat(token) ->tworzy grupe ->group id będzie lądować na kafce. Admin start chat as Admin. z kafki będzie brać id grupy
    }
}
