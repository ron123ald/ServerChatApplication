namespace ServerChatApplication.Server.Client
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Sockets;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ServerChatApplication.Server.Chat;
    using ServerChatApplication.Server.ChatEvents;

    public class ChatClientHandler : ChatUser, IDisposable
    {
        private byte[] _data = new byte[1024];
        private NewEventArgs _eventArgs = default(NewEventArgs);
        private StreamReader _reader = default(StreamReader);
        private StreamWriter _writer = default(StreamWriter);
        private NetworkStream _networkStream = default(NetworkStream);
        private BackgroundWorker _backgroundWorker = default(BackgroundWorker);

        public event LogEventHandler LogEvent;
        public event SendMessageEventHandler SendMessageEvent;
        public event LeaveEventHandler LeaveEvent;

        public ChatClientHandler(NewEventArgs args)
        {
            this._eventArgs = args;

            this._backgroundWorker = new BackgroundWorker();
            this._backgroundWorker.DoWork += DoWork;
            this._backgroundWorker.RunWorkerCompleted += backgroundRunWorkerCompleted;
            this._backgroundWorker.RunWorkerAsync();
        }

        private void backgroundRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this._networkStream = this._eventArgs.Client.GetStream();
                this._reader = new StreamReader(this._networkStream);
                this._writer = new StreamWriter(this._networkStream);
                
                /// this will hold the data that recieved from the stream.
                string dataRecieved = string.Empty;
                WriteLine("***** WELCOME *****");

                while (true)
                {
                    dataRecieved = this._reader.ReadLine();
                    if (!string.IsNullOrEmpty(dataRecieved))
                    {
                        /// Get Action type from dataRecieved
                        switch (dataRecieved.GetActionType())
                        {
                            case ChatActionTypes.join:

                                /// first data receieved will contains the user details
                                /// username, guid, createdate
                                processUserDetails(dataRecieved);
                                /// log event to console
                                this.LogEvent(this, new LogEventArgs(string.Format("Client \"{0}\" connected in server [ {1} ]", this.Username, this.UserUniqueID)));
                                /// notify other users

                                break;
                            case ChatActionTypes.leave:

                                this.LeaveEvent(this, new LeaveEventArgs(this.Username, this.UserUniqueID));
                                /// log event to console
                                this.LogEvent(this, new LogEventArgs(string.Format("Client \"{0}\" disconnected from server [ {1} ]", this.Username, this.UserUniqueID)));
                                /// notify other users
                                
                                break;
                            case ChatActionTypes.send:

                                SendMessageEventArgs data = dataRecieved.ProcessSendMessageDetails();
                                /// Process data request 
                                this.SendMessageEvent(this, data);
                                /// log event to console
                                this.LogEvent(this, new LogEventArgs(string.Format("Client \"{0}\" send message to {1} with message [\"{2}\"]", this.Username, data.To, data.Message)));

                                break;
                            default: break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LeaveEvent(this, new LeaveEventArgs(this.Username, this.UserUniqueID));
                this.LogEvent(this, new LogEventArgs(ex.Message, ConsoleColor.Red));
            }
        }

        public void WriteLine(string data)
        {
            this._writer.WriteLine(data);
            this._writer.Flush();
        }

        public string ReadLine()
        {
            return this._reader.ReadLine();
        }

        private void processUserDetails(string data)
        {
            /// converting the data
            /// and save it to it's designated property
            JObject userDetails = JsonConvert.DeserializeObject<JObject>(data);
            if (userDetails != null)
            {
                this.Username = (String)userDetails["Username"];
                this.UserUniqueID = (String)userDetails["UserUniqueID"];
                this.CreateDate = DateTime.Parse((string)userDetails["CreateDate"]);
            }
        }

        public void Dispose()
        {
            /// tcpClient close
            this._eventArgs.Client.Close();
            /// background worker dispose
            this._backgroundWorker.Dispose();
            /// networkStream close and dispose
            this._networkStream.Close();
            this._networkStream.Dispose();
            /// stream writer and reader, close and dispose
            this._writer.Close();
            this._reader.Close();
            this._writer.Dispose();
            this._reader.Dispose();
        }
    }
}
