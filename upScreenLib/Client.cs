using System;
using System.IO;
using System.Linq;
using FluentFTP;
using Renci.SshNet;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Renci.SshNet.Async;

namespace upScreenLib
{
    public static class Client
    {
        private static SftpClient sftpc;
        private static FtpClient ftpc;

        public static Task<bool> taskCheckAccount = TryConnect();

        #region Functions

        /// <summary>
        /// Connect to the remote server, using the 
        /// account info found in Common.Profile
        /// </summary>
        public static async Task Connect()
        {
            if (FTP)
            {
                ftpc = new FtpClient(Common.Profile.Host, Common.Profile.Username, Common.Profile.Password);
                ftpc.Port = Common.Profile.Port;

                switch (Common.Profile.FtpsInvokeMethod)
                {
                    case FtpsMethod.Explicit:
                    case FtpsMethod.Implicit:
                        ftpc.EncryptionMode = (FtpEncryptionMode) Common.Profile.FtpsInvokeMethod;
                        ftpc.SslProtocols = SslProtocols.Default;
                        //todo: what about e.PolicyErrors 
                        ftpc.ValidateCertificate += (o, e) => e.Accept = true;
                        break;
                    default:
                        ftpc.SslProtocols = SslProtocols.None;
                        break;
                }
                await ftpc.ConnectAsync();
            }
            else
            {
                sftpc = new SftpClient(Common.Profile.Host, Common.Profile.Port, 
                        Common.Profile.Username, Common.Profile.Password);

                // ugly but eh
                await Task.Run(async () => sftpc.Connect());
            }
        }

        /// <summary>
        /// Start TryConnect in a separate thread
        /// </summary>
        public static Task<bool> CheckAccount()
        {
            if (taskCheckAccount.Status == TaskStatus.RanToCompletion)
                taskCheckAccount = TryConnect();

            return taskCheckAccount;
        }

        /// <summary>
        /// Try to connect to the account found in Common.Profile, return
        /// bool with the connection result
        /// </summary>
        public static async Task<bool> TryConnect()
        {
            try
            {
                await Connect();
                return true;
            }
            catch
            {
                // If connecting failed, return false
                return false;
            }
        }

        /// <summary>
        /// Disconnect our client
        /// </summary>
        public static void Disconnect()
        {
            if (FTP)
                ftpc.Disconnect();
            else
                sftpc.Disconnect();
        }

        /// <summary>
        /// Returns a list of files/folders inside the given path (folder)
        /// </summary>
        public static async Task<List<ClientItem>> List(string path)
        {
            var l = new List<ClientItem>();

            if (path.StartsWith("/"))
                path = path.Substring(1);

            if (FTP)
                foreach (var f in await ftpc.GetListingAsync(path))
                {
                    ClientItemType t;
                    switch (f.Type)
                    {
                        case FtpFileSystemObjectType.File:
                            t = ClientItemType.File;
                            break;
                        case FtpFileSystemObjectType.Directory:
                            t = ClientItemType.Folder;
                            break;
                        default:
                            t = ClientItemType.Other;
                            break;
                    }
                    l.Add(new ClientItem { Name = f.Name, FullPath = f.FullName, Type = t });
                }
            else
                foreach (var s in (await sftpc.ListDirectoryAsync(path)).Where(s => s.Name != "." && s.Name != ".."))
                {
                    ClientItemType t;
                    if (s.IsRegularFile)
                        t = ClientItemType.File;
                    else if (s.IsDirectory)
                        t = ClientItemType.Folder;
                    else
                        t = ClientItemType.Other;

                    l.Add(new ClientItem { Name = s.Name, FullPath = s.FullName, Type = t });
                }

            return l;
        }

        /// <summary>
        /// Upload the captured image, file path found in CapturedImage.LocalPath
        /// </summary>
        public static async Task UploadImage(CapturedImage image)
        {
            using (var localStream = File.OpenRead(image.LocalPath))
            {
                if (FTP)
                {
                    var remoteStream = await ftpc.OpenWriteAsync(image.RemotePath);

                    var buf = new byte[ftpc.TransferChunkSize];
                    int read;
                    while ((read = await localStream.ReadAsync(buf, 0, buf.Length)) > 0)
                    {
                        await remoteStream.WriteAsync(buf, 0, read);
                    }
                    //TODO: switch to this when FluentFTP's UploadFileAsync is patched:
                    //await ftpc.UploadFileAsync(CapturedImage.LocalPath, CapturedImage.RemotePath);
                }
                else
                {
                    await sftpc.UploadAsync(localStream, image.RemotePath);
                }
            }
        }

        #endregion

        #region Members

        public static bool FTP
        {
            get { return (Common.Profile.Protocol != FtpProtocol.SFTP); }
        }

        public static bool isConnected
        {
            get { return FTP ? ftpc.IsConnected : sftpc.IsConnected; }
        }

        public static string WorkingDirectory
        {
            get { return FTP ? ftpc.GetWorkingDirectory() : sftpc.WorkingDirectory; }
            set
            {
                if (FTP)
                    ftpc.SetWorkingDirectory(value);
                else
                    sftpc.ChangeDirectory(value);
            }
        }

        public static bool Exists(string path)
        {
            return FTP ? ftpc.FileExists(path) : sftpc.Exists(path);
        }

        #endregion
    }
}