namespace ServerChatApplication.Server.ChatEvents
{
    using System;
    public class LeaveEventArgs : EventArgs
    {
        private string _userUniqueID = string.Empty;
        private string _userName = string.Empty;

        public LeaveEventArgs(string userName, string userUniqueID)
        {
            this.UserName = userName;
            this.UserUniqueID = userUniqueID;
        }

        #region UserName
        
        public string UserName
        {
            get { return this._userName; }
            set { this._userName = value; }
        } 

        #endregion

        #region UserUniqueID
        
        public string UserUniqueID
        {
            get { return this._userUniqueID; }
            set { this._userUniqueID = value; }
        } 

        #endregion
    }
}
