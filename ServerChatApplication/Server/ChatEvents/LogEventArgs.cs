namespace ServerChatApplication.Server.ChatEvents
{
    using System;
    public class LogEventArgs : EventArgs
    {
        private string _message = string.Empty;
        private ConsoleColor _color = ConsoleColor.Green;

        public LogEventArgs(string message)
        {
            this.Message = message;
        }

        public LogEventArgs(string message, ConsoleColor color)
        {
            this.Color = color;
            this.Message = message;
        }

        public string Message
        {
            get { return this._message; }
            private set { this._message = value; }
        }

        public ConsoleColor Color
        {
            get { return this._color; }
            private set { this._color = value; }
        }
    }
}
