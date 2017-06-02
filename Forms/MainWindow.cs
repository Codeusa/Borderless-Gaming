using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BorderlessGaming.Common;
using BorderlessGaming.Properties;
using BorderlessGaming.Utilities;
using BorderlessGaming.WindowsAPI;
using System.Threading.Tasks;

namespace BorderlessGaming.Forms
{
    public partial class MainWindow : Form
    {
        #region Local data

        /// <summary>
        ///     The Borderless Toggle hotKey
        /// </summary>
        private const int MakeBorderless_HotKey = (int)Keys.F6;

        /// <summary>
        ///     The Borderless Toggle hotKey modifier
        /// </summary>
        private const int MakeBorderless_HotKeyModifier = 0x008; // WIN-Key

        /// <summary>
        ///     The Mouse Lock hotKey
        /// </summary>
        private const int MouseLock_HotKey = (int)Keys.Scroll;

        /// <summary>
        ///    The Mouse Hide hotkey
        /// </summary>
        private const int MouseHide_HotKey = (int)Keys.Scroll;

        /// <summary>
        ///    The Mouse Hide hotkey modifier
        /// </summary>
        private const int MouseHide_HotKeyModifier = 0x008;      // WIN-Key

        #endregion

		private BorderlessGaming controller;
        public MainWindow()
        {
			controller = new BorderlessGaming(this);
			controller.Processes.CollectionChanged += Processes_CollectionChanged;
			controller.Favorites.CollectionChanged += Favorites_CollectionChanged;
            InitializeComponent();
        }

        #region External access and processing

        public static MainWindow ext()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(MainWindow))
                    return (MainWindow)form;
            }

            return null;
        }

        private static object _DoEvents_locker = new object();
        private static bool _DoEvents_engaged = false;
        public static void DoEvents()
        {
            try
            {
                bool local__DoEventsEngaged = false;
                lock (_DoEvents_locker)
                {
                    local__DoEventsEngaged = _DoEvents_engaged;

                    if (!local__DoEventsEngaged)
                        _DoEvents_engaged = true;
                }

                if (!local__DoEventsEngaged)
                {
                    // hack-y, but it lets the window message pump process user inputs to keep the UI alive on the main thread
                    Application.DoEvents();
                }

                lock (_DoEvents_locker)
                {
                    _DoEvents_engaged = false;
                }
            }
            catch { }
        }

        #endregion

		private void Processes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Action action = () =>
			{
				lstProcesses.BeginUpdate();
				if (e.NewItems != null)
				{
					//lock(controller.Processes)
					//{
					//cast really isnt needed right now.
					var newItems = e.NewItems.Cast<ProcessDetails>().ToArray();
					foreach (var ni in newItems)
					{
						lstProcesses.Items.Add(ni);
					}
					//}
				}
				if (e.OldItems != null)
				{
					//lock (controller.Processes)
					//{
					var oldItems = e.OldItems.Cast<ProcessDetails>().ToArray();
					foreach (var oi in oldItems)
					{
						lstProcesses.Items.Remove(oi);
					}
					//}
				}
				lstProcesses.EndUpdate();
			};
			if (InvokeRequired)
				Invoke(action);
			else
				action();
		}


		private void Favorites_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Action action = () =>
			{
				if (e.NewItems != null)
				{
					//lock (controller.Favorites)
					//{
					//cast really isnt needed right now.
					var newItems = e.NewItems.Cast<Favorites.Favorite>().ToArray();
					foreach (var ni in newItems)
					{
						lstFavorites.Items.Add(ni);
					}
					//}
				}
				if (e.OldItems != null)
				{
					//lock (controller.Favorites)
					//{
					var oldItems = e.OldItems.Cast<Favorites.Favorite>().ToArray();
					foreach (var oi in oldItems)
					{
						lstFavorites.Items.Remove(oi);
					}
					//}
				}
			};
			if (InvokeRequired)
				Invoke(action);
			else
				action();
		}

        #region Application Menu Events

        private void toolStripRunOnStartup_CheckChanged(object sender, EventArgs e)
        {
            AutoStart.SetShortcut(toolStripRunOnStartup.Checked, Environment.SpecialFolder.Startup, "-silent -minimize");

            AppEnvironment.Setting("RunOnStartup", toolStripRunOnStartup.Checked);
        }

        private void toolStripGlobalHotkey_CheckChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("UseGlobalHotkey", toolStripGlobalHotkey.Checked);

            RegisterHotkeys();
        }

        private void toolStripMouseLock_CheckChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("UseMouseLockHotkey", toolStripMouseLock.Checked);

            RegisterHotkeys();
        }

        private void useMouseHideHotkeyWinScrollLockToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("UseMouseHideHotkey", useMouseHideHotkeyWinScrollLockToolStripMenuItem.Checked);

            RegisterHotkeys();
        }

        private void startMinimizedToTrayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("StartMinimized", startMinimizedToTrayToolStripMenuItem.Checked);
        }

        private void hideBalloonTipsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("HideBalloonTips", hideBalloonTipsToolStripMenuItem.Checked);
        }

        private void closeToTrayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("CloseToTray", closeToTrayToolStripMenuItem.Checked);
        }

        private void useSlowerWindowDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppEnvironment.Setting("SlowWindowDetection", useSlowerWindowDetectionToolStripMenuItem.Checked);
        }

        private async void viewFullProcessDetailsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("ViewAllProcessDetails", viewFullProcessDetailsToolStripMenuItem.Checked);

			await controller.RefreshProcesses();
        }
        
		private async void resetHiddenProcessesToolStripMenuItem_Click(object sender, EventArgs e)
        {
			controller.HiddenProcesses.Reset();
			await controller.RefreshProcesses();
        }
        
        private void openDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                (
                    "explorer.exe",
                    "/e,\"" + AppEnvironment.DataPath + "\",\"" + AppEnvironment.DataPath + "\"")
                );
            }
            catch { }
        }

        private void pauseAutomaticProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
			controller.AutoHandleFavorites = false;
        }

        private void toggleMouseCursorVisibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Manipulation.MouseCursorIsHidden || (MessageBox.Show("Do you really want to hide the mouse cursor?\r\n\r\nYou may have a difficult time finding the mouse again once it's hidden.\r\n\r\nIf you have enabled the global hotkey to toggle the mouse cursor visibility, you can press [Win + Scroll Lock] to toggle the mouse cursor on.\r\n\r\nAlso, exiting Borderless Gaming will immediately restore your mouse cursor.", "Really Hide Mouse Cursor?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes))
                Manipulation.ToggleMouseCursorVisibility(this);
        }

        private void toggleWindowsTaskbarVisibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Manipulation.ToggleWindowsTaskbarVisibility();
        }

        private void toolStripReportBug_Click(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/Borderless-Gaming/issues");
        }

        private void toolStripSupportUs_Click(object sender, EventArgs e)
        {
            Tools.GotoSite("http://store.steampowered.com/app/388080");
        }

		private void toolStripRegexReference_Click(object sender, EventArgs e)
		{
			Tools.GotoSite("www.regular-expressions.info/reference.html");
		}

        private void toolStripAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }
        
		private async void fullApplicationRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
			await controller.RefreshProcesses();
        }

        private void usageGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.GotoSite("http://steamcommunity.com/app/388080/discussions/0/535151589899658778/");
        }

        #endregion

        #region Application Form Events
        
        private void lstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool valid_selection = false;

            if (lstProcesses.SelectedItem != null)
            {
                ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

                valid_selection = pd.Manageable;
            }

            btnMakeBorderless.Enabled = btnRestoreWindow.Enabled = addSelectedItem.Enabled = valid_selection;
        }

        private void lstFavorites_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveFavorite.Enabled = (lstFavorites.SelectedItem != null);
        }

        private void setWindowTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

            Native.SetWindowText(pd.WindowHandle, Tools.Input_Text("Set Window Title", "Set the new window title text:", Native.GetWindowTitle(pd.WindowHandle)));
        }

		private async void hideThisProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

			controller.HiddenProcesses.Add(pd.BinaryName.Trim().ToLower());

			await controller.RefreshProcesses();
        }

        /// <summary>
        ///     Makes the currently selected process borderless
        /// </summary>
        private void btnMakeBorderless_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

			controller.RemoveBorder(pd);
        }

        private void btnRestoreWindow_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

            Manipulation.RestoreWindow(pd);
        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by window title text)
        /// </summary>
        private void byTheWindowTitleTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

			//TODO move to controller
			if (controller.Favorites.CanAdd(pd.WindowTitle))
			{
				Favorites.Favorite fav = new Favorites.Favorite();
				fav.Kind = Favorites.Favorite.FavoriteKinds.ByTitleText;
				fav.SearchText = pd.WindowTitle;
				controller.Favorites.Add(fav);
			}
        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by process binary name)
        /// </summary>
        private void byTheProcessBinaryNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

			//TODO move to controller
			if (controller.Favorites.CanAdd(pd.BinaryName))
			{
				Favorites.Favorite fav = new Favorites.Favorite();
				fav.Kind = Favorites.Favorite.FavoriteKinds.ByBinaryName;
				fav.SearchText = pd.BinaryName;
				controller.Favorites.Add(fav);
			}
        }

        /// <summary>
		///     adds the currently selected process to the favorites (by window title text)
		/// </summary>
		private void byTheWindowTitleTextregexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstProcesses.SelectedItem == null) return;

			ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

			if (!pd.Manageable)
				return;

			//TODO move to controller
			if (controller.Favorites.CanAdd(pd.WindowTitle))
			{
                string res = Tools.Input_Text("Add to favorites by RegEx string", "Regex string (see the Help menu for reference)", pd.WindowTitle);

                if (res.Trim().Length > 0)
                {
                    Favorites.Favorite fav = new Favorites.Favorite();
                    fav.Kind = Favorites.Favorite.FavoriteKinds.ByRegexString;
                    fav.SearchText = res;
                    controller.Favorites.Add(fav);
                }
			}
		}

        private void addSelectedItem_Click(object sender, EventArgs e)
        {
            /*
            Rectangle rect = new Rectangle(0, 0, 0, 0);

            if (true)
            {
                rect = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                foreach (Screen screen in Screen.AllScreens)
                    rect = Tools.GetContainingRectangle(rect, screen.Bounds);
            }

            Form frm = new Form();
            frm.Text = "Borderless Gaming Background";
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.BackColor = Color.Black;
            frm.Show();
            frm.Location = new Point(rect.X, rect.Y);
            frm.Size = new System.Drawing.Size(rect.Width, rect.Height);
            frm.DoubleClick += (Fs, Fe) => { try { ((Form)(Fs)).Close(); } catch { } };
            //frm.Shown += (Fs, Fe) => { Cursor.Hide(); };
            //frm.FormClosing += (Fs, Fe) => { Cursor.Show(); };
            return;
            */

            // assume that the button press to add to favorites will do so by window title (unless it's blank, then go by process name)

            if (lstProcesses.SelectedItem == null) return;

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
                return;

            if (!string.IsNullOrEmpty(pd.WindowTitle))
                byTheWindowTitleTextToolStripMenuItem_Click(sender, e);
            else
                byTheProcessBinaryNameToolStripMenuItem_Click(sender, e);
        }

        private void RefreshFavoritesList(Favorites.Favorite fav = null)
        {
			//refreshing is done through observables so this method just readds the favorite
			//to make it look like it updated and because i dont want to change all that code
			controller.Favorites.Add(fav);
        }

        /// <summary>
        ///     removes the currently selected entry from the favorites
        /// </summary>
        private void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            if (lstFavorites.SelectedItem == null) return;

			//TODO move to controller.
			Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
				return;

			controller.Favorites.Remove(fav);
        }        
        
        private void removeMenusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFavorites.SelectedItem == null) return;

            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
				return;

			controller.Favorites.Remove(fav);

			fav.RemoveMenus = removeMenusToolStripMenuItem.Checked;
			RefreshFavoritesList(fav);
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFavorites.SelectedItem == null) return;

            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            fav.TopMost = alwaysOnTopToolStripMenuItem.Checked;
			RefreshFavoritesList(fav);
        }

        private void adjustWindowBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the left window edge (0 pixels = no adjustment):", fav.OffsetL.ToString()), out fav.OffsetL);
            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the right window edge (0 pixels = no adjustment):", fav.OffsetR.ToString()), out fav.OffsetR);
            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the top window edge (0 pixels = no adjustment):", fav.OffsetT.ToString()), out fav.OffsetT);
            int.TryParse(Tools.Input_Text("Adjust Window Bounds", "Pixel adjustment for the bottom window edge (0 pixels = no adjustment):", fav.OffsetB.ToString()), out fav.OffsetB);

			RefreshFavoritesList(fav);
        }
        
        private void automaximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            fav.ShouldMaximize = automaximizeToolStripMenuItem.Checked;

            if (fav.ShouldMaximize)
            {
                fav.SizeMode = Favorites.Favorite.SizeModes.FullScreen;
                fav.PositionX = 0;
                fav.PositionY = 0;
                fav.PositionW = 0;
                fav.PositionH = 0;
            }

			RefreshFavoritesList(fav);
        }        
        
        private void hideMouseCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            fav.HideMouseCursor = hideMouseCursorToolStripMenuItem.Checked;
			RefreshFavoritesList(fav);
        }

        private void hideWindowsTaskbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            fav.HideWindowsTaskbar = hideWindowsTaskbarToolStripMenuItem.Checked;

            RefreshFavoritesList(fav);
        }

        private void setWindowSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

            DialogResult result = MessageBox.Show("Would you like to select the area using your mouse cursor?\r\n\r\nIf you answer No, you will be prompted for specific pixel dimensions.", "Select Area?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
                return;

            if (result == DialogResult.Yes)
            {
                using (DesktopAreaSelector frmSelectArea = new DesktopAreaSelector())
                {
                    if (frmSelectArea.ShowDialog() != DialogResult.OK)
                        return;

                    // Temporarily disable compiler warning CS1690: http://msdn.microsoft.com/en-us/library/x524dkh4.aspx
                    //
                    // We know what we're doing: everything is safe here.
#pragma warning disable 1690
                    fav.PositionX = frmSelectArea.CurrentTopLeft.X;
                    fav.PositionY = frmSelectArea.CurrentTopLeft.Y;
                    fav.PositionW = frmSelectArea.CurrentBottomRight.X - frmSelectArea.CurrentTopLeft.X;
                    fav.PositionH = frmSelectArea.CurrentBottomRight.Y - frmSelectArea.CurrentTopLeft.Y;
#pragma warning restore 1690
                }
            }
            else // System.Windows.Forms.DialogResult.No
            {
                int.TryParse(Tools.Input_Text("Set Window Size", "Pixel X location for the top left corner (X coordinate):", fav.PositionX.ToString()), out fav.PositionX);
                int.TryParse(Tools.Input_Text("Set Window Size", "Pixel Y location for the top left corner (Y coordinate):", fav.PositionY.ToString()), out fav.PositionY);
                int.TryParse(Tools.Input_Text("Set Window Size", "Window width (in pixels):", fav.PositionW.ToString()), out fav.PositionW);
                int.TryParse(Tools.Input_Text("Set Window Size", "Window height (in pixels):", fav.PositionH.ToString()), out fav.PositionH);
            }

			controller.Favorites.Remove(fav);

            if ((fav.PositionW == 0) || (fav.PositionH == 0))
                fav.SizeMode = Favorites.Favorite.SizeModes.FullScreen;
            else
            {
                fav.SizeMode = Favorites.Favorite.SizeModes.SpecificSize;
                fav.ShouldMaximize = false;
            }

            RefreshFavoritesList(fav);
        }
        
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            fav.SizeMode = (fullScreenToolStripMenuItem.Checked) ? Favorites.Favorite.SizeModes.FullScreen : Favorites.Favorite.SizeModes.NoChange;

            if (fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen)
            {
                fav.PositionX = 0;
                fav.PositionY = 0;
                fav.PositionW = 0;
                fav.PositionH = 0;
            }
            else
                fav.ShouldMaximize = false;

            RefreshFavoritesList(fav);
        }

        
        private void noSizeChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

			if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

			controller.Favorites.Remove(fav);

            fav.SizeMode = (noSizeChangeToolStripMenuItem.Checked) ? Favorites.Favorite.SizeModes.NoChange : Favorites.Favorite.SizeModes.FullScreen;

            if (fav.SizeMode == Favorites.Favorite.SizeModes.NoChange)
            {
                fav.ShouldMaximize = false;
                fav.OffsetL = 0;
                fav.OffsetR = 0;
                fav.OffsetT = 0;
                fav.OffsetB = 0;
                fav.PositionX = 0;
                fav.PositionY = 0;
                fav.PositionW = 0;
                fav.PositionH = 0;
            }
            
            RefreshFavoritesList(fav);
        }

        private void delayBorderlessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;

            if (!controller.Favorites.CanRemove(fav.SearchText))
                return;

            controller.Favorites.Remove(fav);

            fav.DelayBorderless = delayBorderlessToolStripMenuItem.Checked;
            RefreshFavoritesList(fav);

        }

        /// <summary>
        ///     Sets up the Favorite-ContextMenu according to the current state
        /// </summary>
        private void mnuFavoritesContext_Opening(object sender, CancelEventArgs e)
        {
            if (lstFavorites.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }

            Favorites.Favorite fav = (Favorites.Favorite)lstFavorites.SelectedItem;
            fullScreenToolStripMenuItem.Checked = fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen;
            automaximizeToolStripMenuItem.Checked = fav.ShouldMaximize;
            alwaysOnTopToolStripMenuItem.Checked = fav.TopMost;
            hideMouseCursorToolStripMenuItem.Checked = fav.HideMouseCursor;
            hideWindowsTaskbarToolStripMenuItem.Checked = fav.HideWindowsTaskbar;
            removeMenusToolStripMenuItem.Checked = fav.RemoveMenus;

            automaximizeToolStripMenuItem.Enabled = fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen;
            adjustWindowBoundsToolStripMenuItem.Enabled = (fav.SizeMode == Favorites.Favorite.SizeModes.FullScreen) && (!fav.ShouldMaximize);
            setWindowSizeToolStripMenuItem.Enabled = fav.SizeMode != Favorites.Favorite.SizeModes.FullScreen;
            setWindowSizeToolStripMenuItem.Checked = fav.SizeMode == Favorites.Favorite.SizeModes.SpecificSize;
            noSizeChangeToolStripMenuItem.Checked = fav.SizeMode == Favorites.Favorite.SizeModes.NoChange;
        }

        /// <summary>
        ///     Sets up the Process-ContextMenu according to the current state
        /// </summary>
        private void processContext_Opening(object sender, CancelEventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }

            ProcessDetails pd = ((ProcessDetails)lstProcesses.SelectedItem);

            if (!pd.Manageable)
            {
                e.Cancel = true;
                return;
            }

			contextAddToFavs.Enabled = controller.Favorites.CanAdd(pd.BinaryName) && controller.Favorites.CanAdd(pd.WindowTitle);

            if (Screen.AllScreens.Length < 2)
            {
                contextBorderlessOn.Visible = false;
            }
            else
            {
                contextBorderlessOn.Visible = true;

                if (contextBorderlessOn.HasDropDownItems)
                    contextBorderlessOn.DropDownItems.Clear();

                Rectangle superSize = Screen.PrimaryScreen.Bounds;

                foreach (Screen screen in Screen.AllScreens)
                {
                    superSize = Tools.GetContainingRectangle(superSize, screen.Bounds);

                    // fix for a .net-bug on Windows XP
                    int idx = screen.DeviceName.IndexOf('\0');
                    string fixedDeviceName = (idx > 0) ? screen.DeviceName.Substring(0, idx) : screen.DeviceName;

                    string label = fixedDeviceName + ((screen.Primary) ? " (P)" : string.Empty);

                    ToolStripMenuItem tsi = new ToolStripMenuItem(label);
					tsi.Click += (s, ea) => { controller.RemoveBorder_ToSpecificScreen(pd, screen); };

                    contextBorderlessOn.DropDownItems.Add(tsi);
                }

                // add supersize Option
                ToolStripMenuItem superSizeItem = new ToolStripMenuItem("SuperSize!");

				superSizeItem.Click += (s, ea) => { controller.RemoveBorder_ToSpecificRect(pd, superSize); };

                contextBorderlessOn.DropDownItems.Add(superSizeItem);
            }
        }
        
        private ToolStripMenuItem disableSteamIntegrationToolStripMenuItem = null;

        /// <summary>
        ///     Sets up the form
        /// </summary>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // set the title
            Text = "Borderless Gaming " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2) + ((UAC.Elevated) ? " [Administrator]" : "");

            // load up settings
            toolStripRunOnStartup.Checked = AppEnvironment.SettingValue("RunOnStartup", false);
            toolStripGlobalHotkey.Checked = AppEnvironment.SettingValue("UseGlobalHotkey", false);
            toolStripMouseLock.Checked = AppEnvironment.SettingValue("UseMouseLockHotkey", false);
            useMouseHideHotkeyWinScrollLockToolStripMenuItem.Checked = AppEnvironment.SettingValue("UseMouseHideHotkey", false);
            startMinimizedToTrayToolStripMenuItem.Checked = AppEnvironment.SettingValue("StartMinimized", false);
            hideBalloonTipsToolStripMenuItem.Checked = AppEnvironment.SettingValue("HideBalloonTips", false);
            closeToTrayToolStripMenuItem.Checked = AppEnvironment.SettingValue("CloseToTray", false);
            viewFullProcessDetailsToolStripMenuItem.Checked = AppEnvironment.SettingValue("ViewAllProcessDetails", false);
            useSlowerWindowDetectionToolStripMenuItem.Checked = AppEnvironment.SettingValue("SlowWindowDetection", false);

            // minimize the window if desired (hiding done in Shown)
            if (AppEnvironment.SettingValue("StartMinimized", false) || Tools.StartupParameters.Contains("-minimize"))
                WindowState = FormWindowState.Minimized;
            else
                WindowState = FormWindowState.Normal;

            if (Program.SteamLoaded && disableSteamIntegrationToolStripMenuItem == null)
            {
                disableSteamIntegrationToolStripMenuItem =
                    new ToolStripMenuItem
                    {
                        Name = "disableSteamIntegrationToolStripMenuItem",
                        Size = new Size(254, 22),
                        Text = "Disable Steam integration/hook",
                        ToolTipText = "Prevents \"In-App\" on Steam",
                        Checked = AppEnvironment.SettingValue("DisableSteamIntegration", false),
                        CheckOnClick = true
                    };
                // let's do this before registering the CheckedChanged event
                disableSteamIntegrationToolStripMenuItem.CheckedChanged += new EventHandler(disableSteamIntegrationToolStripMenuItem_CheckChanged);
                toolsToolStripMenuItem.DropDownItems.Insert(0, disableSteamIntegrationToolStripMenuItem);
            }
        }

        private void disableSteamIntegrationToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            AppEnvironment.Setting("DisableSteamIntegration", disableSteamIntegrationToolStripMenuItem.Checked);
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // hide the window if desired (this doesn't work well in Load)
            if (AppEnvironment.SettingValue("StartMinimized", false) || Tools.StartupParameters.Contains("-minimize"))
                Hide();

            // initialize favorite list
			foreach (var ni in controller.Favorites)
				lstFavorites.Items.Add(ni);

            // start Task API controller
			controller.Start();

            // Update buttons' enabled/disabled state
            lstProcesses_SelectedIndexChanged(sender, e);
            lstFavorites_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        ///     Cleans up when the application exits (main form closes)
        /// </summary>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Not allowed to exit the application if we've hidden the Windows taskbar.
            //
            // Make them exit the game that triggered the taskbar to be hidden -- or -- use a global hotkey to restore it.
            if (Manipulation.WindowsTaskbarIsHidden)
            {
                ClosingFromExitMenu = false;
                e.Cancel = true;
                return;
            }

            // If we're exiting -- or -- if we're closing-to-tray, then restore the mouse cursor.
            //
            // This prevents a scenario where the user can't (easily) get back to Borderless Gaming to undo the hidden mouse cursor.
            Manipulation.ToggleMouseCursorVisibility(this, Tools.Boolstate.True);

            // If the user didn't choose to exit from the tray icon context menu...
            if (!ClosingFromExitMenu)
            {
                // ... and they have the preference set to close-to-tray ...
                if (AppEnvironment.SettingValue("CloseToTray", false))
                {
                    // ... then minimize the app and do not exit (minimizing will trigger another event to hide the form)
                    WindowState = FormWindowState.Minimized;
                    e.Cancel = true;
                    return;
                }
            }

            // At this point, we're okay to exit the application

            // Unregister all global hotkeys
            UnregisterHotkeys();

            // Hide the tray icon.  If we don't do this, then Environment.Exit() can sometimes ghost the icon in the
            // Windows system tray area.
            trayIcon.Visible = false;
            
            // Overkill... the form should just close naturally.  Ideally we would just allow the form to close and
            // the remaining code in Program.cs would execute (if there were any), but this is how Borderless Gaming has
            // always exited and there may be a compatibility reason for it, so leaving it alone for now.
            Environment.Exit(0);
        }

        private void addSelectedItem_MouseHover(object sender, EventArgs e)
        {
            ToolTip ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control)sender, "Adds the currently-selected application to your favorites list (by the window title).");
        }

        private void btnRemoveFavorite_MouseHover(object sender, EventArgs e)
        {
            ToolTip ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control)sender, "Removes the currently-selected favorite from the list.");
        }

        private void btnMakeBorderless_MouseHover(object sender, EventArgs e)
        {
            ToolTip ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control)sender, "Makes the currently-selected application borderless.");
        }

        private void btnRestoreWindow_MouseHover(object sender, EventArgs e)
        {
            ToolTip ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control)sender, "Attempts to restore a window back to its bordered state.");
        }

        #endregion

        #region Tray Icon Events

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private bool ClosingFromExitMenu = false;
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClosingFromExitMenu = true;
            Close();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                trayIcon.Visible = true;

                if (!AppEnvironment.SettingValue("HideBalloonTips", false) && !Tools.StartupParameters.Contains("-silent"))
                {
                    // Display a balloon tooltip message for 2 seconds
                    trayIcon.BalloonTipText = string.Format(Resources.TrayMinimized, "Borderless Gaming");
                    trayIcon.ShowBalloonTip(2000);
                }

                if (!Manipulation.WindowsTaskbarIsHidden)
                    Hide();
            }
        }

        #endregion

        #region Global HotKeys

        /// <summary>
        ///     registers the global hotkeys
        /// </summary>
        private void RegisterHotkeys()
        {
            UnregisterHotkeys();

            if (AppEnvironment.SettingValue("UseGlobalHotkey", false))
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), MakeBorderless_HotKeyModifier, MakeBorderless_HotKey);

            if (AppEnvironment.SettingValue("UseMouseLockHotkey", false))
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), 0, MouseLock_HotKey);

            if (AppEnvironment.SettingValue("UseMouseHideHotkey", false))
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), MouseHide_HotKeyModifier, MouseHide_HotKey);
        }

        /// <summary>
        ///     unregisters the global hotkeys
        /// </summary>
        private void UnregisterHotkeys()
        {
            Native.UnregisterHotKey(Handle, GetType().GetHashCode());
        }

        /// <summary>
        ///     Catches the Hotkeys
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_HOTKEY)
            {
                uint keystroke          = ((uint)m.LParam >> 16) & 0x0000FFFF;
                uint keystroke_modifier = ((uint)m.LParam)       & 0x0000FFFF;

                // Global hotkey to make a window borderless
                if ((keystroke == MakeBorderless_HotKey) && (keystroke_modifier == MakeBorderless_HotKeyModifier))
                {
                    // Find the currently-active window
                    IntPtr hCurrentActiveWindow = Native.GetForegroundWindow();

                    // Only if that window isn't Borderless Windows itself
                    if (hCurrentActiveWindow != Handle)
                    {
                        // Figure out the process details based on the current window handle
						ProcessDetails pd = controller.Processes.FromHandle(hCurrentActiveWindow);
						if (pd == null)
						{
							Task.WaitAll(controller.RefreshProcesses());
							pd = controller.Processes.FromHandle(hCurrentActiveWindow);
							if (pd == null)
								return;
						}
                        // If we have information about this process -and- we've already made it borderless, then reverse the process
						if (pd.MadeBorderless)
                            Manipulation.RestoreWindow(pd);
                        // Otherwise, this is a fresh request to remove the border from the current window
                        else
							controller.RemoveBorder(pd);
                    }

                    return; // handled the message, do not call base WndProc for this message
                }

                if ((keystroke == MouseHide_HotKey) && (keystroke_modifier == MouseHide_HotKeyModifier))
                {
                    Manipulation.ToggleMouseCursorVisibility(this);

                    return; // handled the message, do not call base WndProc for this message
                }

                if ((keystroke == MouseLock_HotKey) && (keystroke_modifier == 0))
                {
                    IntPtr hWnd = Native.GetForegroundWindow();

                    // get size of clientarea
                    Native.Rect rect = new Native.Rect();
                    Native.GetClientRect(hWnd, ref rect);

                    // get top,left point of clientarea
                    Native.POINTAPI p = new Native.POINTAPI { X = 0, Y = 0 };
                    Native.ClientToScreen(hWnd, ref p);

                    Rectangle clipRect = new Rectangle(p.X, p.Y, rect.Right - rect.Left, rect.Bottom - rect.Top);

                    Cursor.Clip = (Cursor.Clip.Equals(clipRect)) ? Rectangle.Empty : clipRect;

                    return; // handled the message, do not call base WndProc for this message
                }
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}
