using System;
using System.Collections.Generic;
using System.Windows.Forms;
using upScreenLib.LogConsole;
using upScreenLib;

namespace upScreen.Forms
{
    public partial class frmAddAccount : Form
    {
        private bool _ftpOrSftp;

        public frmAddAccount()
        {
            InitializeComponent();
        }

        private void frmAddAccount_Load(object sender, EventArgs e)
        {
            cMode.SelectedIndex = 0;
        }

        private void cMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change to default port number, based on 
            nPort.Value = cMode.SelectedIndex == 0 ? 21 : 22;
        }

        private void bTest_Click(object sender, EventArgs e)
        {
            _ftpOrSftp = cMode.SelectedIndex == 0;
            try
            {
                // Try connecting with the given account info
                Common.Profile.AddAccount(tHost.Text, tUsername.Text, tPassword.Text, Convert.ToInt32(nPort.Value));
                Common.Profile.Protocol = _ftpOrSftp ? FtpProtocol.FTP : FtpProtocol.SFTP;

                Client.Connect();

                // On success:
                ListDirectories();
                tHttpPath.Text = Common.Profile.Host;
                gPaths.Enabled = true;
                bDone.Enabled = true;

                AcceptButton = bDone;
            }
            catch (Exception ex)
            {
                // On failure:
                Console.WriteLine("Error: " + ex.Message);
                string msg = "Could not connect." + Environment.NewLine + "Please make sure you have typed in the right login information";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Tree Node Control

        /// <summary>
        /// List the directories in the root folder
        /// </summary>
        private void ListDirectories()
        {
            tFolderTree.Nodes.Clear();
            // Add the root item
            TreeNode first = new TreeNode { Text = "/" };
            tFolderTree.Nodes.Add(first);
            // Add an item for each folder in the root path
            foreach (var c in Client.List("."))
            {
                if (c.Type != ClientItemType.Folder) continue;

                TreeNode ParentNode = new TreeNode { Text = c.Name };
                tFolderTree.Nodes.Add(ParentNode);

                TreeNode ChildNode = new TreeNode { Text = c.Name };
                ParentNode.Nodes.Add(ChildNode);
            }

            tFolderTree.SelectedNode = first;
        }

        private void tFolderTree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            string path = "/" + e.Node.FullPath.Replace('\\', '/');
            // Clear all child nodes
            if (e.Node.Nodes.Count > 0)
            {
                int i = e.Node.Index;
                foreach (TreeNode tn in e.Node.Nodes)                
                    try
                    {
                        tFolderTree.Nodes[i].Nodes.Remove(tn);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(l.Debug, ex.Message);
                    }             
            }
            // Add a new item to the tree for each remote folder
            foreach (ClientItem c in Client.List(path))
            {
                // We only want to list folders...
                if (c.Type != ClientItemType.Folder) continue;

                TreeNode ParentNode = new TreeNode { Text = c.Name };
                e.Node.Nodes.Add(ParentNode);

                TreeNode ChildNode = new TreeNode { Text = c.Name };
                ParentNode.Nodes.Add(ChildNode);
            }
        }

        private void tFolderTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path = "/" + e.Node.FullPath.Replace('\\', '/');
            if (path.EndsWith(".."))
                path = path.Substring(0, path.Length - 2);
            else if (path.EndsWith("."))
                path = path.Substring(0, path.Length - 1);
            else if (path.EndsWith("//"))
                path = path.Substring(0, path.Length - 1);

            tRemotePath.Text = path;
            tHttpPath.Text = Common.Profile.Host + path;
        }

        #endregion

        private void tHttpPath_TextChanged(object sender, EventArgs e)
        {
            bDone.Enabled = !string.IsNullOrWhiteSpace(tHttpPath.Text);
        }

        private void bDone_Click(object sender, EventArgs e)
        {
            string rPath = (tRemotePath.Text == "/." || tRemotePath.Text == "//..") ? "" : tRemotePath.Text.Substring(1, tRemotePath.Text.Length - 1);
            Common.Profile.AddPaths(rPath, tHttpPath.Text);
            
            // Set all previous profiles to not default
            Settings.Profiles.ForEach(p => p.IsDefaultAccount = false);

            // Add the new profile to the list of profiles
            Settings.Profiles.Add(new SettingsProfile
            {
                // Account info
                Protocol = _ftpOrSftp ? FtpProtocol.FTP : FtpProtocol.SFTP,
                Host = Common.Profile.Host,
                Username = Common.Profile.Username,
                Password = Common.Profile.Password,
                Port = Common.Profile.Port,
                // Paths info
                RemotePaths = new List<string>(Common.Profile.RemotePaths),
                HttpPath = Common.Profile.HttpPath,
                DefaultFolder = Common.Profile.DefaultFolder,
                IsDefaultAccount = true
            });
            // Save and close
            Settings.Save();
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            // Exit if there's no other account saved
            if (Settings.Profiles.Count == 0) 
                Common.KillOrWait(true);

            Close();
        }

        private void frmAddAccount_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Exit if there's no other account saved
            if (Settings.Profiles.Count == 0)
                Common.KillOrWait(true);
        }
    }
}
