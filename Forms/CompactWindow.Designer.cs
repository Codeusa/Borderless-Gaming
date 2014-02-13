using System;
using Utilities;
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
            this.workerTimer = new System.Windows.Forms.Timer(this.components);
            this.addSelectedItem = new System.Windows.Forms.Button();
            this.favoritesList = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.processLabel = new System.Windows.Forms.Label();
            this.favoritesLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBug = new System.Windows.Forms.ToolStripMenuItem();
            this._startUpCheckBox = new System.Windows.Forms.CheckBox();
            this.trayIconContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // makeBorderlessButton
            // 
            this.makeBorderlessButton.Location = new System.Drawing.Point(223, 224);
            this.makeBorderlessButton.Name = "makeBorderlessButton";
            this.makeBorderlessButton.Size = new System.Drawing.Size(111, 36);
            this.makeBorderlessButton.TabIndex = 1;
            this.makeBorderlessButton.Text = "Make Borderless Once";
            this.makeBorderlessButton.UseVisualStyleBackColor = true;
            this.makeBorderlessButton.Click += new System.EventHandler(this.MakeBorderless);
            // 
            // selectedProcess
            // 
            this.selectedProcess.AutoSize = true;
            this.selectedProcess.Location = new System.Drawing.Point(225, 61);
            this.selectedProcess.Name = "selectedProcess";
            this.selectedProcess.Size = new System.Drawing.Size(107, 13);
            this.selectedProcess.TabIndex = 2;
            this.selectedProcess.Text = "No Process Selected";
            this.selectedProcess.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // processList
            // 
            this.processList.FormattingEnabled = true;
            this.processList.Location = new System.Drawing.Point(15, 61);
            this.processList.Name = "processList";
            this.processList.Size = new System.Drawing.Size(200, 199);
            this.processList.TabIndex = 0;
            this.processList.SelectedIndexChanged += new System.EventHandler(this.ProcessListSelectedIndexChanged);
            // 
            // workerTimer
            // 
            this.workerTimer.Interval = 3000;
            this.workerTimer.Tick += new System.EventHandler(this.workerTimer_Tick);
            // 
            // addSelectedItem
            // 
            this.addSelectedItem.Location = new System.Drawing.Point(223, 140);
            this.addSelectedItem.Name = "addSelectedItem";
            this.addSelectedItem.Size = new System.Drawing.Size(111, 36);
            this.addSelectedItem.TabIndex = 7;
            this.addSelectedItem.Text = "Add Process to Favorites";
            this.addSelectedItem.UseVisualStyleBackColor = true;
            this.addSelectedItem.Click += new System.EventHandler(this.SendToFavorites);
            // 
            // favoritesList
            // 
            this.favoritesList.FormattingEnabled = true;
            this.favoritesList.Location = new System.Drawing.Point(343, 61);
            this.favoritesList.Name = "favoritesList";
            this.favoritesList.Size = new System.Drawing.Size(200, 199);
            this.favoritesList.TabIndex = 10;
            this.favoritesList.SelectedIndexChanged += new System.EventHandler(this.FavoritesListSelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(223, 182);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 36);
            this.button3.TabIndex = 11;
            this.button3.Text = "Remove Selected Favorite";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.RemoveFavoriteButton);
            // 
            // processLabel
            // 
            this.processLabel.AutoSize = true;
            this.processLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processLabel.Location = new System.Drawing.Point(12, 36);
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new System.Drawing.Size(108, 18);
            this.processLabel.TabIndex = 12;
            this.processLabel.Text = "Process List:";
            // 
            // favoritesLabel
            // 
            this.favoritesLabel.AutoSize = true;
            this.favoritesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesLabel.Location = new System.Drawing.Point(340, 36);
            this.favoritesLabel.Name = "favoritesLabel";
            this.favoritesLabel.Size = new System.Drawing.Size(115, 18);
            this.favoritesLabel.TabIndex = 13;
            this.favoritesLabel.Text = "Favorites List:";
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
            this.toolStripMenuItem1,
            this.btnBug});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MinimumSize = new System.Drawing.Size(0, 31);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(557, 31);
            this.menuStrip.TabIndex = 16;
            this.menuStrip.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 27);
            this.miFile.Text = "File";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OpenAboutForm);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.ExitApplication);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(77, 27);
            this.toolStripMenuItem1.Text = "Support Us";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.OpenPaypal);
            // 
            // btnBug
            // 
            this.btnBug.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBug.Name = "btnBug";
            this.btnBug.Size = new System.Drawing.Size(78, 27);
            this.btnBug.Text = "Report Bug";
            this.btnBug.Click += new System.EventHandler(this.ReportBug);
            // 
            // _startUpCheckBox
            // 
            this._startUpCheckBox.AutoSize = true;
            this._startUpCheckBox.Location = new System.Drawing.Point(307, 8);
            this._startUpCheckBox.Name = "_startUpCheckBox";
            this._startUpCheckBox.Checked = AutoStart.CheckShortcut(Environment.SpecialFolder.Startup); 
            this._startUpCheckBox.Size = new System.Drawing.Size(100, 17);
            this._startUpCheckBox.TabIndex = 17;
            this._startUpCheckBox.Text = "Run On Startup";
            this._startUpCheckBox.UseVisualStyleBackColor = true;
            this._startUpCheckBox.CheckedChanged += new System.EventHandler(this._startUpCheckBox_CheckedChanged);
            // 
            // CompactWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 268);
            this.Controls.Add(this._startUpCheckBox);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.favoritesLabel);
            this.Controls.Add(this.processLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.favoritesList);
            this.Controls.Add(this.addSelectedItem);
            this.Controls.Add(this.selectedProcess);
            this.Controls.Add(this.makeBorderlessButton);
            this.Controls.Add(this.processList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(573, 307);
            this.MinimumSize = new System.Drawing.Size(573, 307);
            this.Name = "CompactWindow";
            this.Text = "Borderless Gaming";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.trayIconContextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button makeBorderlessButton;
        private System.Windows.Forms.Label selectedProcess;
        private System.Windows.Forms.ListBox processList;
        private System.Windows.Forms.Timer workerTimer;
        private System.Windows.Forms.Button addSelectedItem;
        private System.Windows.Forms.ListBox favoritesList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label processLabel;
        private System.Windows.Forms.Label favoritesLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem btnBug;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.CheckBox _startUpCheckBox;
    }
}