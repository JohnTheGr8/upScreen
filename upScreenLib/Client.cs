using System;
using System.IO;
using System.Linq;
using System.Threading;
using Starksoft.Net.Ftp;
using Renci.SshNet;
using System.Collections.Generic;

namespace upScreenLib
{
    public static class Client
    {
        private static SftpClient sftpc;
        private static FtpClient ftpc;

        public static event EventHandler<TryConnectEventArgs> TryConnectCompleted;

        public static Thread tCheckAccount = new Thread(TryConnect);

        #region Functions

        /// <summary>
        /// Connect to the remote server, using the 
        /// account info found in Common.Profile
        /// </summary>
        public static void Connect()
        {
            if (FTP)
            {
                ftpc = new FtpClient(Common.Profile.Host, Common.Profile.Port);
                switch (Common.Profile.FtpsInvokeMethod)
                {
                    case 0:
                        goto default;
                    case FtpsMethod.Explicit:
                        ftpc.SecurityProtocol = FtpSecurityProtocol.Tls1OrSsl3Explicit;
                        ftpc.ValidateServerCertificate += (o, e) => e.IsCertificateValid = true;
                        break;
                    case FtpsMethod.Implicit:
                        ftpc.SecurityProtocol = FtpSecurityProtocol.Tls1OrSsl3Implicit;
                        ftpc.ValidateServerCertificate += (o, e) => e.IsCertificateValid = true;
                        break;
                    default:
                        ftpc.SecurityProtocol = FtpSecurityProtocol.None;
                        break;
                }
                ftpc.Open(Common.Profile.Username, Common.Profile.Password);
            }
            else
            {
                sftpc = new SftpClient(Common.Profile.Host, Common.Profile.Port, 
                        Common.Profile.Username, Common.Profile.Password);
                sftpc.Connect();
            }
        }

        /// <summary>
        /// Start TryConnect in a separate thread
        /// </summary>
        public static void CheckAccount()
        {
            tCheckAccount = new Thread(TryConnect);
            tCheckAccount.Start();
        }

        /// <summary>
        /// Try to connect to the account found in Common.Profile, then raise 
        /// TryConnectCompleted to show if the attempt was successful
        /// </summary>
        public static void TryConnect()
        {
            try
            {
                Connect();
                // Setup the event handler for the async upload
                if (Common.Profile.Protocol != FtpProtocol.SFTP)
                    ftpc.PutFileAsyncCompleted += CaptureControl.ImageUploaded;

                // If the image has been captured, start the upload
                if (Common.IsImageCaptured)
                {
                    CaptureControl.DoStartUpload();
                    TryConnectCompleted(null, new TryConnectEventArgs { Success = true });                    
                }
            }
            catch
            {
                // If connecting failed, raise TryConnectCompleted with Success set to false 
                TryConnectCompleted(null, new TryConnectEventArgs{ Success = false });
            }
        }

        /// <summary>
        /// Disconnect our client
        /// </summary>
        public static void Disconnect()
        {
            if (FTP)
                ftpc.Close();
            else
                sftpc.Disconnect();
        }

        /// <summary>
        /// Returns a list of files/folders inside the given path (folder)
        /// </summary>
        public static List<ClientItem> List(string path)
        {
            var l = new List<ClientItem>();

            if (path.StartsWith("/"))
                path = path.Substring(1);

            if (FTP)
                foreach (FtpItem f in ftpc.GetDirList(path))
                {
                    ClientItemType t;
                    switch (f.ItemType)
                    {
                        case FtpItemType.File:
                            t = ClientItemType.File;
                            break;
                        case FtpItemType.Directory:
                            t = ClientItemType.Folder;
                            break;
                        default:
                            t = ClientItemType.Other;
                            break;
                    }
                    l.Add(new ClientItem { Name = f.Name, FullPath = f.FullPath, Type = t });
                }
            else
                foreach (var s in sftpc.ListDirectory(path).Where(s => s.Name != "." && s.Name != ".."))
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
        public static void UploadCapturedImage()
        {
            if (FTP)
                ftpc.PutFileAsync(CapturedImage.LocalPath, CapturedImage.RemotePath, FileAction.CreateNew);            
            else
            {
                EventWaitHandle wait = new AutoResetEvent(false);

                using (var file = File.OpenRead(CapturedImage.LocalPath))
                {
                    var asynch = sftpc.BeginUploadFile(file, CapturedImage.RemotePath, delegate
                    {
                        Console.Write("\nCallback called.");
                        wait.Set();
                    },
                        null);

                    var sftpASynch = asynch as Renci.SshNet.Sftp.SftpUploadAsyncResult;
                    while (!sftpASynch.IsCompleted)
                    {
                        if (sftpASynch.UploadedBytes > 0)
                            Console.Write("\rUploaded {0} bytes ", sftpASynch.UploadedBytes);
                        Thread.Sleep(100);
                    }
                    Console.Write("\rUploaded {0} bytes!", sftpASynch.UploadedBytes);
                    sftpc.EndUploadFile(asynch);
                    wait.WaitOne();
                }

                CaptureControl.ImageUploaded();
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
            get { return FTP ? ftpc.CurrentDirectory : sftpc.WorkingDirectory; }
            set
            {
                if (FTP)
                    ftpc.ChangeDirectory(value);
                else
                    sftpc.ChangeDirectory(value);
            }
        }

        public static bool Exists(string path)
        {
            return FTP ? ftpc.Exists(path) : sftpc.Exists(path);
        }

        #endregion
    }

    public class TryConnectEventArgs : EventArgs
    {
        public bool Success;    // Was the connection established successfully?
    }
}