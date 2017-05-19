using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using upScreenLib.LogConsole;
using upScreen.Forms;
using upScreenLib;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace upScreen
{
    public partial class frmCapture : Form
    {
        #region Variable Declarations

        bool _onClick;
        bool _mouseMoved;
        bool _donedrawing;

        private bool _otherformopen = false;
        // Form instances
        private readonly frmAddAccount _fAccount = new frmAddAccount();
        private frmSettings _fSettings = new frmSettings();

        public static int _activeAccount = 0;

        #endregion

        public frmCapture()
        {
            InitializeComponent();

            CaptureControl.CaptureComplete += async (o, e) => await ProcessCapturedImages();

            // Set UrlCaptureFailed handler
            CaptureControl.UrlCaptureFailed += (o, args) =>
            {
                // Warn user and exit.
                MessageBox.Show("The provided URL could not be processed.", "upScreen Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Common.KillProcess();
            };
        }

        #region Form Event Handlers

        private async void frmCapture_Load(object sender, EventArgs e)
        {            
            // Setup the forms
            _fAccount.Tag = this;

            // Checks for previous instance of this app and kills it, if it finds it
            var tKillPrevInstances = new Thread (() =>
                {
                    try
                    {
                        string procname = Process.GetCurrentProcess().ProcessName;
                        Process[] allprocesses = Process.GetProcessesByName(procname);
                        if (allprocesses.Length <= 0) return;

                        foreach (Process p in allprocesses.Where(p => p.Id != Process.GetCurrentProcess().Id))
                            p.Kill();
                    }
                    catch { }
                });
            tKillPrevInstances.Start();

            // If uploading a file, hide the form and start uploading
            if (Profile.FromFileMenu)
            {
                _otherformopen = true;
                CaptureControl.CaptureFromArgs();
            }
            else
            {
                var width = 0;
                var height = 0;
                var top = 0;
                var left = 0;

                foreach (var screen in Screen.AllScreens)
                {
                    width += screen.Bounds.Width;
                    height += screen.Bounds.Height;

                    if (screen.WorkingArea.Top < top)
                        top = screen.WorkingArea.Top;

                    if (screen.WorkingArea.Left < left)
                        left = screen.WorkingArea.Left;
                }

                Size = new Size(width, height);
                Location = new Point(left, top);
            }

            // If Host/Username/Password are not set, call StartUpError,
            // otherwise check the account
            if (Common.Profile.IsNotSet)
                StartUpError();
            else
                await CheckAccount();

            // Refresh options in right-click menus
            _activeAccount = Settings.DefaultProfile;
            RefreshAccountList();
            RefreshFolderList();
        }

        // Kill the app if the app lost focus
        private void frmCapture_Leave(object sender, EventArgs e)
        {
            if (!_otherformopen) Common.KillProcess();
        }

        // Kill the app if the app lost focus
        private void frmCapture_Deactivate(object sender, EventArgs e)
        {
            if (ActiveForm != this && !_otherformopen) Common.KillProcess();
        }

        // Kill the app when ESC pressed
        private void frmCapture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Common.KillProcess();
        }

        private Point NativeClickPoint;
        private Point SelectionPoint;

        // set the starting point of the Selection Box
        private void frmCapture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            // Get the screen we're capturing
            CaptureControl.CaptureScreen = Screen.FromPoint(Cursor.Position);
            CaptureControl.GetBackgroundImage();
            this.TransparencyKey = Color.White;
            _onClick = true;
            // Get the click point relative to the screen we are capturing
            NativeClickPoint = MousePosition.Substract(CaptureControl.CaptureScreen.Bounds.Location);
            SelectionPoint = new Point(e.X, e.Y);
            pbSelection.Location = SelectionPoint;
        }

        // resize the selection box based on cursor position
        private void frmCapture_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_onClick) return;

            _mouseMoved = true;

            int new_x, new_y, new_width, new_height;

            if (e.X > SelectionPoint.X)
            {
                new_x = SelectionPoint.X;
                new_width = e.X - SelectionPoint.X;
            }
            else
            {
                new_x = e.X;
                new_width = SelectionPoint.X - e.X;
            }
            if (e.Y > SelectionPoint.Y)
            {
                new_y = SelectionPoint.Y;
                new_height = e.Y - SelectionPoint.Y;
            }
            else
            {
                new_y = e.Y;
                new_height = SelectionPoint.Y - e.Y;
            }

            if (!pbSelection.Visible)
            {
                pbSelection.Visible = true;
                Opacity = 0.7;
            }

            pbSelection.Location = new Point(new_x, new_y);
            pbSelection.Size = new Size(new_width, new_height);
        }

        private void frmCapture_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _onClick = false;
                _donedrawing = true;
            }
            else if (e.Button == MouseButtons.Right)
                clipboardToolStripMenuItem.Enabled = (Clipboard.ContainsImage() || Common.ImageFileInClipboard || Common.ImageUrlInClipboard);

            if (_donedrawing)
            {
                _otherformopen = true;
                Hide();

                bool ValidRectangle = !(pbSelection.Size.Width == 0 && pbSelection.Size.Height == 0);
                if (_mouseMoved && ValidRectangle)
                {
                    if (pbSelection.Size.Width == 0 || pbSelection.Size.Height == 0)
                    {
                        string errorcase = (MousePosition.X == pbSelection.Location.X) ? "width" : "height";
                        string msg = string.Format("Image {0} cannot be null", errorcase);
                        MessageBox.Show(null, msg, "upScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Common.KillProcess();
                        return;
                    }

                    // Calculate the final selected area
                    // Location should be relative to the screen we are capturing
                    var s_FinalPoint = MousePosition.Substract(CaptureControl.CaptureScreen.Bounds.Location);

                    var s_LeftX = s_FinalPoint.X > NativeClickPoint.X ? NativeClickPoint.X : s_FinalPoint.X;
                    var s_RightX = s_FinalPoint.X <= NativeClickPoint.X ? NativeClickPoint.X : s_FinalPoint.X;
                    var s_TopY = s_FinalPoint.Y > NativeClickPoint.Y ? NativeClickPoint.Y : s_FinalPoint.Y;
                    var s_BottomY = s_FinalPoint.Y <= NativeClickPoint.Y ? NativeClickPoint.Y : s_FinalPoint.Y;

                    var s_Width = s_RightX - s_LeftX;
                    var s_Height = s_BottomY - s_TopY;

                    // Capture the selected area
                    Rectangle area = new Rectangle(s_LeftX, s_TopY, s_Width, s_Height);
                    CaptureControl.CaptureArea(area);
                }
                else
                {
                    try
                    {
                        // when single-clicking over the taskbar, capture it. Otherwise, capture the window from the cursor point
                        if (NativeClickPoint.Y > CaptureControl.CaptureScreen.WorkingArea.Height)
                        {
                            Rectangle taskbar = new Rectangle(0, CaptureControl.CaptureScreen.WorkingArea.Height, CaptureControl.CaptureScreen.Bounds.Width, CaptureControl.CaptureScreen.Bounds.Height - CaptureControl.CaptureScreen.WorkingArea.Height);
                            CaptureControl.CaptureArea(taskbar);
                        } else
                            CaptureControl.CaptureWindow(Cursor.Position);
                    }
                    catch
                    {
                        Common.KillProcess();
                    }                
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            // Dont show the form if we're uploading
            // files from the context menu
            if (Profile.FromFileMenu)
            {
                this.Visible = false;
            }
        }

        #endregion

        #region Context Menu Handlers

        // Capture full screen
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _otherformopen = true;
            Hide();
            // Get the screen we're capturing
            CaptureControl.CaptureScreen = Screen.FromPoint(Cursor.Position);
            CaptureControl.CaptureFullScreen();
        }

        // Capture from clipboard
        private void clipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsImage() && !Common.ImageFileInClipboard && !Common.ImageUrlInClipboard)
                return;

            _otherformopen = true;
            Hide();
            CaptureControl.GetFromClipboard();
        }

        private void tmOpenInBrowser_Click(object sender, EventArgs e)
        {
            Common.OpenInBrowser = tmOpenInBrowser.Checked;
        }

        private void tmCopyLink_Click(object sender, EventArgs e)
        {
            Common.CopyLink = tmCopyLink.Checked;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            _onClick = false;
            _otherformopen = true;
            Hide();
            _fSettings = new frmSettings();
            _fSettings.ShowDialog();

            pbSelection.Visible = false;
            CaptureControl.GetBackgroundImage();
            Show();
            RefreshAccountList();
            RefreshFolderList();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.KillProcess();
        }

        #endregion

        #region Update System

        private async Task CheckUpdateAsync()
        {
            try
            {
                var client = new WebClient();
                var address = "http://getupscreen.com/version.txt";
                var version = await client.DownloadStringTaskAsync(address);

                Log.Write(l.Debug, "Current Version: {0} Installed Version: {1}", version, Application.ProductVersion);

                if (version != Application.ProductVersion)
                {
                    // show dialog box for download now, learn more and remind me next time
                    newversion nvform = new newversion(version) { Tag = this };
                    nvform.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Log.Write(l.Debug, "Error with version checking: {0}", ex.Message);
            }
        }

        #endregion

        #region Functions

        private async Task ProcessCapturedImages()
        {
            // upload captured images if we're ready
            await CaptureControl.CheckStartUpload();

            if (!Client.taskCheckAccount.IsCompleted) return;

            // URLs to all uploaded images
            var allLinks = new List<string>();
            foreach (var image in CaptureControl.CapturedImages)
            {
                var link = Common.GetLink(image.Name);
                allLinks.Add(link);
            }

            // Copy image links (?)
            if (Common.CopyLink)
            {
                var textToCopy = string.Join(Environment.NewLine, allLinks);
                Clipboard.SetText(textToCopy);
            }

            foreach (var image in CaptureControl.CapturedImages)
            {
                // todo: this is probably no longer necessary
                // Delete temp files
                if (!Profile.FromFileMenu)
                {
                    Log.Write(l.Info, $"Cleaning up temp image file: {image.LocalPath}");
                    File.Delete(image.LocalPath);
                }

                // Open image link in browser (?)
                if (Common.OpenInBrowser)
                {
                    var link = Common.GetLink(image.Name);
                    Common.ViewInBrowser(link);
                }
            }

            // New version?
            await CheckUpdateAsync();

            // Time to go
            Common.KillProcess();
        }

        public async Task CheckAccount()
        {
            // Try to connect
            var connectResult = await Client.CheckAccount();

            Log.Write(l.Info, $"Connected successfully: {connectResult}");

            if (!connectResult)
            {
                // Connection failed
                StartUpError();
            }
            else if (Common.IsImageCaptured)
            {
                // Connection succeeded, and user has already captured an image
                Log.Write(l.Info, "Images already captured, time to upload");
                await ProcessCapturedImages();
            }
        }

        /// <summary>
        /// If connected to the saved FTP account fails, 
        /// show the NewAccount form
        /// </summary>
        public void StartUpError()
        {
            _otherformopen = true;
            _onClick = false;

            Invoke(new MethodInvoker(() =>
                {
                    Hide();

                    _fAccount.ShowDialog();
                    pbSelection.Visible = false;
                    Show();
                }));
        }

        /// <summary>
        /// Refresh the list of accounts in the right-click menu
        /// </summary>
        public void RefreshAccountList()
        {
            accountToolStripMenuItem.DropDownItems.Clear();
            foreach (string p in Settings.ProfileTitles)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(p);
                // When the item is clicked, check it (also uncheck all other menu items)
                // and try to connect to the corresponding account.
                item.Click += async (s, ar) =>
                    {
                        foreach (ToolStripMenuItem it in accountToolStripMenuItem.DropDownItems)
                            it.Checked = false;
                        item.Checked = true;

                        int i = accountToolStripMenuItem.DropDownItems.IndexOf(item);
                        Client.Disconnect();
                        // Switch to profile at index i
                        Common.Profile = new Profile();
                        Common.Profile = Settings.Profiles[i];

                        await CheckAccount();

                        _activeAccount = i;
                        RefreshFolderList();
                    };
                if (Settings.ProfileTitles[_activeAccount] == p)
                    item.Checked = true;
                
                accountToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// Refresh the list of folders in the right-click menu
        /// </summary>
        public void RefreshFolderList()
        {
            folderToolStripMenuItem.DropDownItems.Clear();
            foreach (string p in Settings.Profiles[_activeAccount].RemoteFolders.Select(x => x.Folder))
            {
                var item = new ToolStripMenuItem(p);
                var defProfile = Settings.Profiles[_activeAccount];
                // When the item is clicked, check it and
                // set it (temporarily) as default folder.
                item.Click += (s, ar) =>
                    {
                        foreach (ToolStripMenuItem it in folderToolStripMenuItem.DropDownItems)
                            it.Checked = false;
                        item.Checked = true;

                        Common.Profile.DefaultFolder = folderToolStripMenuItem.DropDownItems.IndexOf(item);
                    };                
                if (defProfile.RemoteFolders[defProfile.DefaultFolder].Folder == p)
                    item.Checked = true;

                folderToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        #endregion        
    }
}