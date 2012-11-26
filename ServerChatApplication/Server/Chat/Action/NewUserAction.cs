namespace ServerChatApplication.Server.Chat.Action
{
    using Newtonsoft.Json;

    public class NewUserEventArgs : ChatAction
    {

        private string _username = string.Empty;
        private string _userUniqueID = string.Empty;

        public NewUserEventArgs()
        {

        }

        public NewUserEventArgs(string username, string userUniqueID)
        {
            this.Username = username;
            this.UserUniqueID = userUniqueID;
        }

        public string Username
        {
            get { return this._username; }
            set { this._username = value; }
        }

        public string UserUniqueID
        {
            get { return this._userUniqueID; }
            set { this._userUniqueID = value; }
        }

        public override string Action
        {
            get { return ChatActionTypes.newuser.ToString(); }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
