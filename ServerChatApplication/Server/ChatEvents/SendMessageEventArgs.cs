namespace ServerChatApplication.Server.ChatEvents
{
    using System;
    using Newtonsoft.Json;

    public class SendMessageEventArgs : EventArgs
    {
        private string _action = "NewMessage";
        private string _from = string.Empty;
        private string _to = string.Empty;
        private string _message = string.Empty;

        public SendMessageEventArgs()
        {

        }

        public SendMessageEventArgs(string from, string to)
        {
            this.From = from;
            this.To = to;
        }

        public SendMessageEventArgs(string from, string to, string message)
        {
            this.From = from;
            this.To = to;
            this.Message = message;
        }

        #region Action

        public string Action
        {
            get { return this._action; }
        }

        #endregion

        #region From
        public string From
        {
            get { return this._from; }
            set { this._from = value; }
        } 
        #endregion

        #region To
        public string To
        {
            get { return this._to; }
            set { this._to = value; }
        } 
        #endregion

        #region Message
        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
        #endregion

        #region ToString()
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
