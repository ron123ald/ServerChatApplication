namespace ServerChatApplication.Server
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ServerChatApplication.Server.Chat.Action;
    using ServerChatApplication.Server.ChatEvents;
    using ServerChatApplication.Server.Client;
    using ServerChatApplication.Server.Collection;
    public static class ChatUtility
    {
        public static ChatActionTypes GetActionType(this string dataRecieved)
        {
            ChatActionTypes type = default(ChatActionTypes);
            JObject actionDetails = JsonConvert.DeserializeObject<JObject>(dataRecieved);
            if (actionDetails != null)
            {
                switch ((string)actionDetails["Action"])
                {
                    case "join":
                        type = ChatActionTypes.join;
                        break;
                    case "send":
                        type = ChatActionTypes.send;
                        break;
                    case "leave":
                        type = ChatActionTypes.leave;
                        break;
                    default: break;
                }
            }
            return type;
        }

        public static SendMessageEventArgs ProcessSendMessageDetails(this string data)
        {
            SendMessageEventArgs e = default(SendMessageEventArgs);
            /// converting the data
            /// and save it to it's designated property
            try
            {
                e = JsonConvert.DeserializeObject<SendMessageEventArgs>(data);
            }
            catch { }
            return e;
        }

        public static void NotifyOtherUsers(this NewUserEventArgs action)
        {
            /// get all in users list and send new UserEvent to it
            IDictionary<int, ChatClientHandler> userHandlers = (ChatClientCollection.InstanceContext).Get();
            foreach (var item in userHandlers)
                (item.Value).WriteLine(action.ToString());
        }
    }
}
