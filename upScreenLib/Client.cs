using System;
using System.IO;
using System.Linq;
using FluentFTP;
using Renci.SshNet;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Renci.SshNet.Async;
using upScreenLib.LogConsole;

namespace upScreenLib
{
    public static class Client
    {
        private static SftpClient sftpc;
        private static FtpClient ftpc;

        public static Task<bool> taskCheckAccount = TryConnect();

        public static event EventHandler<ValidateCertificateEventArgs> ValidateCertificate;

        #region Functions

        /// <summary>
        /// Connect to the remote server, using the 
        /// account info found in Common.Profile
        /// </summary>
        public static async Task Connect()
        {
            Log.Write(l.Client, $"Connecting {Common.Profile.Username}@{Common.Profile.Host}");

            if (FTP)
            {
                ftpc = new FtpClient(Common.Profile.Host, Common.Profile.Username, Common.Profile.Password);

                // Check if the server returned certificate is valid
                ftpc.ValidateCertificate += (o, e) =>
                {
                    var thumbprint = new X509Certificate2(e.Certificate).Thumbprint;

                    if (Common.Profile.TrustedCertificate == thumbprint)
                    {
                        Log.Write(l.Client, $"Valid certificate: {thumbprint}");
                        e.Accept = true;
                    }
                    else if (ValidateCertificate != null)
                    {
                        var args = (ValidateCertificateEventArgs)e;
                        // Prompt user to validate
                        ValidateCertificate(null, args);
                        e.Accept = args.IsTrusted;
                    }
                    else
                    {
                        e.Accept = false;
                    }
                };

                ftpc.Port = Common.Profile.Port;

                // Set encryption mode
                ftpc.EncryptionMode = (FtpEncryptionMode)Common.Profile.FtpsInvokeMethod;
                Log.Write(l.Client, $"Encryption mode: {ftpc.EncryptionMode}");

                switch (Common.Profile.FtpsInvokeMethod)
                {
                    case FtpsMethod.Explicit:
                    case FtpsMethod.Implicit:
                        //todo: what about e.PolicyErrors
                        ftpc.SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls12;
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

            Log.Write(l.Client, "Disconnected");
        }

        /// <summary>
        /// Returns a list of folders inside the given path (folder)
        /// </summary>
        public static async Task<IEnumerable<string>> ListFolders(string path)
        {
            if (FTP)
            {
                var list = await ftpc.GetListingAsync(path);
                return list.Where(x => x.Type == FtpFileSystemObjectType.Directory).Select(x => x.Name);
            }
            else
            {
                var list = await sftpc.ListDirectoryAsync(path);
                return list.Where(x => x.IsDirectory && x.Name != "." && x.Name != "..").Select(x => x.Name);
            }
        }

        /// <summary>
        /// Upload the captured image, file path found in CapturedImage.LocalPath
        /// </summary>
        public static async Task UploadImage(CapturedImage image)
        {
            Log.Write(l.Client, $"Uploading {image.Name} to {image.RemotePath}");

            if (FTP)
            {
                await ftpc.UploadFileAsync(image.LocalPath, image.RemotePath);
            }
            else
            {
                using (var localStream = File.OpenRead(image.LocalPath))
                {
                    await sftpc.UploadAsync(localStream, image.RemotePath);
                }
            }
        }

        #endregion

        #region Members

        public static bool FTP => Common.Profile.Protocol != FtpProtocol.SFTP;

        public static bool isConnected => FTP ? ftpc.IsConnected : sftpc.IsConnected;

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

        public static bool Exists(string path) => FTP ? ftpc.FileExists(path) : sftpc.Exists(path);

        #endregion
    }

    public class ValidateCertificateEventArgs : EventArgs
    {
        public string Fingerprint;

        // FTPS info
        public string SerialNumber;
        public string Algorithm;
        public string Issuer;
        public string ValidFrom;
        public string ValidTo;

        // Trust the certificate?
        public bool IsTrusted;

        public static explicit operator ValidateCertificateEventArgs(FtpSslValidationEventArgs e)
        {
            return new ValidateCertificateEventArgs
            {
                Fingerprint = ((X509Certificate2)e.Certificate).Thumbprint,
                SerialNumber = e.Certificate.GetSerialNumberString(),
                Algorithm = e.Certificate.GetKeyAlgorithmParametersString(),
                ValidFrom = e.Certificate.GetEffectiveDateString(),
                ValidTo = e.Certificate.GetExpirationDateString(),
                Issuer = e.Certificate.Issuer
            };
        }
    }
}