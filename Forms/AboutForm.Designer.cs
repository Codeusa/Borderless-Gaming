namespace BorderlessGaming.Forms
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this._aboutTitleLabel = new System.Windows.Forms.Label();
            this._gitHubLabel = new System.Windows.Forms.Label();
            this._blogLabel = new System.Windows.Forms.Label();
            this._viewGithubLinkLabel = new System.Windows.Forms.LinkLabel();
            this._viewBlogLinkLabel = new System.Windows.Forms.LinkLabel();
            this._ownerLabel = new System.Windows.Forms.Label();
            this._ownerGithubGlobe = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._ownerNameTag = new System.Windows.Forms.Label();
            this._committersLabel = new System.Windows.Forms.Label();
            this._dmxtGithubGlobe = new System.Windows.Forms.PictureBox();
            this._dmxtNameTag = new System.Windows.Forms.Label();
            this._impulserGithubGlobe = new System.Windows.Forms.PictureBox();
            this._impulserNameTag = new System.Windows.Forms.Label();
            this._stackOfPancakesGithubGlobe = new System.Windows.Forms.PictureBox();
            this._stackOfPancakesNameTag = new System.Windows.Forms.Label();
            this._copyrightLabel = new System.Windows.Forms.Label();
            this._steamGroupLabel = new System.Windows.Forms.Label();
            this._viewSteamGroupLinkLabel = new System.Windows.Forms.LinkLabel();
            this._codeusaSoftwareLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._ownerGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dmxtGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._impulserGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._stackOfPancakesGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._codeusaSoftwareLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // _aboutTitleLabel
            // 
            this._aboutTitleLabel.AutoSize = true;
            this._aboutTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this._aboutTitleLabel.Location = new System.Drawing.Point(13, 13);
            this._aboutTitleLabel.Name = "_aboutTitleLabel";
            this._aboutTitleLabel.Size = new System.Drawing.Size(171, 24);
            this._aboutTitleLabel.TabIndex = 0;
            this._aboutTitleLabel.Text = "Borderless Gaming";
            // 
            // _gitHubLabel
            // 
            this._gitHubLabel.AutoSize = true;
            this._gitHubLabel.Location = new System.Drawing.Point(14, 52);
            this._gitHubLabel.Name = "_gitHubLabel";
            this._gitHubLabel.Size = new System.Drawing.Size(41, 13);
            this._gitHubLabel.TabIndex = 1;
            this._gitHubLabel.Text = "Github:";
            // 
            // _blogLabel
            // 
            this._blogLabel.AutoSize = true;
            this._blogLabel.Location = new System.Drawing.Point(14, 68);
            this._blogLabel.Name = "_blogLabel";
            this._blogLabel.Size = new System.Drawing.Size(31, 13);
            this._blogLabel.TabIndex = 1;
            this._blogLabel.Text = "Blog:";
            // 
            // _viewGithubLinkLabel
            // 
            this._viewGithubLinkLabel.AutoSize = true;
            this._viewGithubLinkLabel.Location = new System.Drawing.Point(54, 52);
            this._viewGithubLinkLabel.Name = "_viewGithubLinkLabel";
            this._viewGithubLinkLabel.Size = new System.Drawing.Size(235, 13);
            this._viewGithubLinkLabel.TabIndex = 2;
            this._viewGithubLinkLabel.TabStop = true;
            this._viewGithubLinkLabel.Text = "https://github.com/Codeusa/Borderless-Gaming";
            this._viewGithubLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenGithubRepo);
            // 
            // _viewBlogLinkLabel
            // 
            this._viewBlogLinkLabel.AutoSize = true;
            this._viewBlogLinkLabel.Location = new System.Drawing.Point(44, 68);
            this._viewBlogLinkLabel.Name = "_viewBlogLinkLabel";
            this._viewBlogLinkLabel.Size = new System.Drawing.Size(140, 13);
            this._viewBlogLinkLabel.TabIndex = 3;
            this._viewBlogLinkLabel.TabStop = true;
            this._viewBlogLinkLabel.Text = "http://andrew.codeusa.net/";
            this._viewBlogLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenBlog);
            // 
            // _ownerLabel
            // 
            this._ownerLabel.AutoSize = true;
            this._ownerLabel.Location = new System.Drawing.Point(15, 120);
            this._ownerLabel.Name = "_ownerLabel";
            this._ownerLabel.Size = new System.Drawing.Size(41, 13);
            this._ownerLabel.TabIndex = 4;
            this._ownerLabel.Text = "Owner:";
            // 
            // _ownerGithubGlobe
            // 
            this._ownerGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._ownerGithubGlobe.Image = ((System.Drawing.Image)(resources.GetObject("_ownerGithubGlobe.Image")));
            this._ownerGithubGlobe.Location = new System.Drawing.Point(18, 137);
            this._ownerGithubGlobe.Name = "_ownerGithubGlobe";
            this._ownerGithubGlobe.Size = new System.Drawing.Size(16, 18);
            this._ownerGithubGlobe.TabIndex = 5;
            this._ownerGithubGlobe.TabStop = false;
            this._ownerGithubGlobe.Click += new System.EventHandler(this.OpenOwnerGithub);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(41, 138);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 16);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.OpenOwnerSteam);
            // 
            // _ownerNameTag
            // 
            this._ownerNameTag.AutoSize = true;
            this._ownerNameTag.Location = new System.Drawing.Point(63, 140);
            this._ownerNameTag.Name = "_ownerNameTag";
            this._ownerNameTag.Size = new System.Drawing.Size(94, 13);
            this._ownerNameTag.TabIndex = 7;
            this._ownerNameTag.Text = "Codeusa - Andrew";
            // 
            // _committersLabel
            // 
            this._committersLabel.AutoSize = true;
            this._committersLabel.Location = new System.Drawing.Point(15, 165);
            this._committersLabel.Name = "_committersLabel";
            this._committersLabel.Size = new System.Drawing.Size(61, 13);
            this._committersLabel.TabIndex = 4;
            this._committersLabel.Text = "Committers:";
            // 
            // _dmxtGithubGlobe
            // 
            this._dmxtGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._dmxtGithubGlobe.Image = ((System.Drawing.Image)(resources.GetObject("_dmxtGithubGlobe.Image")));
            this._dmxtGithubGlobe.Location = new System.Drawing.Point(18, 182);
            this._dmxtGithubGlobe.Name = "_dmxtGithubGlobe";
            this._dmxtGithubGlobe.Size = new System.Drawing.Size(16, 18);
            this._dmxtGithubGlobe.TabIndex = 5;
            this._dmxtGithubGlobe.TabStop = false;
            this._dmxtGithubGlobe.Click += new System.EventHandler(this.OpenDmxtGithub);
            // 
            // _dmxtNameTag
            // 
            this._dmxtNameTag.AutoSize = true;
            this._dmxtNameTag.Location = new System.Drawing.Point(40, 185);
            this._dmxtNameTag.Name = "_dmxtNameTag";
            this._dmxtNameTag.Size = new System.Drawing.Size(64, 13);
            this._dmxtNameTag.TabIndex = 7;
            this._dmxtNameTag.Text = "dmxt - Dana";
            // 
            // _impulserGithubGlobe
            // 
            this._impulserGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._impulserGithubGlobe.Image = ((System.Drawing.Image)(resources.GetObject("_impulserGithubGlobe.Image")));
            this._impulserGithubGlobe.Location = new System.Drawing.Point(18, 200);
            this._impulserGithubGlobe.Name = "_impulserGithubGlobe";
            this._impulserGithubGlobe.Size = new System.Drawing.Size(16, 18);
            this._impulserGithubGlobe.TabIndex = 5;
            this._impulserGithubGlobe.TabStop = false;
            this._impulserGithubGlobe.Click += new System.EventHandler(this.OpenImpulserGithub);
            // 
            // _impulserNameTag
            // 
            this._impulserNameTag.AutoSize = true;
            this._impulserNameTag.Location = new System.Drawing.Point(40, 203);
            this._impulserNameTag.Name = "_impulserNameTag";
            this._impulserNameTag.Size = new System.Drawing.Size(75, 13);
            this._impulserNameTag.TabIndex = 7;
            this._impulserNameTag.Text = "Impulser - Alex";
            // 
            // _stackOfPancakesGithubGlobe
            // 
            this._stackOfPancakesGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._stackOfPancakesGithubGlobe.Image = ((System.Drawing.Image)(resources.GetObject("_stackOfPancakesGithubGlobe.Image")));
            this._stackOfPancakesGithubGlobe.Location = new System.Drawing.Point(18, 218);
            this._stackOfPancakesGithubGlobe.Name = "_stackOfPancakesGithubGlobe";
            this._stackOfPancakesGithubGlobe.Size = new System.Drawing.Size(16, 18);
            this._stackOfPancakesGithubGlobe.TabIndex = 5;
            this._stackOfPancakesGithubGlobe.TabStop = false;
            this._stackOfPancakesGithubGlobe.Click += new System.EventHandler(this.OpenStackOfPancakesGithub);
            // 
            // _stackOfPancakesNameTag
            // 
            this._stackOfPancakesNameTag.AutoSize = true;
            this._stackOfPancakesNameTag.Location = new System.Drawing.Point(40, 221);
            this._stackOfPancakesNameTag.Name = "_stackOfPancakesNameTag";
            this._stackOfPancakesNameTag.Size = new System.Drawing.Size(98, 13);
            this._stackOfPancakesNameTag.TabIndex = 7;
            this._stackOfPancakesNameTag.Text = "Stack-of-Pancakes";
            // 
            // _copyrightLabel
            // 
            this._copyrightLabel.AutoSize = true;
            this._copyrightLabel.Location = new System.Drawing.Point(14, 256);
            this._copyrightLabel.Name = "_copyrightLabel";
            this._copyrightLabel.Size = new System.Drawing.Size(180, 13);
            this._copyrightLabel.TabIndex = 8;
            this._copyrightLabel.Text = "Copyright © 2014 Codeusa Software";
            // 
            // _steamGroupLabel
            // 
            this._steamGroupLabel.AutoSize = true;
            this._steamGroupLabel.Location = new System.Drawing.Point(14, 86);
            this._steamGroupLabel.Name = "_steamGroupLabel";
            this._steamGroupLabel.Size = new System.Drawing.Size(70, 13);
            this._steamGroupLabel.TabIndex = 1;
            this._steamGroupLabel.Text = "Steam group:";
            // 
            // _viewSteamGroupLinkLabel
            // 
            this._viewSteamGroupLinkLabel.AutoSize = true;
            this._viewSteamGroupLinkLabel.Location = new System.Drawing.Point(82, 86);
            this._viewSteamGroupLinkLabel.Name = "_viewSteamGroupLinkLabel";
            this._viewSteamGroupLinkLabel.Size = new System.Drawing.Size(266, 13);
            this._viewSteamGroupLinkLabel.TabIndex = 3;
            this._viewSteamGroupLinkLabel.TabStop = true;
            this._viewSteamGroupLinkLabel.Text = "http://steamcommunity.com/groups/borderless-gaming";
            this._viewSteamGroupLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenSteamGroup);
            // 
            // _codeusaSoftwareLogo
            // 
            this._codeusaSoftwareLogo.Image = ((System.Drawing.Image)(resources.GetObject("_codeusaSoftwareLogo.Image")));
            this._codeusaSoftwareLogo.Location = new System.Drawing.Point(285, 207);
            this._codeusaSoftwareLogo.Name = "_codeusaSoftwareLogo";
            this._codeusaSoftwareLogo.Size = new System.Drawing.Size(66, 65);
            this._codeusaSoftwareLogo.TabIndex = 9;
            this._codeusaSoftwareLogo.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 283);
            this.Controls.Add(this._codeusaSoftwareLogo);
            this.Controls.Add(this._copyrightLabel);
            this.Controls.Add(this._stackOfPancakesNameTag);
            this.Controls.Add(this._impulserNameTag);
            this.Controls.Add(this._stackOfPancakesGithubGlobe);
            this.Controls.Add(this._dmxtNameTag);
            this.Controls.Add(this._impulserGithubGlobe);
            this.Controls.Add(this._ownerNameTag);
            this.Controls.Add(this._dmxtGithubGlobe);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this._committersLabel);
            this.Controls.Add(this._ownerGithubGlobe);
            this.Controls.Add(this._ownerLabel);
            this.Controls.Add(this._viewSteamGroupLinkLabel);
            this.Controls.Add(this._viewBlogLinkLabel);
            this.Controls.Add(this._viewGithubLinkLabel);
            this.Controls.Add(this._steamGroupLabel);
            this.Controls.Add(this._blogLabel);
            this.Controls.Add(this._gitHubLabel);
            this.Controls.Add(this._aboutTitleLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(373, 322);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(373, 322);
            this.Name = "AboutForm";
            this.Text = "Borderless Gaming 5.2";
            ((System.ComponentModel.ISupportInitialize)(this._ownerGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dmxtGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._impulserGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._stackOfPancakesGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._codeusaSoftwareLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _aboutTitleLabel;
        private System.Windows.Forms.Label _gitHubLabel;
        private System.Windows.Forms.Label _blogLabel;
        private System.Windows.Forms.LinkLabel _viewGithubLinkLabel;
        private System.Windows.Forms.LinkLabel _viewBlogLinkLabel;
        private System.Windows.Forms.Label _ownerLabel;
        private System.Windows.Forms.PictureBox _ownerGithubGlobe;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label _ownerNameTag;
        private System.Windows.Forms.Label _committersLabel;
        private System.Windows.Forms.PictureBox _dmxtGithubGlobe;
        private System.Windows.Forms.Label _dmxtNameTag;
        private System.Windows.Forms.PictureBox _impulserGithubGlobe;
        private System.Windows.Forms.Label _impulserNameTag;
        private System.Windows.Forms.PictureBox _stackOfPancakesGithubGlobe;
        private System.Windows.Forms.Label _stackOfPancakesNameTag;
        private System.Windows.Forms.Label _copyrightLabel;
        private System.Windows.Forms.Label _steamGroupLabel;
        private System.Windows.Forms.LinkLabel _viewSteamGroupLinkLabel;
        private System.Windows.Forms.PictureBox _codeusaSoftwareLogo;
    }
}