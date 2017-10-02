using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using upScreenLib.LogConsole;
using upScreenLib;

namespace upScreen.Forms
{
    public partial class frmAddAccount : Form
    {
        private bool _ftpOrSftp;

        public static bool _pickingFolders = false;

        public static bool _updatingAccount = false;

        // Dictionary of cheked folders and their respective http paths
        private Dictionary<string, string> RemotePathsDictionary = new Dictionary<string, string>();

        public frmAddAccount()
        {
            InitializeComponent();
        }

        private async void frmAddAccount_Load(object sender, EventArgs e)
        {
            cMode.SelectedIndex = 0;
            cConnectionOptions.SelectedIndex = 0;

            if (_pickingFolders)
            {
                this.Text = "Pick Folders";
                gAccount.Enabled = false;
                gPaths.Enabled = true;

                // fill Account group
                cMode.SelectedIndex = Common.Profile.Protocol == FtpProtocol.FTP ? 0 : 1;
                tHost.Text = Common.Profile.Host;
                nPort.Value = Common.Profile.Port;
                tUsername.Text = Common.Profile.Username;
                tPassword.Text = Common.Profile.Password;                

                // List remote folders in treeview
                await ListDirectories();
            }
            else if (_updatingAccount)
            {
                this.Text = "Update Account Details";
                gAccount.Enabled = true;
                gPaths.Enabled = false;

                // fill Account group
                cMode.SelectedIndex = Common.Profile.Protocol == FtpProtocol.FTP ? 0 : 1;
                tHost.Text = Common.Profile.Host;
                nPort.Value = Common.Profile.Port;
                tUsername.Text = Common.Profile.Username;
                tPassword.Text = Common.Profile.Password;
            }
        }

        private void cMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change to default port number, based on 
            nPort.Value = cMode.SelectedIndex == 0 ? 21 : 22;
            // Disable for SFTP (for now)
            cConnectionOptions.Enabled = (cMode.SelectedIndex == 0);
        }

        private async void bTest_Click(object sender, EventArgs e)
        {
            _ftpOrSftp = cMode.SelectedIndex == 0;
            try
            {
                if (!_updatingAccount)
                    Common.Profile = new Profile();

                // Try connecting with the given account info
                Common.Profile.AddAccount(tHost.Text, tUsername.Text, tPassword.Text, Convert.ToInt32(nPort.Value));
                Common.Profile.Protocol = _ftpOrSftp ? FtpProtocol.FTP : FtpProtocol.SFTP;
                Common.Profile.FtpsInvokeMethod = (FtpsMethod)cConnectionOptions.SelectedIndex;
                
                Client.ValidateCertificate += (o, n) =>
                {
                    // Add certificate info
                    var msg = string.Format("{0,-25}\t {1}\n{2,-25}\t {3}\n{4,-25}\t {5}\n{6,-25}\t {7}\n\n",
                            "Valid from:", n.ValidFrom, "Valid to:", n.ValidTo, "Serial number:", n.SerialNumber, "Algorithm:", n.Algorithm);

                    msg += string.Format("Fingerprint: {0}\n\n", n.Fingerprint);
                    msg += "Trust this certificate and continue?";

                    // Ask to trust the server certificate
                    var certificateTrustedResult = MessageBox.Show(msg, "Do you trust this certificate?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    n.IsTrusted = certificateTrustedResult == DialogResult.Yes;

                    if (certificateTrustedResult == DialogResult.Yes)
                    {
                        Common.Profile.TrustedCertificate = n.Fingerprint;
                    }
                };

                await Client.Connect();

                if (_updatingAccount)
                {
                    // If connecting with existing (modified) profile works,
                    // save and exit
                    Settings.Save();
                    Close();
                }
                else
                {

                    // On success:
                    await ListDirectories();
                    tHttpPath.Text = Common.Profile.Host;
                    gPaths.Enabled = true;

                    AcceptButton = bDone;
                }
            }
            catch (Exception ex)
            {
                // On failure:
                Log.Write(l.Error, "Error: " + ex.Message);
                string msg = "Could not connect." + Environment.NewLine + "Please make sure you have typed in the right login information";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Tree Node Control

        /// <summary>
        /// List the directories in the root folder
        /// </summary>
        private async Task ListDirectories()
        {
            tFolderTree.Nodes.Clear();
            // Add the root item
            TreeNode first = new TreeNode { Text = "/" };
            tFolderTree.Nodes.Add(first);
            // Add an item for each folder in the root path
            foreach (var folderName in await Client.ListFolders("."))
            {
                TreeNode ParentNode = new TreeNode { Text = folderName };
                tFolderTree.Nodes.Add(ParentNode);

                TreeNode ChildNode = new TreeNode { Text = folderName };
                ParentNode.Nodes.Add(ChildNode);
            }

            tFolderTree.SelectedNode = first;

            if (_pickingFolders)
            {
                // Check them in the folders list
                foreach (TreeNode dir in tFolderTree.Nodes)
                    dir.Checked = (Common.Profile.RemoteFolders.Any(x => x.Folder == ConvertNodePath(dir)));
                // Load the already picked folders
                RemotePathsDictionary = Common.Profile.RemoteFolders.ToDictionary(x => x.Folder, x => x.HttpPath);
            }

            bDone.Enabled = RemotePathsDictionary.Count > 0;
        }

        private async void tFolderTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            string path = "/" + e.Node.FullPath.Replace('\\', '/');
            // Clear all child nodes
            if (e.Node.Nodes.Count > 0)
                e.Node.Nodes.Clear();

            // Add a new item to the tree for each remote folder
            foreach (var folderName in await Client.ListFolders(path))
            {
                TreeNode ParentNode = new TreeNode { Text = folderName };
                e.Node.Nodes.Add(ParentNode);

                // Check the Node we just added?
                ParentNode.Checked =
                    RemotePathsDictionary.Any(x => x.Key == ConvertNodePath(e.Node.Nodes.Cast<TreeNode>().Last()));

                TreeNode ChildNode = new TreeNode { Text = folderName };
                ParentNode.Nodes.Add(ChildNode);
            }
        }

        private void tFolderTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path = ConvertNodePath(e.Node);

            tHttpPath.Text = RemotePathsDictionary.ContainsKey(path)
                ? RemotePathsDictionary[path] : Common.Profile.Host + path;
        }

        private void tFolderTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var path = ConvertNodePath(e.Node);
            if (e.Node.Checked && RemotePathsDictionary.ContainsKey(path)) return;

            if (e.Node.Checked)
                RemotePathsDictionary.Add(path, Common.Profile.Host + path);
            else
                RemotePathsDictionary.Remove(path);

            bDone.Enabled = RemotePathsDictionary.Count > 0;
        }

        #endregion

        private void tHttpPath_TextChanged(object sender, EventArgs e)
        {
            bDone.Enabled = !string.IsNullOrWhiteSpace(tHttpPath.Text) && RemotePathsDictionary.Count > 0;

            string folder = ConvertNodePath(tFolderTree.SelectedNode);

            if (RemotePathsDictionary.ContainsKey(folder))
                RemotePathsDictionary[folder] = tHttpPath.Text;
        }

        private void bDone_Click(object sender, EventArgs e)
        {
            // Set all previous profiles to not default
            Settings.Profiles.ForEach(p => p.IsDefaultAccount = false);

            // Update profile
            Common.Profile.DefaultFolder = 0;
            Common.Profile.RemoteFolders = RemotePathsDictionary.Select(x => new RemoteFolder(x.Key, x.Value)).ToList();

            if (_pickingFolders)
                // Update the current profile
                Common.Profile = Common.Profile;
            else
            {
                // Make it default
                Common.Profile.IsDefaultAccount = true;
                // Add the new profile to the list of profiles
                Settings.Profiles.Add(Common.Profile);
            }
            // Save and close
            Settings.Save();
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            // Exit if there's no other account saved
            if (Settings.Profiles.Count == 0 || _updatingAccount) 
                Common.KillProcess();

            Close();
        }

        private void frmAddAccount_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Exit if there's no other account saved
            if (Settings.Profiles.Count == 0)
                Common.KillProcess();
        }

        private string ConvertNodePath(TreeNode tn)
        {
            string path = "/" + tn.FullPath.Replace('\\', '/');
            if (path.EndsWith(".."))
                path = path.Substring(0, path.Length - 2);
            else if (path.EndsWith("."))
                path = path.Substring(0, path.Length - 1);
            else if (path.EndsWith("//"))
                path = path.Substring(0, path.Length - 1);

            return path;
        }
    }
}
