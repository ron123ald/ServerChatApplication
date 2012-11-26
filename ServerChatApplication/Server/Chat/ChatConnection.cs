namespace ServerChatApplication.Server.Chat
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using ServerChatApplication.Server.ChatEvents;
    public class ChatConnection : IDisposable
    {
        private IPAddress _address = IPAddress.Any;
        private int _port = 8000;
        private TcpListener _listener = default(TcpListener);
        private BackgroundWorker _backgroundWorker = default(BackgroundWorker);

        public event LogEventHandler LogEvent;
        public event JoinEventHandler JoinEvent;

        #region Constructor

        public ChatConnection()
        {
            this._listener = new TcpListener(IPAddress.Any, this._port);
            
            this._backgroundWorker = new BackgroundWorker();
            this._backgroundWorker.DoWork += backgroundDoWork;
            this._backgroundWorker.RunWorkerCompleted += backgroundRunWorkerCompleted;
        }
        
        public ChatConnection(int port)
            : this()
        {
            this._port = port;
        }

        public ChatConnection(IPAddress address, int port)
        {
            this._listener = new TcpListener(address, port);
        } 

        #endregion

        #region Background Worker

        private void backgroundRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void backgroundDoWork(object sender, DoWorkEventArgs e)
        {
            this._listener.Start();
            while (true)
            {
                NewEventArgs _newClient = new NewEventArgs();
                _newClient.Client = this._listener.AcceptTcpClient();
                /// fire join event
                this.JoinEvent(this, _newClient);
            }
        }

        #endregion

        #region Port
        public int Port
        {
            set { this._port = value; }
            get { return this._port; }
        } 
        #endregion

        #region Address
        public IPAddress Address
        {
            set { this._address = value; }
            get { return this._address; }
        } 
        #endregion

        public void EstablishConnection()
        {
            this.LogEvent(this, new LogEventArgs("Server is Running and waiting for clients"));

            this._backgroundWorker.RunWorkerAsync();
        }

        public void DestroyConnection()
        {
            this._listener.Stop();
        }

        public void Dispose()
        {
            this._backgroundWorker.Dispose();
            this.DestroyConnection();
        }
    }
}
