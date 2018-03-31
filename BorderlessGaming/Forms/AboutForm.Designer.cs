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
            this.versionLabel = new System.Windows.Forms.Label();
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
            this._copyrightLabel = new System.Windows.Forms.Label();
            this._steamGroupLabel = new System.Windows.Forms.Label();
            this._viewSteamGroupLinkLabel = new System.Windows.Forms.LinkLabel();
            this._madpewGithubGlobe = new System.Windows.Forms.PictureBox();
            this._madpewNametag = new System.Windows.Forms.Label();
            this._psouza4Nametag = new System.Windows.Forms.Label();
            this._psouza4GithubGlobe = new System.Windows.Forms.PictureBox();
            this._SecretOnlineNametag = new System.Windows.Forms.Label();
            this._SecretOnlineGithubGlobe = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this._codeusaSoftwareLogo = new System.Windows.Forms.PictureBox();
            this._stackOfPancakesNameTag = new System.Windows.Forms.Label();
            this._stackOfPancakesGithubGlobe = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._ownerGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dmxtGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._impulserGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._madpewGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._psouza4GithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._SecretOnlineGithubGlobe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._codeusaSoftwareLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._stackOfPancakesGithubGlobe)).BeginInit();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            // 
            // _gitHubLabel
            // 
            resources.ApplyResources(this._gitHubLabel, "_gitHubLabel");
            this._gitHubLabel.Name = "_gitHubLabel";
            // 
            // _blogLabel
            // 
            resources.ApplyResources(this._blogLabel, "_blogLabel");
            this._blogLabel.Name = "_blogLabel";
            // 
            // _viewGithubLinkLabel
            // 
            resources.ApplyResources(this._viewGithubLinkLabel, "_viewGithubLinkLabel");
            this._viewGithubLinkLabel.Name = "_viewGithubLinkLabel";
            this._viewGithubLinkLabel.TabStop = true;
            this._viewGithubLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenGithubRepo);
            // 
            // _viewBlogLinkLabel
            // 
            resources.ApplyResources(this._viewBlogLinkLabel, "_viewBlogLinkLabel");
            this._viewBlogLinkLabel.Name = "_viewBlogLinkLabel";
            this._viewBlogLinkLabel.TabStop = true;
            this._viewBlogLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenBlog);
            // 
            // _ownerLabel
            // 
            resources.ApplyResources(this._ownerLabel, "_ownerLabel");
            this._ownerLabel.Name = "_ownerLabel";
            // 
            // _ownerGithubGlobe
            // 
            resources.ApplyResources(this._ownerGithubGlobe, "_ownerGithubGlobe");
            this._ownerGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._ownerGithubGlobe.Name = "_ownerGithubGlobe";
            this._ownerGithubGlobe.TabStop = false;
            this._ownerGithubGlobe.Click += new System.EventHandler(this.OpenOwnerGithub);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.OpenOwnerSteam);
            // 
            // _ownerNameTag
            // 
            resources.ApplyResources(this._ownerNameTag, "_ownerNameTag");
            this._ownerNameTag.Name = "_ownerNameTag";
            // 
            // _committersLabel
            // 
            resources.ApplyResources(this._committersLabel, "_committersLabel");
            this._committersLabel.Name = "_committersLabel";
            // 
            // _dmxtGithubGlobe
            // 
            resources.ApplyResources(this._dmxtGithubGlobe, "_dmxtGithubGlobe");
            this._dmxtGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._dmxtGithubGlobe.Name = "_dmxtGithubGlobe";
            this._dmxtGithubGlobe.TabStop = false;
            this._dmxtGithubGlobe.Click += new System.EventHandler(this.OpenDmxtGithub);
            // 
            // _dmxtNameTag
            // 
            resources.ApplyResources(this._dmxtNameTag, "_dmxtNameTag");
            this._dmxtNameTag.Name = "_dmxtNameTag";
            // 
            // _impulserGithubGlobe
            // 
            resources.ApplyResources(this._impulserGithubGlobe, "_impulserGithubGlobe");
            this._impulserGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._impulserGithubGlobe.Name = "_impulserGithubGlobe";
            this._impulserGithubGlobe.TabStop = false;
            this._impulserGithubGlobe.Click += new System.EventHandler(this.OpenImpulserGithub);
            // 
            // _impulserNameTag
            // 
            resources.ApplyResources(this._impulserNameTag, "_impulserNameTag");
            this._impulserNameTag.Cursor = System.Windows.Forms.Cursors.Hand;
            this._impulserNameTag.Name = "_impulserNameTag";
            this._impulserNameTag.Click += new System.EventHandler(this._impulserNameTag_Click);
            // 
            // _copyrightLabel
            // 
            resources.ApplyResources(this._copyrightLabel, "_copyrightLabel");
            this._copyrightLabel.Name = "_copyrightLabel";
            // 
            // _steamGroupLabel
            // 
            resources.ApplyResources(this._steamGroupLabel, "_steamGroupLabel");
            this._steamGroupLabel.Name = "_steamGroupLabel";
            // 
            // _viewSteamGroupLinkLabel
            // 
            resources.ApplyResources(this._viewSteamGroupLinkLabel, "_viewSteamGroupLinkLabel");
            this._viewSteamGroupLinkLabel.Name = "_viewSteamGroupLinkLabel";
            this._viewSteamGroupLinkLabel.TabStop = true;
            this._viewSteamGroupLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenSteamGroup);
            // 
            // _madpewGithubGlobe
            // 
            resources.ApplyResources(this._madpewGithubGlobe, "_madpewGithubGlobe");
            this._madpewGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._madpewGithubGlobe.Name = "_madpewGithubGlobe";
            this._madpewGithubGlobe.TabStop = false;
            this._madpewGithubGlobe.Click += new System.EventHandler(this.OpenMadpewGithub);
            // 
            // _madpewNametag
            // 
            resources.ApplyResources(this._madpewNametag, "_madpewNametag");
            this._madpewNametag.Name = "_madpewNametag";
            // 
            // _psouza4Nametag
            // 
            resources.ApplyResources(this._psouza4Nametag, "_psouza4Nametag");
            this._psouza4Nametag.Name = "_psouza4Nametag";
            // 
            // _psouza4GithubGlobe
            // 
            resources.ApplyResources(this._psouza4GithubGlobe, "_psouza4GithubGlobe");
            this._psouza4GithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._psouza4GithubGlobe.Name = "_psouza4GithubGlobe";
            this._psouza4GithubGlobe.TabStop = false;
            this._psouza4GithubGlobe.Click += new System.EventHandler(this.OpenPsouza4Github);
            // 
            // _SecretOnlineNametag
            // 
            resources.ApplyResources(this._SecretOnlineNametag, "_SecretOnlineNametag");
            this._SecretOnlineNametag.Name = "_SecretOnlineNametag";
            // 
            // _SecretOnlineGithubGlobe
            // 
            resources.ApplyResources(this._SecretOnlineGithubGlobe, "_SecretOnlineGithubGlobe");
            this._SecretOnlineGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._SecretOnlineGithubGlobe.Name = "_SecretOnlineGithubGlobe";
            this._SecretOnlineGithubGlobe.TabStop = false;
            this._SecretOnlineGithubGlobe.Click += new System.EventHandler(this.OpenSecretOnlineGithub);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.OpenPsouza4Steam);
            // 
            // _codeusaSoftwareLogo
            // 
            resources.ApplyResources(this._codeusaSoftwareLogo, "_codeusaSoftwareLogo");
            this._codeusaSoftwareLogo.Name = "_codeusaSoftwareLogo";
            this._codeusaSoftwareLogo.TabStop = false;
            // 
            // _stackOfPancakesNameTag
            // 
            resources.ApplyResources(this._stackOfPancakesNameTag, "_stackOfPancakesNameTag");
            this._stackOfPancakesNameTag.Name = "_stackOfPancakesNameTag";
            // 
            // _stackOfPancakesGithubGlobe
            // 
            resources.ApplyResources(this._stackOfPancakesGithubGlobe, "_stackOfPancakesGithubGlobe");
            this._stackOfPancakesGithubGlobe.Cursor = System.Windows.Forms.Cursors.Hand;
            this._stackOfPancakesGithubGlobe.Name = "_stackOfPancakesGithubGlobe";
            this._stackOfPancakesGithubGlobe.TabStop = false;
            this._stackOfPancakesGithubGlobe.Click += new System.EventHandler(this.OpenStackOfPancakesGithub);
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._SecretOnlineNametag);
            this.Controls.Add(this._SecretOnlineGithubGlobe);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this._psouza4Nametag);
            this.Controls.Add(this._psouza4GithubGlobe);
            this.Controls.Add(this._madpewNametag);
            this.Controls.Add(this._madpewGithubGlobe);
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
            this.Controls.Add(this.versionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Load += new System.EventHandler(this.AboutFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this._ownerGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dmxtGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._impulserGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._madpewGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._psouza4GithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._SecretOnlineGithubGlobe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._codeusaSoftwareLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._stackOfPancakesGithubGlobe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label versionLabel;
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
        private System.Windows.Forms.Label _copyrightLabel;
        private System.Windows.Forms.Label _steamGroupLabel;
        private System.Windows.Forms.LinkLabel _viewSteamGroupLinkLabel;
        private System.Windows.Forms.PictureBox _madpewGithubGlobe;
        private System.Windows.Forms.Label _madpewNametag;
        private System.Windows.Forms.Label _psouza4Nametag;
        private System.Windows.Forms.PictureBox _psouza4GithubGlobe;
        private System.Windows.Forms.Label _SecretOnlineNametag;
        private System.Windows.Forms.PictureBox _SecretOnlineGithubGlobe;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox _codeusaSoftwareLogo;
        private System.Windows.Forms.Label _stackOfPancakesNameTag;
        private System.Windows.Forms.PictureBox _stackOfPancakesGithubGlobe;
    }
}