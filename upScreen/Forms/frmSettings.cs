using System;
using System.Linq;
using System.Windows.Forms;
using upScreenLib;

namespace upScreen.Forms
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            // Load the accounts list, change to the default one
            RefreshAccounts();
            // Load the default account's Format and File-Length settings
            cFormat.SelectedIndex = (int)Common.Profile.Extension;
            nLenght.Value = Convert.ToDecimal(Common.Profile.FileLenght);
            cOpenByDefault.Checked = Common.Profile.OpenInBrowser;
            cCopyByDefault.Checked = Common.Profile.CopyToClipboard;
        }

        private void cFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Profiles[cAccounts.SelectedIndex].Extension = (ImageExtensions)cFormat.SelectedIndex;
            // Save changes
            Settings.Save();
        }

        private void nLenght_ValueChanged(object sender, EventArgs e)
        {
            Settings.Profiles[cAccounts.SelectedIndex].FileLenght = Convert.ToInt32(nLenght.Value);
            // Save changes
            Settings.Save();
        }

        private void cOpenByDefault_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Profiles[cAccounts.SelectedIndex].OpenInBrowser = cOpenByDefault.Checked;
            // Save changes
            Settings.Save();
        }

        private void cCopyByDefault_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Profiles[cAccounts.SelectedIndex].CopyToClipboard = cCopyByDefault.Checked;
            // Save changes
            Settings.Save();
        }

        private void bAddAccount_Click(object sender, EventArgs e)
        {
            // Show the Add-Account dialog
            var fAccount = new frmAddAccount();
            fAccount.ShowDialog();
            // Refresh the list of accounts
            RefreshAccounts();
            // Refresh folders and image file format/length fields
            RefreshSettings();
        }

        private void bRemoveAccount_Click(object sender, EventArgs e)
        {
            // If there's only one account in the list, delete it and restart upScreen
            if (cAccounts.Items.Count == 1)
            {
                Settings.Clear();
                Application.Restart();
                Common.KillProcess();
            }
            // otherwise, just remove the selected account and save
            int i = cAccounts.SelectedIndex;            
            Settings.Profiles.RemoveAt(i);
            cAccounts.Items.RemoveAt(i);
            Settings.Save();
            cAccounts.SelectedIndex = 0;

            // Refresh image file format/length fields
            RefreshSettings();
        }

        private void bDone_Click(object sender, EventArgs e)
        {
            // Update the current profile, in case something changed
            Common.Profile.Extension = Settings.Profiles[Settings.DefaultProfile].Extension;
            Common.Profile.FileLenght = Settings.Profiles[Settings.DefaultProfile].FileLenght;
            // Close the form
            Close();
        }

        private void cAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disable the 'Set Default' button if the current profile is already the default one
            bSetDefault.Enabled = ( cAccounts.SelectedIndex != Settings.DefaultProfile );
            // Refresh image file format/length fields
            RefreshSettings();
        }

        private void bSetDefault_Click(object sender, EventArgs e)
        {
            // Disable button
            bSetDefault.Enabled = false;
            // Set all accounts as not default
            Settings.Profiles.ForEach(p => p.IsDefaultAccount = false);
            // Set the new default account
            Settings.Profiles[cAccounts.SelectedIndex].IsDefaultAccount = true;
            // Save the new Profiles list
            Settings.Save();
        }

        private void lAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.ViewInBrowser("http://getupscreen.com");
        }

        /// <summary>
        /// Refresh the Format and File Length fields based on current account
        /// Also refresh Folders list
        /// </summary>
        private void RefreshSettings()
        {
            cFormat.SelectedIndex = (int)Settings.Profiles[cAccounts.SelectedIndex].Extension;
            nLenght.Value = Settings.Profiles[cAccounts.SelectedIndex].FileLenght;
            cOpenByDefault.Checked = Settings.Profiles[cAccounts.SelectedIndex].OpenInBrowser;
            cCopyByDefault.Checked = Settings.Profiles[cAccounts.SelectedIndex].CopyToClipboard;
            RefreshFolders();
        }

        private void bSetDefaultFolder_Click(object sender, EventArgs e)
        {
            // Disable button
            bSetDefaultFolder.Enabled = false;
            // Set the new default folder index
            Settings.Profiles[cAccounts.SelectedIndex].DefaultFolder = cFolders.SelectedIndex;
            // Save the new Profiles list
            Settings.Save();
        }

        private async void bPickFolders_Click(object sender, EventArgs e)
        {
            if (frmCapture._activeAccount != cAccounts.SelectedIndex)
            {
                Client.Disconnect();
                // Switch to profile at the index of the specified account
                Common.Profile = new Profile();
                Common.Profile = Settings.Profiles[cAccounts.SelectedIndex];

                await Client.TryConnect();
            }

            // Show the Pick-Folders dialog (slightly modified Add-Account dialog)
            var fAccount = new frmAddAccount();

            frmAddAccount._pickingFolders = true;
            fAccount.ShowDialog();

            // Reset _pickingFolders
            frmAddAccount._pickingFolders = false;
            // Refresh list of Folders
            RefreshFolders();

            if (frmCapture._activeAccount != cAccounts.SelectedIndex)
            {
                Client.Disconnect();
                // Switch back to the active profile
                Common.Profile = new Profile();
                Common.Profile = Settings.Profiles[frmCapture._activeAccount];

                await Client.CheckAccount();
            }
        }

        private void cFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disable the 'Set Default' button if the current profile is already the default one
            bSetDefaultFolder.Enabled = (cFolders.SelectedIndex != Settings.Profiles[cAccounts.SelectedIndex].DefaultFolder);
        }

        /// <summary>
        /// Refresh the list of accounts
        /// </summary>
        private void RefreshAccounts()
        {
            // Load the accounts list, change to the default one
            cAccounts.Items.Clear();
            cAccounts.Items.AddRange(Settings.ProfileTitles);
            cAccounts.SelectedIndex = Settings.DefaultProfile;
        }

        /// <summary>
        /// Refresh the list of folders
        /// </summary>
        private void RefreshFolders()
        {
            var folders = Settings.Profiles[cAccounts.SelectedIndex].RemoteFolders.Select(x => x.Folder).ToArray();
            cFolders.Items.Clear();
            cFolders.Items.AddRange(folders);
            cFolders.SelectedIndex = Settings.Profiles[cAccounts.SelectedIndex].DefaultFolder;
        }
    }
}
