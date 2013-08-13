using System;
using System.Windows.Forms;
using System.Diagnostics;
using upScreenLib;

namespace upScreen
{
    public partial class newversion : Form
    {
        private readonly string _newVers;

        public newversion(string newv)
        {
            InitializeComponent();
            _newVers = newv;
        }

        private void newversion_Load(object sender, EventArgs e)
        {
            lCurrentVersion.Text = Application.ProductVersion.Substring(0, 5);
            lNewVersion.Text = _newVers.Substring(0, 5);
        }

        private void bDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string updaterPath = Application.StartupPath + @"\updater.exe";
                var pi = new ProcessStartInfo(updaterPath) {Verb = "runas"};
                Process.Start(pi);
            }
            catch { }

            Common.KillOrWait(true);
        }

        private void bLearnMore_Click(object sender, EventArgs e)
        {
            Common.ViewInBrowser("http://getupscreen.com");
            Common.KillOrWait(true);
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Common.KillOrWait();
        }

        private void newversion_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.KillOrWait();
        }
    }
}
