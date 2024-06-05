using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using esign.Friendships.Dto.Ver1;

namespace esign.Chat.Dto.Ver1
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }
        
        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}