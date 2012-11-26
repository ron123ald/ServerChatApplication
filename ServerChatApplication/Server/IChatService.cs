namespace ServerChatApplication.Server
{
    using System;
    using System.Net;
    using ServerChatApplication.Server.Chat.Action;
    using ServerChatApplication.Server.ChatEvents;

    public delegate void LogEventHandler(object sender, LogEventArgs e);
    public delegate void JoinEventHandler(object sender, NewEventArgs e);
    public delegate void SendMessageEventHandler(object sender, SendMessageEventArgs e);
    public delegate void LeaveEventHandler(object sender, LeaveEventArgs e);
    public delegate void NewUserEventHandler(object sender, NewUserEventArgs e);
    public delegate void OnStartEventHandler(object sender, StartEventArgs e);
    public delegate void OnStopEventHandler(object sender, StopEventArgs e);

    public interface IChatService : IDisposable
    {
        event OnStartEventHandler OnStart;
        event OnStopEventHandler OnStop;
        event LogEventHandler EventLog;

        void StartServer();
        void StartServer(int port);
        void StartServer(IPAddress address, int port);

    }
}
