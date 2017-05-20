using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace upScreenLib
{
    public class Profile
    {
        public string Host;

        public string Username;

        public string Password;

        public int Port = 21;

        public List<RemoteFolder> RemoteFolders = new List<RemoteFolder>();

        [JsonConverter(typeof(StringEnumConverter))]
        public ImageExtensions Extension = ImageExtensions.PNG;

        public int FileLenght = 5;

        public string Pattern;

        [JsonConverter(typeof(StringEnumConverter))]
        public FtpProtocol Protocol = FtpProtocol.FTP;

        [JsonConverter(typeof(StringEnumConverter))]
        public FtpsMethod FtpsInvokeMethod;

        public int DefaultFolder = 0;

        public bool IsDefaultAccount = false;

        [JsonIgnore]
        public static bool FromFileMenu { get; set; }

        [JsonIgnore]
        public string RemoteFolder => RemoteFolders[DefaultFolder].Folder;

        [JsonIgnore]
        public string RemoteHttpPath => RemoteFolders[DefaultFolder].HttpPath;

        [JsonIgnore]
        public bool IsNotSet =>
            (new[] { Host, Username, Password }).Any(string.IsNullOrEmpty);

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
        /// Convert from the old RemotePaths that only contained folders.
        /// This is only for compatibility reasons with older config files.
        /// </summary>
        [OnDeserialized]
        internal void OnDeserializedConvertRemotePaths(StreamingContext context)
        {
            if (RemotePaths.Count > 0 && RemoteFolders.Count <= 0)
                RemoteFolders.Add(new RemoteFolder(RemotePaths[0], HttpPath));
        }

        [Obsolete("Not used anymore")]
        public List<string> RemotePaths = new List<string>();
        
        [Obsolete("Not used anymore")]
        public string HttpPath;
    }

    public class RemoteFolder
    {
        public RemoteFolder(string folder, string http)
        {
            Folder = folder;
            HttpPath = http;
        }

        // The folder path (relative to user's root)
        public string Folder;
        // The http path to the folder
        public string HttpPath;
    }
}