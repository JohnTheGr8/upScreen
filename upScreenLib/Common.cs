using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using upScreenLib.LogConsole;

namespace upScreenLib
{
    public static class Common
    {
        #region Fields

        // A static Profile instance to use from all forms/classes
        public static Profile Profile;        
        // Vars to determine what to do with the captured image (not saved to config)
        public static bool CopyLink = true;
        public static bool OpenInBrowser = true;
        // The allowed characters for the image file name
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        // What has finished? What next?
        public static bool KillApp = false;
        public static bool OtherFormOpen = false;
        public static bool IsImageCaptured = false;

        private static Regex imageUrlMatcher = new Regex(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$");

        #endregion        

        /// <summary>
        /// Generate a random string, used for file names
        /// </summary>
        /// <param name="length">the length of the string</param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(AllowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// Combine the given paths into one
        /// </summary>
        public static string Combine(string s1, string s2)
        {
            string c = s1;

            while (c.EndsWith("/") || c.EndsWith(@"\"))
                c = c.Substring(0, c.Length - 1);
            while (s2.StartsWith("/") || s2.StartsWith(@"\"))
                s2 = s2.Substring(1);

            c = string.Format("{0}/{1}", c, s2);

            if (c.StartsWith("/"))
                c = c.Substring(1);

            return c;
        }

        /// <summary>
        /// Get the HTTP link of the captured image
        /// </summary>
        /// <returns></returns>
        public static string GetLink(string name)
        {
            string link = Profile.RemoteHttpPath;

            if (!link.StartsWith("http://") && !link.StartsWith("https://"))
                link = $"http://{link}";

            if (!link.EndsWith("/"))
                link = link + "/";
            
            link = link + name;
            return Uri.EscapeUriString(link);
        }

        /// <summary>
        /// Kill the process, make it look like an accident...
        /// </summary>
        public static void KillProcess() => Process.GetCurrentProcess().Kill();

        /// <summary>
        /// View a link in the default browser
        /// </summary>
        public static void ViewInBrowser(string link) => Process.Start(link);

        /// <summary>
        /// Checks if the clipboard contains a file or list of files
        /// </summary>
        public static bool ImageFileInClipboard => Clipboard.ContainsFileDropList() && Clipboard.GetFileDropList().Cast<string>().All(IsValidImage);

        /// <summary>
        /// Checks if the clipboard contains a valid url
        /// </summary>        
        public static bool ImageUrlInClipboard => Clipboard.ContainsText() && imageUrlMatcher.IsMatch(Clipboard.GetText());

        /// <summary>
        /// Check if the file contains an image by trying to load it
        /// </summary>
        private static bool IsValidImage(string filename)
        {
            try
            {
                var newImage = Image.FromFile(filename);
                return true;
            }
            catch (OutOfMemoryException ex)
            {
                Log.Write(l.Error, ex.Message);
                return false;
            }            
        }

        /// <summary>
        /// Get the (string) extension of the specified ImageFormat
        /// </summary>
        public static string GetFormat(ImageFormat f = null)
        {
            // if an ImageFormat is set, return the extension as string
            if (f != null)
            {
                if (Equals(f, ImageFormat.Gif))
                    return ".gif";
                if (Equals(f, ImageFormat.Jpeg))
                    return ".jpeg";
                if (Equals(f, ImageFormat.Png))
                    return ".png";
            }
            // otherwise, return the extension based on Profile.Extension
            switch (Profile.Extension)
            {
                case ImageExtensions.PNG:
                    return ".png";
                case ImageExtensions.JPEG:
                    return ".jpg";
                case ImageExtensions.GIF:
                    return ".gif";
                default:
                    return ".png";
            }
        }

        /// <summary>
        /// Subsctract point b from point a
        /// </summary>
        public static Point Substract(this Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
    }
}