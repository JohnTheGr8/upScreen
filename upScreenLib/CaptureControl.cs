using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using upScreenLib.LogConsole;

namespace upScreenLib
{
    public class CaptureControl
    {
        // Event raised when the image(s) has been captured
        public static event EventHandler CaptureComplete;

        // Event raised when the given url cannot be processed correctly
        public static event EventHandler<EventArgs> UrlCaptureFailed;

        // The screen which we're going to capture, defaults to the primary screen
        public static Screen CaptureScreen = Screen.PrimaryScreen;

        // List of files passed in as arguements
        public static List<string> ArgFiles = new List<string>();

        // All images captured in this session
        public static List<CapturedImage> CapturedImages = new List<CapturedImage>();

        // Screenshot of the entire CaptureScreen
        private static Bitmap FullScreen;

        #region Capture Full Screen

        public static void CaptureFullScreen()
        {
            GetBackgroundImage();

            Log.Write(l.Info, "Full screeen mode");

            SaveImage(FullScreen);
        }

        #endregion

        #region Capture Area

        public static void CaptureArea(Rectangle area)
        {
            Log.Write(l.Info, $"Area mode, x: {area.X} y: {area.Y} width: {area.Width} height: {area.Height}");

            var image = Crop(FullScreen, area);

            SaveImage(image);
        }

        #endregion

        #region Capture Window

        /// <summary>
        /// Find and capture the window 'behind' the specified point
        /// </summary>
        public static void CaptureWindow(Point p)
        {
            IntPtr window = ApiWrapper.Window.WindowFromPoint(p);

            if (window == IntPtr.Zero) return;

            var windowText = ApiWrapper.Window.GetWindowText(window);
            Log.Write(l.Info, $"Window mode: {windowText}");

            // Find the window from the click point
            while (ApiWrapper.Window.IsChild(ApiWrapper.Window.GetParent(window), window))
                window = ApiWrapper.Window.GetParent(window);

            // rectangle at the position and size of the selected window
            Rectangle windowRect = ApiWrapper.Window.GetWindowRect(window);

            // find the height of the windows taskbar
            int tbheight = CaptureScreen.Bounds.Height - CaptureScreen.WorkingArea.Height;

            // fix for when the window height is maximum
            if (windowRect.Height + windowRect.Y + tbheight + windowRect.Y == CaptureScreen.Bounds.Height)
            {
                windowRect.Y = 0;
                windowRect.Height = CaptureScreen.WorkingArea.Height;
            }

            // Make the rectangle location relative to the screen we are capturing
            windowRect.Location = windowRect.Location.Substract(CaptureScreen.Bounds.Location);

            // Fix negative locations
            if (windowRect.Location.X < 0)
            {
                windowRect.Width = windowRect.Width + windowRect.X;
                windowRect.X = 0;
            }
            if (windowRect.Location.Y < 0)
            {
                windowRect.Height = windowRect.Height + windowRect.Y;
                windowRect.Y = 0;
            }

            // Crop the full-screen image at the position/dimmensions of the selected window
            var image = Crop(FullScreen, windowRect);

            Common.OtherFormOpen = true;

            SaveImage(image);
        }

        #endregion

        #region Capture from Arguements

        /// <summary>
        /// Used when a file path is passed as an arguement to upScreen (when uploading from the context menus)
        /// Loads the image file into CapturedImage.Image
        /// </summary>
        public static void CaptureFromArgs()
        {
            Log.Write(l.Info, $"Context menu mode: {ArgFiles.Count} files");

            foreach (string filePath in ArgFiles)
            {
                Common.OtherFormOpen = true;

                SaveImage(filePath);
            }

            // raise CaptureComplete to start uploading
            CaptureComplete(null, EventArgs.Empty);
        }

        #endregion        

        #region Image Editing and Finalizing

        /// <summary>
        /// Crop the given image at the given rectangle's positon/dimmensions
        /// </summary>
        /// <param name="img">The image to crop</param>
        /// <param name="rect">The rectangle that defines the crop starting point and dimmensions</param>
        /// <returns></returns>
        public static Image Crop(Image img, Rectangle rect)
        {
            var bmp = new Bitmap(img);
            Rectangle frect = FinalizeRectangle(rect);
            Image bmpCropped = bmp.Clone(frect, bmp.PixelFormat);
            return bmpCropped;
        }

        /// <summary>
        /// Check if the rectangle gets outside the screen boundaries
        /// </summary>
        private static Rectangle FinalizeRectangle(Rectangle rect)
        {
            if (rect.Location.X + rect.Size.Width > CaptureScreen.Bounds.Width)
                rect.Width = CaptureScreen.Bounds.Width - rect.Location.X;

            if (rect.Location.Y + rect.Size.Height > CaptureScreen.Bounds.Height)
                rect.Height = CaptureScreen.Bounds.Height - rect.Location.Y;

            return rect;
        }

        public static void SaveImage(Image image)
        {
            var captureName = Common.RandomString(Common.Profile.FileLenght) + Common.GetFormat();

            var info = new CapturedImage
            {
                Name = captureName,
                LocalPath = Path.Combine(Settings.AppDataFolder, captureName),
                RemotePath = Common.Combine(Common.Profile.RemoteFolder, captureName)
            };

            Log.Write(l.Info, $"Saving image as: {info.LocalPath}");

            image.Save(info.LocalPath);
            CapturedImages.Add(info);

            // raise CaptureComplete to start uploading
            CaptureComplete(null, EventArgs.Empty);
        }

        public static void SaveImage(string localPath)
        {
            var image = Image.FromFile(localPath);
            var captureName = Common.RandomString(Common.Profile.FileLenght) + Common.GetFormat(image.RawFormat);

            var info = new CapturedImage
            {
                Name = captureName,
                LocalPath = Path.Combine(Settings.AppDataFolder, captureName),
                RemotePath = Common.Combine(Common.Profile.RemoteFolder, captureName)
            };

            Log.Write(l.Info, $"Saving image as: {info.LocalPath}");

            image.Save(info.LocalPath);
            CapturedImages.Add(info);
        }

        #endregion

        #region Image Uploading

        /// <summary>
        /// Start the upload if the client is connected and ready to go
        /// </summary>
        public static async Task CheckStartUpload()
        {
            if (!Client.taskCheckAccount.IsCompleted)
            {
                Log.Write(l.Info, "Image captured, client not ready to upload yet");
                Common.IsImageCaptured = true;
            }
            else
                await UploadImages();
        }

        /// <summary>
        /// Upload the captured images
        /// </summary>
        public static async Task UploadImages()
        {
            foreach (var image in CapturedImages)
            {
                await Client.UploadImage(image);
            }
        }

        #endregion        

        #region Image fetching

        /// <summary>
        /// Capture the entire screen into CapturedImage.Bmp
        /// </summary>
        public static void GetBackgroundImage()
        {
            var bmp = new Bitmap(CaptureScreen.Bounds.Width, CaptureScreen.Bounds.Height);
            var gfx = Graphics.FromImage(bmp);
            gfx.CopyFromScreen(CaptureScreen.Bounds.X, CaptureScreen.Bounds.Y, 0, 0, CaptureScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            FullScreen = bmp;
        }

        /// <summary>
        /// Set CapturedImage.Image as the image/image file copied to clipboard
        /// </summary>
        public static void GetFromClipboard()
        {
            Image image = null;

            // Load the image from a copied image
            if (Clipboard.ContainsImage())
            {
                Log.Write(l.Info, "Clipboard mode, copied image");
                image = Clipboard.GetImage();
            }
            // Load the image from a copied image file
            else if (Clipboard.ContainsFileDropList())
            {
                Log.Write(l.Info, "Clipboard mode, file list");
                foreach (var localFile in Clipboard.GetFileDropList())
                {
                    SaveImage(localFile);
                }
                // raise CaptureComplete to start uploading
                CaptureComplete(null, EventArgs.Empty);
            }
            // Load the image from a copied image url
            else if (Common.ImageUrlInClipboard)
            {
                var url = Clipboard.GetText();
                Log.Write(l.Info, $"Clipboard mode, url: {url}");
                image = GetFromUrl(url);
            }

            // Prevent null reference exception
            if (image == null) return;

            Common.OtherFormOpen = true;

            SaveImage(image);
        }

        /// <summary>
        /// Download and return image from url
        /// </summary>
        /// <param name="url">url to image</param>        
        private static Image GetFromUrl(string url)
        {
            var uri = new UriBuilder(url).Uri;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "HEAD";
                request.ServicePoint.Expect100Continue = false;
                request.ContentType = "application/x-www-form-urlencoded";

                using (var response = request.GetResponse())
                {
                    var fileType = response.Headers[HttpResponseHeader.ContentType];
                    var allowedTypes = new List<string> { "image/png", "image/jpeg", "image/jpg", "image/gif" };

                    //Compare web file MIME to valid types
                    if (allowedTypes.Contains(fileType))
                    {
                        //Download file to MemoryStream
                        var imgBytes = new WebClient().DownloadData(uri);
                        var memStream = new MemoryStream(imgBytes);
                        return Image.FromStream(memStream);
                    }
                }
            }
            catch
            {
                // Something went wrong...
                UrlCaptureFailed(null, EventArgs.Empty);
            }
            return null;
        }

        #endregion
    }
}