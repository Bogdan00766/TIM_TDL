using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Dtos.Chat
{
    public class ChatMessageDto
    {
        public int? SenderId { get; set; }
        public string Content { get; set; }
        public string ChatId { get; set; }
    }
}
