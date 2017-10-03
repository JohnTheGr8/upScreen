using System;
using System.Windows.Forms;
using upScreenLib;

namespace upScreen
{
    public partial class newversion : Form
    {
        bool _updating;

        public newversion(string oldv, string newv)
        {
            InitializeComponent();

            lCurrentVersion.Text = oldv;
            lNewVersion.Text = newv;
        }

        private void bDownload_Click(object sender, EventArgs e)
        {
            _updating = true;
            Close();
        }

        private void bLearnMore_Click(object sender, EventArgs e)
        {
            Common.ViewInBrowser("https://github.com/JohnTheGr8/upScreen/blob/master/CHANGELOG.md");
            Common.KillProcess();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Common.KillProcess();
        }

        private void newversion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_updating)
            {
                Common.KillProcess();
            }
        }
    }
}
