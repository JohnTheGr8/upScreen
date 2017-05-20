namespace upScreen.Forms
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nLenght = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.cFormat = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bSetDefaultFolder = new System.Windows.Forms.Button();
            this.bPickFolders = new System.Windows.Forms.Button();
            this.cFolders = new System.Windows.Forms.ComboBox();
            this.bSetDefault = new System.Windows.Forms.Button();
            this.bRemoveAccount = new System.Windows.Forms.Button();
            this.bAddAccount = new System.Windows.Forms.Button();
            this.cAccounts = new System.Windows.Forms.ComboBox();
            this.lAbout = new System.Windows.Forms.LinkLabel();
            this.bDone = new System.Windows.Forms.Button();
            this.cOpenByDefault = new System.Windows.Forms.CheckBox();
            this.cCopyByDefault = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nLenght)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cCopyByDefault);
            this.groupBox1.Controls.Add(this.cOpenByDefault);
            this.groupBox1.Controls.Add(this.nLenght);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.cFormat);
            this.groupBox1.Location = new System.Drawing.Point(12, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 131);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // nLenght
            // 
            this.nLenght.Location = new System.Drawing.Point(92, 45);
            this.nLenght.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nLenght.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nLenght.Name = "nLenght";
            this.nLenght.Size = new System.Drawing.Size(57, 20);
            this.nLenght.TabIndex = 1;
            this.nLenght.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nLenght.ValueChanged += new System.EventHandler(this.nLenght_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Image Format:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 47);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(62, 13);
            this.label20.TabIndex = 6;
            this.label20.Text = "File Lenght:";
            // 
            // cFormat
            // 
            this.cFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cFormat.FormattingEnabled = true;
            this.cFormat.Items.AddRange(new object[] {
            ".png",
            ".jpg",
            ".gif"});
            this.cFormat.Location = new System.Drawing.Point(92, 15);
            this.cFormat.Name = "cFormat";
            this.cFormat.Size = new System.Drawing.Size(57, 21);
            this.cFormat.TabIndex = 0;
            this.cFormat.SelectedIndexChanged += new System.EventHandler(this.cFormat_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.bSetDefaultFolder);
            this.groupBox2.Controls.Add(this.bPickFolders);
            this.groupBox2.Controls.Add(this.cFolders);
            this.groupBox2.Controls.Add(this.bSetDefault);
            this.groupBox2.Controls.Add(this.bRemoveAccount);
            this.groupBox2.Controls.Add(this.bAddAccount);
            this.groupBox2.Controls.Add(this.cAccounts);
            this.groupBox2.Location = new System.Drawing.Point(13, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 180);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Accounts";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Folders:";
            // 
            // bSetDefaultFolder
            // 
            this.bSetDefaultFolder.Location = new System.Drawing.Point(224, 142);
            this.bSetDefaultFolder.Name = "bSetDefaultFolder";
            this.bSetDefaultFolder.Size = new System.Drawing.Size(85, 23);
            this.bSetDefaultFolder.TabIndex = 12;
            this.bSetDefaultFolder.Text = "Set Default";
            this.bSetDefaultFolder.UseVisualStyleBackColor = true;
            this.bSetDefaultFolder.Click += new System.EventHandler(this.bSetDefaultFolder_Click);
            // 
            // bPickFolders
            // 
            this.bPickFolders.Location = new System.Drawing.Point(9, 142);
            this.bPickFolders.Name = "bPickFolders";
            this.bPickFolders.Size = new System.Drawing.Size(85, 23);
            this.bPickFolders.TabIndex = 11;
            this.bPickFolders.Text = "Pick Folders";
            this.bPickFolders.UseVisualStyleBackColor = true;
            this.bPickFolders.Click += new System.EventHandler(this.bPickFolders_Click);
            // 
            // cFolders
            // 
            this.cFolders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cFolders.FormattingEnabled = true;
            this.cFolders.Location = new System.Drawing.Point(9, 115);
            this.cFolders.Name = "cFolders";
            this.cFolders.Size = new System.Drawing.Size(300, 21);
            this.cFolders.TabIndex = 10;
            this.cFolders.SelectedIndexChanged += new System.EventHandler(this.cFolders_SelectedIndexChanged);
            // 
            // bSetDefault
            // 
            this.bSetDefault.Location = new System.Drawing.Point(223, 57);
            this.bSetDefault.Name = "bSetDefault";
            this.bSetDefault.Size = new System.Drawing.Size(85, 23);
            this.bSetDefault.TabIndex = 9;
            this.bSetDefault.Text = "Set Default";
            this.bSetDefault.UseVisualStyleBackColor = true;
            this.bSetDefault.Click += new System.EventHandler(this.bSetDefault_Click);
            // 
            // bRemoveAccount
            // 
            this.bRemoveAccount.Location = new System.Drawing.Point(100, 57);
            this.bRemoveAccount.Name = "bRemoveAccount";
            this.bRemoveAccount.Size = new System.Drawing.Size(85, 23);
            this.bRemoveAccount.TabIndex = 8;
            this.bRemoveAccount.Text = "Remove";
            this.bRemoveAccount.UseVisualStyleBackColor = true;
            this.bRemoveAccount.Click += new System.EventHandler(this.bRemoveAccount_Click);
            // 
            // bAddAccount
            // 
            this.bAddAccount.Location = new System.Drawing.Point(9, 57);
            this.bAddAccount.Name = "bAddAccount";
            this.bAddAccount.Size = new System.Drawing.Size(85, 23);
            this.bAddAccount.TabIndex = 5;
            this.bAddAccount.Text = "Add";
            this.bAddAccount.UseVisualStyleBackColor = true;
            this.bAddAccount.Click += new System.EventHandler(this.bAddAccount_Click);
            // 
            // cAccounts
            // 
            this.cAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cAccounts.FormattingEnabled = true;
            this.cAccounts.Location = new System.Drawing.Point(8, 21);
            this.cAccounts.Name = "cAccounts";
            this.cAccounts.Size = new System.Drawing.Size(300, 21);
            this.cAccounts.TabIndex = 4;
            this.cAccounts.SelectedIndexChanged += new System.EventHandler(this.cAccounts_SelectedIndexChanged);
            // 
            // lAbout
            // 
            this.lAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lAbout.AutoSize = true;
            this.lAbout.Location = new System.Drawing.Point(12, 337);
            this.lAbout.Name = "lAbout";
            this.lAbout.Size = new System.Drawing.Size(35, 13);
            this.lAbout.TabIndex = 9;
            this.lAbout.TabStop = true;
            this.lAbout.Text = "About";
            this.lAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lAbout_LinkClicked);
            // 
            // bDone
            // 
            this.bDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bDone.Location = new System.Drawing.Point(252, 332);
            this.bDone.Name = "bDone";
            this.bDone.Size = new System.Drawing.Size(75, 23);
            this.bDone.TabIndex = 10;
            this.bDone.Text = "Done";
            this.bDone.UseVisualStyleBackColor = true;
            this.bDone.Click += new System.EventHandler(this.bDone_Click);
            // 
            // cOpenByDefault
            // 
            this.cOpenByDefault.AutoSize = true;
            this.cOpenByDefault.Location = new System.Drawing.Point(10, 76);
            this.cOpenByDefault.Name = "cOpenByDefault";
            this.cOpenByDefault.Size = new System.Drawing.Size(176, 17);
            this.cOpenByDefault.TabIndex = 7;
            this.cOpenByDefault.Text = "Open links in browser by default";
            this.cOpenByDefault.UseVisualStyleBackColor = true;
            this.cOpenByDefault.CheckedChanged += new System.EventHandler(this.cOpenByDefault_CheckedChanged);
            // 
            // cCopyByDefault
            // 
            this.cCopyByDefault.AutoSize = true;
            this.cCopyByDefault.Location = new System.Drawing.Point(10, 99);
            this.cCopyByDefault.Name = "cCopyByDefault";
            this.cCopyByDefault.Size = new System.Drawing.Size(181, 17);
            this.cCopyByDefault.TabIndex = 8;
            this.cCopyByDefault.Text = "Copy links to clipboard by default";
            this.cCopyByDefault.UseVisualStyleBackColor = true;
            this.cCopyByDefault.CheckedChanged += new System.EventHandler(this.cCopyByDefault_CheckedChanged);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 364);
            this.Controls.Add(this.bDone);
            this.Controls.Add(this.lAbout);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options - upScreen";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nLenght)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bRemoveAccount;
        private System.Windows.Forms.Button bAddAccount;
        private System.Windows.Forms.ComboBox cAccounts;
        private System.Windows.Forms.NumericUpDown nLenght;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cFormat;
        private System.Windows.Forms.LinkLabel lAbout;
        private System.Windows.Forms.Button bDone;
        private System.Windows.Forms.Button bSetDefault;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bSetDefaultFolder;
        private System.Windows.Forms.Button bPickFolders;
        private System.Windows.Forms.ComboBox cFolders;
        private System.Windows.Forms.CheckBox cCopyByDefault;
        private System.Windows.Forms.CheckBox cOpenByDefault;
    }
}