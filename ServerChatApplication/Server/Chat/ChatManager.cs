namespace ServerChatApplication.Server.Chat
{
    using System;
    using System.Net;
    using ServerChatApplication.Server.Collection;

    public class ChatManager : IChatService, IDisposable
    {
        private bool _disposing = false;
        private ChatConnection _connection = default(ChatConnection);
        private ChatClientCollection _collection = default(ChatClientCollection);
        private Logger _logger = default(Logger);

        public ChatManager()
        {
            /// connections
            this._connection = new ChatConnection();
            this._connection.LogEvent += connectionLogEvent;
            this._connection.JoinEvent += connectionJoinEvent;

            /// collections
            this._collection = ChatClientCollection.InstanceContext;

            /// logger
            this._logger = Logger.InstanceContext;
        }

        #region ChatConnection Members
        
        private void connectionLogEvent(object sender, ChatEvents.LogEventArgs e)
        {
            if (this.EventLog != null)
                this.EventLog(this, e);
            this._logger.WriteToDisk(e.Message);
        }

        private void connectionJoinEvent(object sender, ChatEvents.NewEventArgs e)
        {
            Client.ChatClientHandler handler = new Client.ChatClientHandler(e);
            handler.LeaveEvent += handlerLeaveEvent;
            handler.SendMessageEvent += handlerSendMessageEvent;
            handler.LogEvent += handlerLogEvent;
            handler.NewUserEvent += handlerNewUserEvent;
            this._collection.Add(handler);
        }

        #region Handler

        private void handlerNewUserEvent(object sender, Action.NewUserEventArgs e)
        {
            System.Action action = (() => e.NotifyOtherUsers());
            action.Invoke();
        }

        /// <summary>
        /// e.From = sender
        /// e.TO = recepient
        /// e.Message = message to send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handlerSendMessageEvent(object sender, ChatEvents.SendMessageEventArgs e)
        {
            if (e != null)
            {
                /// process send to recepient
                /// get sender and recipient
                Client.ChatClientHandler recepientHandler = this._collection.Get(e.To);
                if (recepientHandler != null)
                    recepientHandler.WriteLine(e.ToString());
            }
        }

        private void handlerLeaveEvent(object sender, ChatEvents.LeaveEventArgs e)
        {
            /// get handler in Collections, remove and dispose
            Client.ChatClientHandler handler = this._collection.Get(e.UserUniqueID);
            if (handler != null)
            {
                this._collection.Remove(handler);
                handler.Dispose();
            }
        }

        private void handlerLogEvent(object sender, ChatEvents.LogEventArgs e)
        {
            if (e != null)
            {
                if (this.EventLog != null)
                    this.EventLog(this, e);
                this._logger.WriteToDisk(e.Message);
            }
        }

        #endregion

        #endregion

        #region IChatService Members

        #region EventHandlers
        
        public event LogEventHandler EventLog;

        public event OnStartEventHandler OnStart;

        public event OnStopEventHandler OnStop; 

        #endregion

        #region StartServer Overloads
        
        public void StartServer()
        {
            System.Action action = (()=> this._connection.EstablishConnection());
            action.Invoke();
        }

        public void StartServer(int port)
        {
            this._connection.Port = port;

            this.StartServer();
        }

        public void StartServer(IPAddress address, int port)
        {
            this._connection.Address = address;
            this._connection.Port = port;

            this.StartServer();
        } 

        #endregion

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposing)
            {
                if (disposing)
                {
                    this._collection.Dispose();
                    this._connection.DestroyConnection();
                    this._connection.Dispose();
                }
                this._disposing = true;
            }
        }
    }
}
