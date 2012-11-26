namespace ServerChatApplication.Server.Collection
{
    using System;
    using System.Collections.Generic;
    using ServerChatApplication.Server.Client;
    public class ChatClientCollection : IDisposable
    {
        private IDictionary<int, ChatClientHandler> _collection = default(IDictionary<int, ChatClientHandler>);
        private static ChatClientCollection _instance = default(ChatClientCollection);

        private ChatClientCollection() 
        {
            this._collection = new Dictionary<int, ChatClientHandler>();
        }

        public static ChatClientCollection InstanceContext
        {
            get { return _instance ?? ( _instance = (new ChatClientCollection()) ); }
        }

        public void Add(ChatClientHandler handler)
        {
            if (!ContainsKey(handler.GetHashCode()))
                this._collection.Add(handler.GetHashCode(), handler);
        }

        public void Remove(ChatClientHandler handler)
        {
            if (ContainsKey(handler.GetHashCode()))
                this._collection.Remove(handler.GetHashCode());
        }

        public ChatClientHandler Get(int hashCode)
        {
            return default(ChatClientHandler);
        }

        public ChatClientHandler Get(string userUniqueID)
        {
            ChatClientHandler handler = default(ChatClientHandler);
            foreach (var item in this._collection)
            {
                if ((item.Value).UserUniqueID.Equals(userUniqueID, StringComparison.InvariantCultureIgnoreCase))
                {
                    handler = item.Value;
                    break;
                }
            }
            return handler;
        }

        public IDictionary<int, ChatClientHandler> Get()
        {
            return this._collection;
        }

        private bool ContainsKey(int hashCode)
        {
            return this._collection.ContainsKey(hashCode);
        }

        public void Dispose()
        {
            foreach (var item in this._collection)
                (item.Value).Dispose();
        }

        #region Handler Members
        
        

        #endregion
    }
}