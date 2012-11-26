namespace ServerChatApplication
{
    using System;
    using ServerChatApplication.Server;
    using ServerChatApplication.Server.Chat;
    class Program
    {
        private static IChatService chatService = default(IChatService);
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Chat Server Manager";
                chatService = new ChatManager();
                chatService.OnStart += chatService_OnStart;
                chatService.OnStop += chatService_OnStop;
                chatService.EventLog += chatService_EventLog;
                chatService.StartServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }

        static void chatService_EventLog(object sender, Server.ChatEvents.LogEventArgs e)
        {
            Console.ForegroundColor = e.Color;
            Console.WriteLine(e.Message);
            Console.ResetColor();
        }

        static void chatService_OnStop(object sender, Server.ChatEvents.StopEventArgs e)
        {
            chatService.Dispose();
        }

        static void chatService_OnStart(object sender, Server.ChatEvents.StartEventArgs e)
        {
        }
    }
}
