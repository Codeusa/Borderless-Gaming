namespace BorderlessGaming.Forms
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.workerTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGitHub = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBug = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBlog = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.ddListTargetsBy = new System.Windows.Forms.ToolStripDropDownButton();
            this.processNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowTitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblWindowWatch = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabActive = new System.Windows.Forms.TabPage();
            this.lstAttributes = new System.Windows.Forms.ListBox();
            this.processLabel = new System.Windows.Forms.Label();
            this.makeBorderlessButton = new System.Windows.Forms.Button();
            this.processList = new System.Windows.Forms.ListBox();
            this.tabFave = new System.Windows.Forms.TabPage();
            this.favoritesLabel = new System.Windows.Forms.Label();
            this.favoritesList = new System.Windows.Forms.ListBox();
            this.trayIconContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabActive.SuspendLayout();
            this.tabFave.SuspendLayout();
            this.SuspendLayout();
            // 
            // workerTimer
            // 
            this.workerTimer.Interval = 3000;
            this.workerTimer.Tick += new System.EventHandler(this.workerTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.trayIconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Borderless Gaming";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.TrayIconOpen);
            // 
            // trayIconContextMenu
            // 
            this.trayIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.trayIconContextMenu.Name = "trayIconContextMenu";
            this.trayIconContextMenu.Size = new System.Drawing.Size(104, 48);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.TrayIconOpen);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.TrayIconExit);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.btnGitHub,
            this.btnBug,
            this.btnBlog});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MinimumSize = new System.Drawing.Size(0, 31);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(514, 31);
            this.menuStrip.TabIndex = 15;
            this.menuStrip.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 27);
            this.miFile.Text = "File";
            // 
            // btnGitHub
            // 
            this.btnGitHub.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnGitHub.Name = "btnGitHub";
            this.btnGitHub.Size = new System.Drawing.Size(85, 27);
            this.btnGitHub.Text = "View GitHub";
            this.btnGitHub.Click += new System.EventHandler(this.GitHubButtonClick);
            // 
            // btnBug
            // 
            this.btnBug.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBug.Name = "btnBug";
            this.btnBug.Size = new System.Drawing.Size(78, 27);
            this.btnBug.Text = "Report Bug";
            this.btnBug.Click += new System.EventHandler(this.BugReportClick);
            // 
            // btnBlog
            // 
            this.btnBlog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBlog.Name = "btnBlog";
            this.btnBlog.Size = new System.Drawing.Size(71, 27);
            this.btnBlog.Text = "View Blog";
            this.btnBlog.Click += new System.EventHandler(this.BlogButtonClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddListTargetsBy,
            this.lblWindowWatch});
            this.statusStrip.Location = new System.Drawing.Point(0, 469);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(514, 22);
            this.statusStrip.TabIndex = 16;
            this.statusStrip.Text = "statusStrip1";
            // 
            // ddListTargetsBy
            // 
            this.ddListTargetsBy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddListTargetsBy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processNamesToolStripMenuItem,
            this.windowTitlesToolStripMenuItem});
            this.ddListTargetsBy.Image = ((System.Drawing.Image)(resources.GetObject("ddListTargetsBy.Image")));
            this.ddListTargetsBy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddListTargetsBy.Name = "ddListTargetsBy";
            this.ddListTargetsBy.Size = new System.Drawing.Size(96, 20);
            this.ddListTargetsBy.Text = "List Targets By";
            // 
            // processNamesToolStripMenuItem
            // 
            this.processNamesToolStripMenuItem.Name = "processNamesToolStripMenuItem";
            this.processNamesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.processNamesToolStripMenuItem.Text = "Process Names";
            // 
            // windowTitlesToolStripMenuItem
            // 
            this.windowTitlesToolStripMenuItem.Name = "windowTitlesToolStripMenuItem";
            this.windowTitlesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.windowTitlesToolStripMenuItem.Text = "Window Titles";
            // 
            // lblWindowWatch
            // 
            this.lblWindowWatch.Name = "lblWindowWatch";
            this.lblWindowWatch.Size = new System.Drawing.Size(119, 17);
            this.lblWindowWatch.Text = "Watching 0 Windows";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabActive);
            this.tabControl1.Controls.Add(this.tabFave);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(514, 438);
            this.tabControl1.TabIndex = 18;
            // 
            // tabActive
            // 
            this.tabActive.Controls.Add(this.lstAttributes);
            this.tabActive.Controls.Add(this.processLabel);
            this.tabActive.Controls.Add(this.makeBorderlessButton);
            this.tabActive.Controls.Add(this.processList);
            this.tabActive.Location = new System.Drawing.Point(4, 22);
            this.tabActive.Name = "tabActive";
            this.tabActive.Padding = new System.Windows.Forms.Padding(3);
            this.tabActive.Size = new System.Drawing.Size(506, 412);
            this.tabActive.TabIndex = 0;
            this.tabActive.Text = "Active";
            this.tabActive.UseVisualStyleBackColor = true;
            // 
            // lstAttributes
            // 
            this.lstAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAttributes.FormattingEnabled = true;
            this.lstAttributes.Location = new System.Drawing.Point(345, 27);
            this.lstAttributes.Name = "lstAttributes";
            this.lstAttributes.Size = new System.Drawing.Size(155, 212);
            this.lstAttributes.TabIndex = 22;
            // 
            // processLabel
            // 
            this.processLabel.AutoSize = true;
            this.processLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processLabel.Location = new System.Drawing.Point(6, 6);
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new System.Drawing.Size(108, 18);
            this.processLabel.TabIndex = 21;
            this.processLabel.Text = "Process List:";
            // 
            // makeBorderlessButton
            // 
            this.makeBorderlessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.makeBorderlessButton.Location = new System.Drawing.Point(345, 245);
            this.makeBorderlessButton.Name = "makeBorderlessButton";
            this.makeBorderlessButton.Size = new System.Drawing.Size(155, 23);
            this.makeBorderlessButton.TabIndex = 19;
            this.makeBorderlessButton.Text = "Make Borderless";
            this.makeBorderlessButton.UseVisualStyleBackColor = true;
            // 
            // processList
            // 
            this.processList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.processList.FormattingEnabled = true;
            this.processList.Location = new System.Drawing.Point(6, 27);
            this.processList.Name = "processList";
            this.processList.Size = new System.Drawing.Size(333, 368);
            this.processList.TabIndex = 18;
            // 
            // tabFave
            // 
            this.tabFave.Controls.Add(this.favoritesLabel);
            this.tabFave.Controls.Add(this.favoritesList);
            this.tabFave.Location = new System.Drawing.Point(4, 22);
            this.tabFave.Name = "tabFave";
            this.tabFave.Padding = new System.Windows.Forms.Padding(3);
            this.tabFave.Size = new System.Drawing.Size(506, 412);
            this.tabFave.TabIndex = 1;
            this.tabFave.Text = "Favorites";
            this.tabFave.UseVisualStyleBackColor = true;
            // 
            // favoritesLabel
            // 
            this.favoritesLabel.AutoSize = true;
            this.favoritesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesLabel.Location = new System.Drawing.Point(6, 6);
            this.favoritesLabel.Name = "favoritesLabel";
            this.favoritesLabel.Size = new System.Drawing.Size(115, 18);
            this.favoritesLabel.TabIndex = 15;
            this.favoritesLabel.Text = "Favorites List:";
            // 
            // favoritesList
            // 
            this.favoritesList.FormattingEnabled = true;
            this.favoritesList.Location = new System.Drawing.Point(6, 27);
            this.favoritesList.Name = "favoritesList";
            this.favoritesList.Size = new System.Drawing.Size(403, 368);
            this.favoritesList.TabIndex = 14;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 491);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(530, 530);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.trayIconContextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabActive.ResumeLayout(false);
            this.tabActive.PerformLayout();
            this.tabFave.ResumeLayout(false);
            this.tabFave.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer workerTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem btnBlog;
        private System.Windows.Forms.ToolStripMenuItem btnBug;
        private System.Windows.Forms.ToolStripMenuItem btnGitHub;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripDropDownButton ddListTargetsBy;
        private System.Windows.Forms.ToolStripMenuItem processNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowTitlesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblWindowWatch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabActive;
        private System.Windows.Forms.ListBox lstAttributes;
        private System.Windows.Forms.Label processLabel;
        private System.Windows.Forms.Button makeBorderlessButton;
        private System.Windows.Forms.ListBox processList;
        private System.Windows.Forms.TabPage tabFave;
        private System.Windows.Forms.Label favoritesLabel;
        private System.Windows.Forms.ListBox favoritesList;
    }
}

