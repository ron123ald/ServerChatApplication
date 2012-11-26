namespace ServerChatApplication.Server.ChatEvents
{
    using System;
    using System.Net.Sockets;
    public class NewEventArgs : EventArgs
    {
        private TcpClient _client = default(TcpClient);

        public NewEventArgs()
        {

        }

        public NewEventArgs(TcpClient client)
        {
            this.Client = client;
        }

        #region Client
        public TcpClient Client
        {
            get { return this._client; }
            set { this._client = value; }
        } 
        #endregion
    }
}
