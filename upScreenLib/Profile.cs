using System.Collections.Generic;
using Newtonsoft.Json;

namespace upScreenLib
{
    public class Profile
    {
        public string Host;

        public string Username;

        public string Password;

        public int Port = 21;

        public List<string> RemotePaths = new List<string>();

        public string HttpPath;

        public ImageExtensions Extension = ImageExtensions.PNG;

        public int FileLenght = 5;

        public string Pattern;

        public FtpProtocol Protocol = FtpProtocol.FTP;

        public FtpsMethod FtpsInvokeMethod;

        public int DefaultFolder = 0;

        public bool IsDefaultAccount = false;

        [JsonIgnore]
        public static bool FromFileMenu { get; set; }

        [JsonIgnore]
        public static List<string> ArgFiles = new List<string>();

        [JsonIgnore]
        public string RemotePath
        {
            get { return RemotePaths[DefaultFolder]; }
            set
            {
                if (RemotePaths.Count == 0)
                    RemotePaths.Add(value);
                else
                    RemotePaths[0] = value;
            }
        }

        /// <summary>
        /// Load the given account details
        /// </summary>
        public void AddAccount(string host, string user, string pass, int port)
        {            
            Host = host;
            Username = user;
            Password = pass;
            Port = port;           
        }

        /// <summary>
        /// Load the given paths
        /// </summary>
        public void AddPaths(string remote, string http)
        {
            RemotePath = remote;
            HttpPath = http;
        }

        /// <summary>
        /// Clear all current profile info
        /// </summary>
        public void Clear()
        {
            Host = null;
            Username = null;
            Password = null;
            Port = 21;
            RemotePath = null;
            HttpPath = null;
        }
    }
}