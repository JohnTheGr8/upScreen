namespace upScreen
{
    partial class newversion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(newversion));
            this.label6 = new System.Windows.Forms.Label();
            this.lNewVersion = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.bLearnMore = new System.Windows.Forms.Button();
            this.bDownload = new System.Windows.Forms.Button();
            this.lCurrentVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(235, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Do you want to download the new version now?";
            // 
            // lNewVersion
            // 
            this.lNewVersion.AutoSize = true;
            this.lNewVersion.Location = new System.Drawing.Point(167, 50);
            this.lNewVersion.Name = "lNewVersion";
            this.lNewVersion.Size = new System.Drawing.Size(34, 13);
            this.lNewVersion.TabIndex = 16;
            this.lNewVersion.Text = "Z.Z.Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "New Version:";
            // 
            // bClose
            // 
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bClose.Location = new System.Drawing.Point(167, 104);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(120, 23);
            this.bClose.TabIndex = 14;
            this.bClose.Text = "Remind me next time";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bLearnMore
            // 
            this.bLearnMore.Location = new System.Drawing.Point(86, 104);
            this.bLearnMore.Name = "bLearnMore";
            this.bLearnMore.Size = new System.Drawing.Size(75, 23);
            this.bLearnMore.TabIndex = 13;
            this.bLearnMore.Text = "Learn More";
            this.bLearnMore.UseVisualStyleBackColor = true;
            this.bLearnMore.Click += new System.EventHandler(this.bLearnMore_Click);
            // 
            // bDownload
            // 
            this.bDownload.Location = new System.Drawing.Point(5, 104);
            this.bDownload.Name = "bDownload";
            this.bDownload.Size = new System.Drawing.Size(75, 23);
            this.bDownload.TabIndex = 12;
            this.bDownload.Text = "Download";
            this.bDownload.UseVisualStyleBackColor = true;
            this.bDownload.Click += new System.EventHandler(this.bDownload_Click);
            // 
            // lCurrentVersion
            // 
            this.lCurrentVersion.AutoSize = true;
            this.lCurrentVersion.Location = new System.Drawing.Point(167, 28);
            this.lCurrentVersion.Name = "lCurrentVersion";
            this.lCurrentVersion.Size = new System.Drawing.Size(34, 13);
            this.lCurrentVersion.TabIndex = 11;
            this.lCurrentVersion.Text = "X.X.X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Current Version:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "New version of upScreen is available";
            // 
            // newversion
            // 
            this.AcceptButton = this.bDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bClose;
            this.ClientSize = new System.Drawing.Size(293, 132);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lNewVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bLearnMore);
            this.Controls.Add(this.bDownload);
            this.Controls.Add(this.lCurrentVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "newversion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "upScreen | Update Available";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.newversion_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lNewVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bLearnMore;
        private System.Windows.Forms.Button bDownload;
        private System.Windows.Forms.Label lCurrentVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}