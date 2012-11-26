namespace ServerChatApplication.Server
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ServerChatApplication.Server.ChatEvents;
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
    }
}
