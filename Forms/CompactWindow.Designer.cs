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
            this.byTheWindowTitleTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byTheProcessBinaryNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextBorderless = new System.Windows.Forms.ToolStripMenuItem();
            this.contextBorderlessOn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.hideThisProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workerTimer = new System.Windows.Forms.Timer(this.components);
            this.addSelectedItem = new System.Windows.Forms.Button();
            this.favoritesList = new System.Windows.Forms.ListBox();
            this.favoritesContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWindowSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.automaximizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustWindowBoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.removeMenusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.contextRemoveFromFavs = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.processLabel = new System.Windows.Forms.Label();
            this.favoritesLabel = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRunOnStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripGlobalHotkey = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMouseLock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.startMinimizedToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideBalloonTipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewFullProcessDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripReportBug = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSupportUs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.backWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.makeBorderedButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblUpdateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processContext.SuspendLayout();
            this.favoritesContext.SuspendLayout();
            this.trayIconContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // makeBorderlessButton
            // 
            this.makeBorderlessButton.Image = global::BorderlessGaming.Properties.Resources.borderless;
            resources.ApplyResources(this.makeBorderlessButton, "makeBorderlessButton");
            this.makeBorderlessButton.Name = "makeBorderlessButton";
            this.makeBorderlessButton.UseVisualStyleBackColor = true;
            this.makeBorderlessButton.Click += new System.EventHandler(this.MakeBorderlessClick);
            // 
            // processList
            // 
            this.processList.ContextMenuStrip = this.processContext;
            resources.ApplyResources(this.processList, "processList");
            this.processList.FormattingEnabled = true;
            this.processList.Name = "processList";
            this.processList.Sorted = true;
            // 
            // processContext
            // 
            this.processContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextAddToFavs,
            this.toolStripMenuItem1,
            this.contextBorderless,
            this.contextBorderlessOn,
            this.toolStripMenuItem8,
            this.hideThisProcessToolStripMenuItem});
            this.processContext.Name = "processContext";
            resources.ApplyResources(this.processContext, "processContext");
            this.processContext.Opening += new System.ComponentModel.CancelEventHandler(this.ProcessContextOpening);
            // 
            // contextAddToFavs
            // 
            this.contextAddToFavs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byTheWindowTitleTextToolStripMenuItem,
            this.byTheProcessBinaryNameToolStripMenuItem});
            this.contextAddToFavs.Name = "contextAddToFavs";
            resources.ApplyResources(this.contextAddToFavs, "contextAddToFavs");
            // 
            // byTheWindowTitleTextToolStripMenuItem
            // 
            this.byTheWindowTitleTextToolStripMenuItem.Name = "byTheWindowTitleTextToolStripMenuItem";
            resources.ApplyResources(this.byTheWindowTitleTextToolStripMenuItem, "byTheWindowTitleTextToolStripMenuItem");
            this.byTheWindowTitleTextToolStripMenuItem.Click += new System.EventHandler(this.byTheWindowTitleTextToolStripMenuItem_Click);
            // 
            // byTheProcessBinaryNameToolStripMenuItem
            // 
            this.byTheProcessBinaryNameToolStripMenuItem.Name = "byTheProcessBinaryNameToolStripMenuItem";
            resources.ApplyResources(this.byTheProcessBinaryNameToolStripMenuItem, "byTheProcessBinaryNameToolStripMenuItem");
            this.byTheProcessBinaryNameToolStripMenuItem.Click += new System.EventHandler(this.byTheProcessBinaryNameToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // contextBorderless
            // 
            this.contextBorderless.Name = "contextBorderless";
            resources.ApplyResources(this.contextBorderless, "contextBorderless");
            this.contextBorderless.Click += new System.EventHandler(this.MakeBorderlessClick);
            // 
            // contextBorderlessOn
            // 
            this.contextBorderlessOn.Name = "contextBorderlessOn";
            resources.ApplyResources(this.contextBorderlessOn, "contextBorderlessOn");
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            resources.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
            // 
            // hideThisProcessToolStripMenuItem
            // 
            this.hideThisProcessToolStripMenuItem.Name = "hideThisProcessToolStripMenuItem";
            resources.ApplyResources(this.hideThisProcessToolStripMenuItem, "hideThisProcessToolStripMenuItem");
            this.hideThisProcessToolStripMenuItem.Click += new System.EventHandler(this.hideThisProcessToolStripMenuItem_Click);
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
            this.addSelectedItem.Click += new System.EventHandler(this.addSelectedItem_Click);
            // 
            // favoritesList
            // 
            this.favoritesList.ContextMenuStrip = this.favoritesContext;
            resources.ApplyResources(this.favoritesList, "favoritesList");
            this.favoritesList.FormattingEnabled = true;
            this.favoritesList.Name = "favoritesList";
            this.favoritesList.Sorted = true;
            // 
            // favoritesContext
            // 
            this.favoritesContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenToolStripMenuItem,
            this.setWindowSizeToolStripMenuItem,
            this.toolStripMenuItem6,
            this.automaximizeToolStripMenuItem,
            this.adjustWindowBoundsToolStripMenuItem,
            this.toolStripMenuItem4,
            this.alwaysOnTopToolStripMenuItem,
            this.removeMenusToolStripMenuItem,
            this.toolStripMenuItem9,
            this.contextRemoveFromFavs});
            this.favoritesContext.Name = "favoritesContext";
            resources.ApplyResources(this.favoritesContext, "favoritesContext");
            this.favoritesContext.Opening += new System.ComponentModel.CancelEventHandler(this.FavoriteContextOpening);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.CheckOnClick = true;
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            resources.ApplyResources(this.fullScreenToolStripMenuItem, "fullScreenToolStripMenuItem");
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // setWindowSizeToolStripMenuItem
            // 
            this.setWindowSizeToolStripMenuItem.Name = "setWindowSizeToolStripMenuItem";
            resources.ApplyResources(this.setWindowSizeToolStripMenuItem, "setWindowSizeToolStripMenuItem");
            this.setWindowSizeToolStripMenuItem.Click += new System.EventHandler(this.setWindowSizeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            // 
            // automaximizeToolStripMenuItem
            // 
            this.automaximizeToolStripMenuItem.CheckOnClick = true;
            this.automaximizeToolStripMenuItem.Name = "automaximizeToolStripMenuItem";
            resources.ApplyResources(this.automaximizeToolStripMenuItem, "automaximizeToolStripMenuItem");
            this.automaximizeToolStripMenuItem.Click += new System.EventHandler(this.automaximizeToolStripMenuItem_Click);
            // 
            // adjustWindowBoundsToolStripMenuItem
            // 
            this.adjustWindowBoundsToolStripMenuItem.Name = "adjustWindowBoundsToolStripMenuItem";
            resources.ApplyResources(this.adjustWindowBoundsToolStripMenuItem, "adjustWindowBoundsToolStripMenuItem");
            this.adjustWindowBoundsToolStripMenuItem.Click += new System.EventHandler(this.adjustWindowBoundsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            // 
            // removeMenusToolStripMenuItem
            // 
            this.removeMenusToolStripMenuItem.CheckOnClick = true;
            this.removeMenusToolStripMenuItem.Name = "removeMenusToolStripMenuItem";
            resources.ApplyResources(this.removeMenusToolStripMenuItem, "removeMenusToolStripMenuItem");
            this.removeMenusToolStripMenuItem.Click += new System.EventHandler(this.removeMenusToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            resources.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
            // 
            // contextRemoveFromFavs
            // 
            this.contextRemoveFromFavs.Name = "contextRemoveFromFavs";
            resources.ApplyResources(this.contextRemoveFromFavs, "contextRemoveFromFavs");
            this.contextRemoveFromFavs.Click += new System.EventHandler(this.RemoveFavoriteClick);
            // 
            // button3
            // 
            this.button3.Image = global::BorderlessGaming.Properties.Resources.remove;
            resources.ApplyResources(this.button3, "button3");
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
            this.trayIcon.ContextMenuStrip = this.trayIconContextMenu;
            resources.ApplyResources(this.trayIcon, "trayIcon");
            this.trayIcon.DoubleClick += new System.EventHandler(this.TrayIconShow);
            // 
            // trayIconContextMenu
            // 
            this.trayIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem7,
            this.exitToolStripMenuItem});
            this.trayIconContextMenu.Name = "trayIconContextMenu";
            resources.ApplyResources(this.trayIconContextMenu, "trayIconContextMenu");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.TrayIconShow);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.TrayIconExit);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOptions,
            this.toolStripInfo});
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRunOnStartup,
            this.toolStripMenuItem3,
            this.toolStripGlobalHotkey,
            this.toolStripMouseLock,
            this.toolStripMenuItem5,
            this.startMinimizedToTrayToolStripMenuItem,
            this.closeToTrayToolStripMenuItem,
            this.hideBalloonTipsToolStripMenuItem,
            this.viewFullProcessDetailsToolStripMenuItem});
            this.toolStripOptions.Name = "toolStripOptions";
            resources.ApplyResources(this.toolStripOptions, "toolStripOptions");
            // 
            // toolStripRunOnStartup
            // 
            this.toolStripRunOnStartup.CheckOnClick = true;
            this.toolStripRunOnStartup.Name = "toolStripRunOnStartup";
            resources.ApplyResources(this.toolStripRunOnStartup, "toolStripRunOnStartup");
            this.toolStripRunOnStartup.CheckedChanged += new System.EventHandler(this.RunOnStartupChecked);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            // 
            // toolStripGlobalHotkey
            // 
            this.toolStripGlobalHotkey.CheckOnClick = true;
            this.toolStripGlobalHotkey.Name = "toolStripGlobalHotkey";
            resources.ApplyResources(this.toolStripGlobalHotkey, "toolStripGlobalHotkey");
            this.toolStripGlobalHotkey.CheckedChanged += new System.EventHandler(this.UseGlobalHotkeyChanged);
            // 
            // toolStripMouseLock
            // 
            this.toolStripMouseLock.CheckOnClick = true;
            this.toolStripMouseLock.Name = "toolStripMouseLock";
            resources.ApplyResources(this.toolStripMouseLock, "toolStripMouseLock");
            this.toolStripMouseLock.CheckedChanged += new System.EventHandler(this.UseMouseLockChanged);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            // 
            // startMinimizedToTrayToolStripMenuItem
            // 
            this.startMinimizedToTrayToolStripMenuItem.CheckOnClick = true;
            this.startMinimizedToTrayToolStripMenuItem.Name = "startMinimizedToTrayToolStripMenuItem";
            resources.ApplyResources(this.startMinimizedToTrayToolStripMenuItem, "startMinimizedToTrayToolStripMenuItem");
            this.startMinimizedToTrayToolStripMenuItem.CheckedChanged += new System.EventHandler(this.startMinimizedToTrayToolStripMenuItem_CheckedChanged);
            // 
            // closeToTrayToolStripMenuItem
            // 
            this.closeToTrayToolStripMenuItem.CheckOnClick = true;
            this.closeToTrayToolStripMenuItem.Name = "closeToTrayToolStripMenuItem";
            resources.ApplyResources(this.closeToTrayToolStripMenuItem, "closeToTrayToolStripMenuItem");
            this.closeToTrayToolStripMenuItem.CheckedChanged += new System.EventHandler(this.closeToTrayToolStripMenuItem_CheckedChanged);
            // 
            // hideBalloonTipsToolStripMenuItem
            // 
            this.hideBalloonTipsToolStripMenuItem.CheckOnClick = true;
            this.hideBalloonTipsToolStripMenuItem.Name = "hideBalloonTipsToolStripMenuItem";
            resources.ApplyResources(this.hideBalloonTipsToolStripMenuItem, "hideBalloonTipsToolStripMenuItem");
            this.hideBalloonTipsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.hideBalloonTipsToolStripMenuItem_CheckedChanged);
            // 
            // viewFullProcessDetailsToolStripMenuItem
            // 
            this.viewFullProcessDetailsToolStripMenuItem.CheckOnClick = true;
            this.viewFullProcessDetailsToolStripMenuItem.Name = "viewFullProcessDetailsToolStripMenuItem";
            resources.ApplyResources(this.viewFullProcessDetailsToolStripMenuItem, "viewFullProcessDetailsToolStripMenuItem");
            this.viewFullProcessDetailsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.viewFullProcessDetailsToolStripMenuItem_CheckedChanged);
            // 
            // toolStripInfo
            // 
            this.toolStripInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripReportBug,
            this.toolStripSupportUs,
            this.toolStripMenuItem2,
            this.toolStripAbout});
            this.toolStripInfo.Name = "toolStripInfo";
            resources.ApplyResources(this.toolStripInfo, "toolStripInfo");
            // 
            // toolStripReportBug
            // 
            this.toolStripReportBug.Name = "toolStripReportBug";
            resources.ApplyResources(this.toolStripReportBug, "toolStripReportBug");
            this.toolStripReportBug.Click += new System.EventHandler(this.ReportBugClick);
            // 
            // toolStripSupportUs
            // 
            this.toolStripSupportUs.Name = "toolStripSupportUs";
            resources.ApplyResources(this.toolStripSupportUs, "toolStripSupportUs");
            this.toolStripSupportUs.Click += new System.EventHandler(this.SupportUsClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // toolStripAbout
            // 
            this.toolStripAbout.Name = "toolStripAbout";
            resources.ApplyResources(this.toolStripAbout, "toolStripAbout");
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
            this.flowLayoutPanel1.Controls.Add(this.makeBorderedButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // makeBorderedButton
            // 
            this.makeBorderedButton.Image = global::BorderlessGaming.Properties.Resources.bordered;
            resources.ApplyResources(this.makeBorderedButton, "makeBorderedButton");
            this.makeBorderedButton.Name = "makeBorderedButton";
            this.makeBorderedButton.UseVisualStyleBackColor = true;
            this.makeBorderedButton.Click += new System.EventHandler(this.makeBorderedButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUpdateStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblUpdateStatus
            // 
            this.lblUpdateStatus.Name = "lblUpdateStatus";
            resources.ApplyResources(this.lblUpdateStatus, "lblUpdateStatus");
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            resources.ApplyResources(this.alwaysOnTopToolStripMenuItem, "alwaysOnTopToolStripMenuItem");
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // CompactWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "CompactWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CompactWindowFormClosing);
            this.Load += new System.EventHandler(this.CompactWindowLoad);
            this.Shown += new System.EventHandler(this.CompactWindowShown);
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
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem toolStripGlobalHotkey;
        private System.Windows.Forms.ToolStripMenuItem toolStripMouseLock;
        private System.Windows.Forms.ToolStripMenuItem byTheWindowTitleTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byTheProcessBinaryNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem adjustWindowBoundsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem automaximizeToolStripMenuItem;
        private System.Windows.Forms.Button makeBorderedButton;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem startMinimizedToTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideBalloonTipsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblUpdateStatus;
        private System.Windows.Forms.ToolStripMenuItem viewFullProcessDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWindowSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem hideThisProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeMenusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
    }
}