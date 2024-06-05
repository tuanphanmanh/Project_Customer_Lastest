using System.Collections.Generic;

namespace esign.Chat.Dto.Ver1
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}