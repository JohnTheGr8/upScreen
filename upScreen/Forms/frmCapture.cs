using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using upScreen.Classes;
using upScreen.Forms;
using upScreenLib;

namespace upScreen
{
    public partial class frmCapture : Form
    {
        #region Variable Declarations

        bool _onClick;
        bool _mouseMoved;
        Point _clickPoint;
        bool _donedrawing;

        private bool _otherformopen = false;
        // Form instances
        private readonly frmAddAccount _fAccount = new frmAddAccount();
        private frmSettings _fSettings = new frmSettings();

        #endregion

        public frmCapture()
        {
            InitializeComponent();
            // Set UploadComplete handlers
            CaptureControl.UploadComplete += CheckForUpdate;
            // Set TryConnectCompleted handlers
            Client.TryConnectCompleted += (o, args) =>
            {
                if (!args.Success)
                    //CheckForUpdate(null, EventArgs.Empty);   // on success, check for updates
                //else
                    StartUpError();     // on failure, call StartUpError();
            };
        }

        #region Form Event Handlers

        private void frmCapture_Load(object sender, EventArgs e)
        {            
            //  Setup the forms
            _fAccount.Tag = this;

            CapturedImage.Link = "";
            CapturedImage.Uploaded = false;

            // If Host/Username/Password are not set, call StartUpError, otherwise check the account
            if ( (new[] { Common.Profile.Host, Common.Profile.Username, Common.Profile.Password }).Any(String.IsNullOrEmpty))
                StartUpError();
            else
                Client.CheckAccount();

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

            RefreshAccountList();

            // If uploading a file, hide the form and start uploading
            if (Profile.FromFileMenu)
            {
                _otherformopen = true;
                Visible = false;
                Hide();
                CaptureControl.CaptureFromArgs();
            }
            else
                Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);            
        }

        // Kill the app if the app lost focus
        private void frmCapture_Leave(object sender, EventArgs e)
        {
            if (!_otherformopen) Common.KillOrWait(true);
        }
        // Kill the app if the app lost focus
        private void frmCapture_Deactivate(object sender, EventArgs e)
        {
            if (ActiveForm != this && !_otherformopen) Common.KillOrWait(true);
        }
        // Kill the app when ESC pressed
        private void frmCapture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Common.KillOrWait(true);
        }
        // set the starting point of the Selection Box
        private void frmCapture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            CaptureControl.GetBackgroundImage();
            this.TransparencyKey = Color.White;
            _onClick = true;
            _clickPoint = MousePosition;
            pbSelection.Location = new Point(_clickPoint.X, _clickPoint.Y);
        }
        // resize the selection box based on cursor position
        private void frmCapture_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_onClick) return;

            _mouseMoved = true;

            int new_x, new_y, new_width, new_height;
            if (MousePosition.X > _clickPoint.X)
            {
                new_x = _clickPoint.X;
                new_width = MousePosition.X - _clickPoint.X;
            }
            else
            {
                new_x = MousePosition.X;
                new_width = _clickPoint.X - MousePosition.X;
            }
            if (MousePosition.Y > _clickPoint.Y)
            {
                new_y = _clickPoint.Y;
                new_height = MousePosition.Y - _clickPoint.Y;
            }
            else
            {
                new_y = MousePosition.Y;
                new_height = _clickPoint.Y - MousePosition.Y;
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
                clipboardToolStripMenuItem.Enabled = (Clipboard.ContainsImage() || Common.ImageFileInClipboard);

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
                        Common.KillOrWait(true);
                        return;
                    }
                    // Capture the selected area
                    Rectangle area = new Rectangle(pbSelection.Location, pbSelection.Size);
                    CaptureControl.CaptureArea(area);
                }
                else
                {
                                        
                    try
                    {
                        // when single-clicking over the taskbar, capture it. Otherwise, capture the window from the cursor point
                        if (MousePosition.Y > Screen.PrimaryScreen.WorkingArea.Height)
                        {
                            Rectangle taskbar = new Rectangle(0, Screen.PrimaryScreen.WorkingArea.Height, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height);
                            CaptureControl.CaptureArea(taskbar);
                        } else
                            CaptureControl.CaptureWindow(Cursor.Position);
                    }
                    catch
                    {
                        Common.KillOrWait(true);
                    }                
                }
            }
        }

        #endregion

        #region Context Menu Handlers
        
        // Capture full screen
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _otherformopen = true;
            Hide();

            CaptureControl.CaptureFullScreen();
        }

        // Capture from clipboard
        private void clipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsImage() && !Common.ImageFileInClipboard) return;

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
            // fOptions.ShowDialog();
            pbSelection.Visible = false;
            CaptureControl.GetBackgroundImage();
            Show();
            RefreshAccountList();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.KillOrWait(true);
        }

        #endregion

        #region Update System
        private WebBrowser br;        
        private void CheckForUpdate(object sender, EventArgs e)
        {
            try
            {
                br = new WebBrowser();
                br.DocumentCompleted += browser_DocumentCompleted;
                br.Navigate(@"http://sharpmindprojects.com/upscreenv.txt");
            }
            catch (Exception ex)
            {
                Log.Write(l.Debug, "Error with version checking: {0}", ex.Message);

                Common.KillOrWait();
            }
        }
        
        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                string version = br.Document.Body.InnerText;
                Log.Write(l.Debug, "Current Version: {0} Installed Version: {1}", version, Application.ProductVersion);
                Common.UpdateChecked = true;

                if (version != Application.ProductVersion)
                {
                    // show dialog box for download now, learn more and remind me next time
                    newversion nvform = new newversion(version) {Tag = this};
                    nvform.ShowDialog();                                       
                }
                else
                    Common.KillOrWait();
            }
            catch
            {
                Log.Write(l.Error, "Server down");
                Common.KillOrWait();
            }
        }
        #endregion

        #region Functions

        /// <summary>
        /// If connected to the saved FTP account fails, 
        /// show the NewAccount form
        /// </summary>
        public void StartUpError()
        {
            _otherformopen = true;
            _onClick = false;

            if (InvokeRequired)
                Invoke(new MethodInvoker(Hide));
            else
                Hide();

            _fAccount.ShowDialog();
            pbSelection.Visible = false;
            Show();
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
                item.Click += (s, ar) =>
                    {
                        foreach (ToolStripMenuItem it in accountToolStripMenuItem.DropDownItems)
                            it.Checked = false;
                        item.Checked = true;

                        int i = accountToolStripMenuItem.DropDownItems.IndexOf(item);
                        Client.Disconnect();
                        Common.Profile = new Profile
                        {
                            Protocol = Settings.Profiles[i].Protocol,
                            Host = Settings.Profiles[i].Host,
                            Username = Settings.Profiles[i].Username,
                            Password = Settings.Profiles[i].Password,
                            Port = Settings.Profiles[i].Port,

                            RemotePath = Settings.Profiles[i].RemotePaths[Settings.Profiles[i].DefaultFolder],
                            HttpPath = Settings.Profiles[i].HttpPath,

                            Extension = Settings.Profiles[i].Extension,
                            FileLenght = Settings.Profiles[i].FileLenght
                        };

                        Client.CheckAccount();
                    };
                if (Settings.ProfileTitles[Settings.DefaultProfile] == p)
                    item.Checked = true;

                accountToolStripMenuItem.DropDownItems.Add(item);                
            }
        }

        #endregion        
    }
}