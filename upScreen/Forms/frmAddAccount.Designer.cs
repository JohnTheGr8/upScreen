namespace upScreen.Forms
{
    partial class frmAddAccount
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddAccount));
            this.gAccount = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bTest = new System.Windows.Forms.Button();
            this.cMode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tPassword = new System.Windows.Forms.TextBox();
            this.tUsername = new System.Windows.Forms.TextBox();
            this.bDone = new System.Windows.Forms.Button();
            this.gPaths = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tHttpPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tRemotePath = new System.Windows.Forms.TextBox();
            this.tFolderTree = new System.Windows.Forms.TreeView();
            this.bCancel = new System.Windows.Forms.Button();
            this.gAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.gPaths.SuspendLayout();
            this.SuspendLayout();
            // 
            // gAccount
            // 
            this.gAccount.Controls.Add(this.label6);
            this.gAccount.Controls.Add(this.bTest);
            this.gAccount.Controls.Add(this.cMode);
            this.gAccount.Controls.Add(this.label10);
            this.gAccount.Controls.Add(this.nPort);
            this.gAccount.Controls.Add(this.label3);
            this.gAccount.Controls.Add(this.tHost);
            this.gAccount.Controls.Add(this.label2);
            this.gAccount.Controls.Add(this.label5);
            this.gAccount.Controls.Add(this.tPassword);
            this.gAccount.Controls.Add(this.tUsername);
            this.gAccount.Location = new System.Drawing.Point(11, 12);
            this.gAccount.Name = "gAccount";
            this.gAccount.Size = new System.Drawing.Size(353, 164);
            this.gAccount.TabIndex = 51;
            this.gAccount.TabStop = false;
            this.gAccount.Text = "Account";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(261, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 59;
            this.label6.Text = ":";
            // 
            // bTest
            // 
            this.bTest.Location = new System.Drawing.Point(255, 129);
            this.bTest.Name = "bTest";
            this.bTest.Size = new System.Drawing.Size(75, 23);
            this.bTest.TabIndex = 5;
            this.bTest.Text = "Test";
            this.bTest.UseVisualStyleBackColor = true;
            this.bTest.Click += new System.EventHandler(this.bTest_Click);
            // 
            // cMode
            // 
            this.cMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMode.FormattingEnabled = true;
            this.cMode.Items.AddRange(new object[] {
            "FTP",
            "SFTP"});
            this.cMode.Location = new System.Drawing.Point(99, 24);
            this.cMode.Name = "cMode";
            this.cMode.Size = new System.Drawing.Size(57, 21);
            this.cMode.TabIndex = 0;
            this.cMode.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "Mode:";
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(277, 51);
            this.nPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(53, 20);
            this.nPort.TabIndex = 2;
            this.nPort.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "Host:";
            // 
            // tHost
            // 
            this.tHost.Location = new System.Drawing.Point(99, 51);
            this.tHost.Name = "tHost";
            this.tHost.Size = new System.Drawing.Size(156, 20);
            this.tHost.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 55;
            this.label5.Text = "Username:";
            // 
            // tPassword
            // 
            this.tPassword.Location = new System.Drawing.Point(99, 103);
            this.tPassword.Name = "tPassword";
            this.tPassword.PasswordChar = '●';
            this.tPassword.Size = new System.Drawing.Size(231, 20);
            this.tPassword.TabIndex = 4;
            // 
            // tUsername
            // 
            this.tUsername.Location = new System.Drawing.Point(99, 77);
            this.tUsername.Name = "tUsername";
            this.tUsername.Size = new System.Drawing.Size(231, 20);
            this.tUsername.TabIndex = 3;
            // 
            // bDone
            // 
            this.bDone.Enabled = false;
            this.bDone.Location = new System.Drawing.Point(289, 398);
            this.bDone.Name = "bDone";
            this.bDone.Size = new System.Drawing.Size(75, 23);
            this.bDone.TabIndex = 9;
            this.bDone.Text = "Done";
            this.bDone.UseVisualStyleBackColor = true;
            this.bDone.Click += new System.EventHandler(this.bDone_Click);
            // 
            // gPaths
            // 
            this.gPaths.Controls.Add(this.label4);
            this.gPaths.Controls.Add(this.tHttpPath);
            this.gPaths.Controls.Add(this.label1);
            this.gPaths.Controls.Add(this.tRemotePath);
            this.gPaths.Controls.Add(this.tFolderTree);
            this.gPaths.Enabled = false;
            this.gPaths.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.gPaths.Location = new System.Drawing.Point(11, 182);
            this.gPaths.Name = "gPaths";
            this.gPaths.Size = new System.Drawing.Size(353, 210);
            this.gPaths.TabIndex = 62;
            this.gPaths.TabStop = false;
            this.gPaths.Text = "Paths";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "HTTP path:";
            // 
            // tHttpPath
            // 
            this.tHttpPath.Location = new System.Drawing.Point(99, 171);
            this.tHttpPath.Name = "tHttpPath";
            this.tHttpPath.Size = new System.Drawing.Size(231, 20);
            this.tHttpPath.TabIndex = 8;
            this.tHttpPath.TextChanged += new System.EventHandler(this.tHttpPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "Remote path:";
            // 
            // tRemotePath
            // 
            this.tRemotePath.Enabled = false;
            this.tRemotePath.Location = new System.Drawing.Point(99, 145);
            this.tRemotePath.Name = "tRemotePath";
            this.tRemotePath.Size = new System.Drawing.Size(231, 20);
            this.tRemotePath.TabIndex = 7;
            this.tRemotePath.Text = "/";
            // 
            // tFolderTree
            // 
            this.tFolderTree.Location = new System.Drawing.Point(15, 20);
            this.tFolderTree.Name = "tFolderTree";
            this.tFolderTree.Size = new System.Drawing.Size(315, 110);
            this.tFolderTree.TabIndex = 6;
            this.tFolderTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tFolderTree_AfterExpand);
            this.tFolderTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tFolderTree_AfterSelect);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(208, 398);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 63;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // frmAddAccount
            // 
            this.AcceptButton = this.bTest;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 433);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.gPaths);
            this.Controls.Add(this.bDone);
            this.Controls.Add(this.gAccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add an account";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAddAccount_FormClosed);
            this.Load += new System.EventHandler(this.frmAddAccount_Load);
            this.gAccount.ResumeLayout(false);
            this.gAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.gPaths.ResumeLayout(false);
            this.gPaths.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gAccount;
        private System.Windows.Forms.Button bTest;
        private System.Windows.Forms.ComboBox cMode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tPassword;
        private System.Windows.Forms.TextBox tUsername;
        private System.Windows.Forms.Button bDone;
        private System.Windows.Forms.GroupBox gPaths;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tHttpPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tRemotePath;
        private System.Windows.Forms.TreeView tFolderTree;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bCancel;

    }
}