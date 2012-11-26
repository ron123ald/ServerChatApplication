namespace ServerChatApplication.Server.Chat
{
    using System;
    public abstract class AbstractUser
    {
        public abstract string Username { get; set; }
        public abstract DateTime CreateDate { get; set; }
        public abstract string UserUniqueID { get; set; }
    }

    public abstract class ChatUser : AbstractUser
    {
        private string _username = string.Empty;
        private DateTime _createDate = default(DateTime);
        private string _userUniqueID = string.Empty;

        public override string Username
        {
            get { return this._username; }
            set { this._username = value; }
        }

        public override DateTime CreateDate
        {
            get { return this._createDate; }
            set { this._createDate = value; }
        }

        public override string UserUniqueID
        {
            get { return this._userUniqueID; }
            set { this._userUniqueID = value; }
        }
    }
}
