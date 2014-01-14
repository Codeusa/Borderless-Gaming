namespace BorderlessGaming.Forms
{
    partial class CompactWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompactWindow));
            this.makeBorderlessButton = new System.Windows.Forms.Button();
            this.selectedProcess = new System.Windows.Forms.Label();
            this.processList = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.workerTimer = new System.Windows.Forms.Timer(this.components);
            this.addSelectedItem = new System.Windows.Forms.Button();
            this.bugReportButton = new System.Windows.Forms.Button();
            this.favoritesList = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.processLabel = new System.Windows.Forms.Label();
            this.favoritesLabel = new System.Windows.Forms.Label();
            this.donateButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // makeBorderlessButton
            // 
            this.makeBorderlessButton.Location = new System.Drawing.Point(223, 21);
            this.makeBorderlessButton.Name = "makeBorderlessButton";
            this.makeBorderlessButton.Size = new System.Drawing.Size(111, 36);
            this.makeBorderlessButton.TabIndex = 1;
            this.makeBorderlessButton.Text = "Make Borderless";
            this.makeBorderlessButton.UseVisualStyleBackColor = true;
            this.makeBorderlessButton.Click += new System.EventHandler(this.MakeBorderless);
            // 
            // selectedProcess
            // 
            this.selectedProcess.AutoSize = true;
            this.selectedProcess.Location = new System.Drawing.Point(220, 4);
            this.selectedProcess.Name = "selectedProcess";
            this.selectedProcess.Size = new System.Drawing.Size(107, 13);
            this.selectedProcess.TabIndex = 2;
            this.selectedProcess.Text = "No Process Selected";
            this.selectedProcess.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // processList
            // 
            this.processList.FormattingEnabled = true;
            this.processList.Location = new System.Drawing.Point(-2, 21);
            this.processList.Name = "processList";
            this.processList.Size = new System.Drawing.Size(219, 199);
            this.processList.TabIndex = 0;
            this.processList.SelectedIndexChanged += new System.EventHandler(this.ProcessListSelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(223, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 31);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BlogButtonClick);
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(301, 143);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 31);
            this.button2.TabIndex = 5;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.GitHubButtonClick);
            // 
            // workerTimer
            // 
            this.workerTimer.Interval = 3000;
            this.workerTimer.Tick += new System.EventHandler(this.workerTimer_Tick);
            // 
            // addSelectedItem
            // 
            this.addSelectedItem.Location = new System.Drawing.Point(223, 63);
            this.addSelectedItem.Name = "addSelectedItem";
            this.addSelectedItem.Size = new System.Drawing.Size(111, 34);
            this.addSelectedItem.TabIndex = 7;
            this.addSelectedItem.Text = "Add Process to Favorites";
            this.addSelectedItem.UseVisualStyleBackColor = true;
            this.addSelectedItem.Click += new System.EventHandler(this.SendToFavorites);
            // 
            // bugReportButton
            // 
            this.bugReportButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bugReportButton.BackgroundImage")));
            this.bugReportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bugReportButton.Location = new System.Drawing.Point(262, 143);
            this.bugReportButton.Name = "bugReportButton";
            this.bugReportButton.Size = new System.Drawing.Size(33, 31);
            this.bugReportButton.TabIndex = 8;
            this.bugReportButton.UseVisualStyleBackColor = true;
            this.bugReportButton.Click += new System.EventHandler(this.BugReportClick);
            // 
            // favoritesList
            // 
            this.favoritesList.FormattingEnabled = true;
            this.favoritesList.Location = new System.Drawing.Point(341, 21);
            this.favoritesList.Name = "favoritesList";
            this.favoritesList.Size = new System.Drawing.Size(219, 199);
            this.favoritesList.TabIndex = 10;
            this.favoritesList.SelectedIndexChanged += new System.EventHandler(this.FavoritesListSelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(223, 103);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 34);
            this.button3.TabIndex = 11;
            this.button3.Text = "Remove Selected Favorite";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.RemoveFavoriteButton);
            // 
            // processLabel
            // 
            this.processLabel.AutoSize = true;
            this.processLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processLabel.Location = new System.Drawing.Point(52, 0);
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new System.Drawing.Size(108, 18);
            this.processLabel.TabIndex = 12;
            this.processLabel.Text = "Process List:";
            // 
            // favoritesLabel
            // 
            this.favoritesLabel.AutoSize = true;
            this.favoritesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesLabel.Location = new System.Drawing.Point(394, 0);
            this.favoritesLabel.Name = "favoritesLabel";
            this.favoritesLabel.Size = new System.Drawing.Size(115, 18);
            this.favoritesLabel.TabIndex = 13;
            this.favoritesLabel.Text = "Favorites List:";
            // 
            // donateButton
            // 
            this.donateButton.Location = new System.Drawing.Point(223, 180);
            this.donateButton.Name = "donateButton";
            this.donateButton.Size = new System.Drawing.Size(111, 31);
            this.donateButton.TabIndex = 14;
            this.donateButton.Text = "Buy me Dr.Pepper";
            this.donateButton.UseVisualStyleBackColor = true;
            this.donateButton.Click += new System.EventHandler(this.SupportButtonClick);
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
            // CompactWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 218);
            this.Controls.Add(this.donateButton);
            this.Controls.Add(this.favoritesLabel);
            this.Controls.Add(this.processLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.favoritesList);
            this.Controls.Add(this.bugReportButton);
            this.Controls.Add(this.addSelectedItem);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.selectedProcess);
            this.Controls.Add(this.makeBorderlessButton);
            this.Controls.Add(this.processList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(573, 257);
            this.MinimumSize = new System.Drawing.Size(573, 257);
            this.Name = "CompactWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Borderless Gaming";
            this.trayIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button makeBorderlessButton;
        private System.Windows.Forms.Label selectedProcess;
        private System.Windows.Forms.ListBox processList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer workerTimer;
        private System.Windows.Forms.Button addSelectedItem;
        private System.Windows.Forms.Button bugReportButton;
        private System.Windows.Forms.ListBox favoritesList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label processLabel;
        private System.Windows.Forms.Label favoritesLabel;
        private System.Windows.Forms.Button donateButton;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}