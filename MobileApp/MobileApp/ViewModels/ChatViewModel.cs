using MobileApp.Dtos.Chat;
using MobileApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ChatMessageDto> _Chats;// = new ObservableCollection<ChatMessageDto> { new ChatMessageDto { Content= "TEST" } };
        public ObservableCollection<ChatMessageDto> Chats
        {
            get { return _Chats; }
            set
            {
                _Chats = value;
                NotifyPropertyChanged();
                PropertyChanged(this, new PropertyChangedEventArgs("Chats"));
            }
        }
        public string Message { get; set; }

        public ChatViewModel()
        {
            _ChatService = new ChatService();
            SendMessageCommand = new Command(SendMessageAsync);
            RegisterToChatCommand = new Command(RegisterToChatAsync);
            ConnectToChatCommand = new Command(ConnectToChatAsync);
        }

        private async void RegisterToChatAsync(object obj)
        {
            await _ChatService.RegisterToQueueAsync();
        }
        private async void SendMessageAsync()
        {
            if (String.IsNullOrEmpty(Message)) return;
            await _ChatService.SendMessageAsync(Message);
        }

        private async void ConnectToChatAsync()
        {
            await _ChatService.ConnectAsync(HandleIncomingMessages);
        }

        private void HandleIncomingMessages(string obj)
        {
            var message = JsonConvert.DeserializeObject<ChatMessageDto>(obj);
            if(Chats == null) Chats = new ObservableCollection<ChatMessageDto>();
            Chats.Add(message);
            if(message.ChatId != null) 
            { 
                _ChatService.SetChatId(message.ChatId);
            }
            PropertyChanged(this, new PropertyChangedEventArgs("Chats"));      
        }

       
        private readonly ChatService _ChatService;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SendMessageCommand { get; }
        public ICommand RegisterToChatCommand { get; }
        public ICommand ConnectToChatCommand { get; }
    }
}
