namespace ServerChatApplication.Server
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;
    public class Logger
    {
        private static Logger _instance = default(Logger);
        private bool _loggerEnable = false;
        private string _loggerFilePath = string.Empty;
        private string _fileName = string.Empty;

        private Logger()
        {
            this.LoggerEnable = bool.Parse(ConfigurationManager.AppSettings.Get("LOGENABLE"));
            this.LoggerFilePath = ConfigurationManager.AppSettings.Get("LOGFILEPATH");

            /// create directory if not exists
            if (this.LoggerEnable && !Directory.Exists(this.LoggerFilePath))
                Directory.CreateDirectory(this.LoggerFilePath);
        }

        public static Logger InstanceContext
        {
            get { return _instance ?? (_instance = (new Logger())); }
        }

        public string LoggerFilePath
        {
            get { return this._loggerFilePath; }
            private set { this._loggerFilePath = value; }
        }

        public bool LoggerEnable
        {
            get { return this._loggerEnable; }
            private set { this._loggerEnable = value; }
        }

        public string FileName
        {
            get 
            {
                this._fileName = string.Format("{0}.txt",DateTime.Now.ToString("MM-dd-yy"));
                if (!File.Exists(this.LoggerFilePath + this._fileName))
                    File.Create(this.LoggerFilePath + this._fileName);
                return this._fileName;
            }
        }

        public void WriteToDisk(string message)
        {
            if (this.LoggerEnable)
            {
                try
                {
                    using (StreamWriter writer = File.AppendText(this.LoggerFilePath + this.FileName))
                    {
                        writer.WriteLine(string.Format("[{0}] {1}", DateTime.Now, message));
                        writer.Flush();
                    }
                }
                catch { }
            }
        }
    }
}
