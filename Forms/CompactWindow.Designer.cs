using BorderlessGaming.Properties;
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
            this.processList = new System.Windows.Forms.ListBox();
            this.processContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextAddToFavs = new System.Windows.Forms.ToolStripMenuItem();
            this.contextBorderless = new System.Windows.Forms.ToolStripMenuItem();
            this.contextBorderlessOn = new System.Windows.Forms.ToolStripMenuItem();
            this.workerTimer = new System.Windows.Forms.Timer(this.components);
            this.addSelectedItem = new System.Windows.Forms.Button();
            this.favoritesList = new System.Windows.Forms.ListBox();
            this.favoritesContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextRemoveFromFavs = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.processLabel = new System.Windows.Forms.Label();
            this.favoritesLabel = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRunOnStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripReportBug = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSupportUs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.backWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.processContext.SuspendLayout();
            this.favoritesContext.SuspendLayout();
            this.trayIconContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // makeBorderlessButton
            // 
            resources.ApplyResources(this.makeBorderlessButton, "makeBorderlessButton");
            this.makeBorderlessButton.Image = global::BorderlessGaming.Properties.Resources.borderless;
            this.makeBorderlessButton.Name = "makeBorderlessButton";
            this.makeBorderlessButton.UseVisualStyleBackColor = true;
            this.makeBorderlessButton.Click += new System.EventHandler(this.MakeBorderlessClick);
            // 
            // processList
            // 
            resources.ApplyResources(this.processList, "processList");
            this.processList.ContextMenuStrip = this.processContext;
            this.processList.FormattingEnabled = true;
            this.processList.Name = "processList";
            this.processList.Sorted = true;
            // 
            // processContext
            // 
            resources.ApplyResources(this.processContext, "processContext");
            this.processContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextAddToFavs,
            this.contextBorderless,
            this.contextBorderlessOn});
            this.processContext.Name = "processContext";
            this.processContext.Opening += new System.ComponentModel.CancelEventHandler(this.ProcessContextOpening);
            // 
            // contextAddToFavs
            // 
            resources.ApplyResources(this.contextAddToFavs, "contextAddToFavs");
            this.contextAddToFavs.Name = "contextAddToFavs";
            this.contextAddToFavs.Click += new System.EventHandler(this.AddFavoriteClick);
            // 
            // contextBorderless
            // 
            resources.ApplyResources(this.contextBorderless, "contextBorderless");
            this.contextBorderless.Name = "contextBorderless";
            this.contextBorderless.Click += new System.EventHandler(this.MakeBorderlessClick);
            // 
            // contextBorderlessOn
            // 
            resources.ApplyResources(this.contextBorderlessOn, "contextBorderlessOn");
            this.contextBorderlessOn.Name = "contextBorderlessOn";
            // 
            // workerTimer
            // 
            this.workerTimer.Interval = 3000;
            this.workerTimer.Tick += new System.EventHandler(this.WorkerTimerTick);
            // 
            // addSelectedItem
            // 
            resources.ApplyResources(this.addSelectedItem, "addSelectedItem");
            this.addSelectedItem.Image = global::BorderlessGaming.Properties.Resources.add;
            this.addSelectedItem.Name = "addSelectedItem";
            this.addSelectedItem.UseVisualStyleBackColor = true;
            this.addSelectedItem.Click += new System.EventHandler(this.AddFavoriteClick);
            // 
            // favoritesList
            // 
            resources.ApplyResources(this.favoritesList, "favoritesList");
            this.favoritesList.ContextMenuStrip = this.favoritesContext;
            this.favoritesList.FormattingEnabled = true;
            this.favoritesList.Name = "favoritesList";
            this.favoritesList.Sorted = true;
            // 
            // favoritesContext
            // 
            resources.ApplyResources(this.favoritesContext, "favoritesContext");
            this.favoritesContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextRemoveFromFavs});
            this.favoritesContext.Name = "favoritesContext";
            this.favoritesContext.Opening += new System.ComponentModel.CancelEventHandler(this.FavoriteContextOpening);
            // 
            // contextRemoveFromFavs
            // 
            resources.ApplyResources(this.contextRemoveFromFavs, "contextRemoveFromFavs");
            this.contextRemoveFromFavs.Name = "contextRemoveFromFavs";
            this.contextRemoveFromFavs.Click += new System.EventHandler(this.RemoveFavoriteClick);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Image = global::BorderlessGaming.Properties.Resources.remove;
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.RemoveFavoriteClick);
            // 
            // processLabel
            // 
            resources.ApplyResources(this.processLabel, "processLabel");
            this.processLabel.Name = "processLabel";
            // 
            // favoritesLabel
            // 
            resources.ApplyResources(this.favoritesLabel, "favoritesLabel");
            this.favoritesLabel.Name = "favoritesLabel";
            // 
            // trayIcon
            // 
            resources.ApplyResources(this.trayIcon, "trayIcon");
            this.trayIcon.ContextMenuStrip = this.trayIconContextMenu;
            this.trayIcon.DoubleClick += new System.EventHandler(this.TrayIconOpen);
            // 
            // trayIconContextMenu
            // 
            resources.ApplyResources(this.trayIconContextMenu, "trayIconContextMenu");
            this.trayIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.trayIconContextMenu.Name = "trayIconContextMenu";
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.TrayIconOpen);
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.TrayIconExit);
            // 
            // menuStrip
            // 
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOptions,
            this.toolStripInfo});
            this.menuStrip.Name = "menuStrip";
            // 
            // toolStripOptions
            // 
            resources.ApplyResources(this.toolStripOptions, "toolStripOptions");
            this.toolStripOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRunOnStartup});
            this.toolStripOptions.Name = "toolStripOptions";
            // 
            // toolStripRunOnStartup
            // 
            resources.ApplyResources(this.toolStripRunOnStartup, "toolStripRunOnStartup");
            this.toolStripRunOnStartup.CheckOnClick = true;
            this.toolStripRunOnStartup.Name = "toolStripRunOnStartup";
            this.toolStripRunOnStartup.CheckedChanged += new System.EventHandler(this.RunOnStartupChecked);
            // 
            // toolStripInfo
            // 
            resources.ApplyResources(this.toolStripInfo, "toolStripInfo");
            this.toolStripInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripReportBug,
            this.toolStripSupportUs,
            this.toolStripAbout});
            this.toolStripInfo.Name = "toolStripInfo";
            // 
            // toolStripReportBug
            // 
            resources.ApplyResources(this.toolStripReportBug, "toolStripReportBug");
            this.toolStripReportBug.Name = "toolStripReportBug";
            this.toolStripReportBug.Click += new System.EventHandler(this.ReportBugClick);
            // 
            // toolStripSupportUs
            // 
            resources.ApplyResources(this.toolStripSupportUs, "toolStripSupportUs");
            this.toolStripSupportUs.Name = "toolStripSupportUs";
            this.toolStripSupportUs.Click += new System.EventHandler(this.SupportUsClick);
            // 
            // toolStripAbout
            // 
            resources.ApplyResources(this.toolStripAbout, "toolStripAbout");
            this.toolStripAbout.Name = "toolStripAbout";
            this.toolStripAbout.Click += new System.EventHandler(this.AboutClick);
            // 
            // backWorker
            // 
            this.backWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackWorkerProcess);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.processLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.favoritesLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.processList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.favoritesList, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.addSelectedItem);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.makeBorderlessButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // CompactWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "CompactWindow";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.CompactWindowLoad);
            this.Resize += new System.EventHandler(this.CompactWindowResize);
            this.processContext.ResumeLayout(false);
            this.favoritesContext.ResumeLayout(false);
            this.trayIconContextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button makeBorderlessButton;
        private System.Windows.Forms.ListBox processList;
        private System.Windows.Forms.Timer workerTimer;
        private System.Windows.Forms.Button addSelectedItem;
        private System.Windows.Forms.ListBox favoritesList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label processLabel;
        private System.Windows.Forms.Label favoritesLabel;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripOptions;
        private System.Windows.Forms.ToolStripMenuItem toolStripRunOnStartup;
        private System.Windows.Forms.ToolStripMenuItem toolStripInfo;
        private System.Windows.Forms.ToolStripMenuItem toolStripReportBug;
        private System.Windows.Forms.ToolStripMenuItem toolStripSupportUs;
        private System.Windows.Forms.ToolStripMenuItem toolStripAbout;
        private System.ComponentModel.BackgroundWorker backWorker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ContextMenuStrip processContext;
        private System.Windows.Forms.ToolStripMenuItem contextAddToFavs;
        private System.Windows.Forms.ToolStripMenuItem contextBorderless;
        private System.Windows.Forms.ToolStripMenuItem contextBorderlessOn;
        private System.Windows.Forms.ContextMenuStrip favoritesContext;
        private System.Windows.Forms.ToolStripMenuItem contextRemoveFromFavs;
    }
}