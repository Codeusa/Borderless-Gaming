using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.Steam;
using BorderlessGaming.Logic.System;
using BorderlessGaming.Logic.Windows;
using BorderlessGaming.Properties;

namespace BorderlessGaming.Forms
{
    public partial class MainWindow : Form
    {
        private readonly LogicWrapper _controller;

        public MainWindow()
        {
            _controller = new LogicWrapper(this);
            _controller.Processes.CollectionChanged += Processes_CollectionChanged;
            InitializeComponent();
        }

        private void Processes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }


        public void AddFavoriteToList(Favorite fav)
        {
            lstFavorites.Items.Add(fav);
        }

        public void RemoveFavoriteFromList(Favorite fav)
        {
            lstFavorites.Items.Remove(fav);
        }

        private void toolStripCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.CheckForUpdates = toolStripCheckForUpdates.Checked;
            Config.Save();
        }

        #region Local data

        /// <summary>
        ///     The Borderless Toggle hotKey
        /// </summary>
        private const int MakeBorderless_HotKey = (int) Keys.F6;

        /// <summary>
        ///     The Borderless Toggle hotKey modifier
        /// </summary>
        private const int MakeBorderless_HotKeyModifier = 0x008; // WIN-Key

        /// <summary>
        ///     The Mouse Lock hotKey
        /// </summary>
        private const int MouseLock_HotKey = (int) Keys.Scroll;

        /// <summary>
        ///     The Mouse Hide hotkey
        /// </summary>
        private const int MouseHide_HotKey = (int) Keys.Scroll;

        /// <summary>
        ///     The Mouse Hide hotkey modifier
        /// </summary>
        private const int MouseHide_HotKeyModifier = 0x008; // WIN-Key

        #endregion

        #region External access and processing

        public static MainWindow Ext()
        {
            return Application.OpenForms.Cast<Form>().Where(form => form.GetType() == typeof(MainWindow))
                .Cast<MainWindow>().FirstOrDefault();
        }

        private static readonly object _DoEvents_locker = new object();
        private static bool _DoEvents_engaged;

        public static void DoEvents()
        {
            try
            {
                var local__DoEventsEngaged = false;
                lock (_DoEvents_locker)
                {
                    local__DoEventsEngaged = _DoEvents_engaged;

                    if (!local__DoEventsEngaged)
                    {
                        _DoEvents_engaged = true;
                    }
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
            catch
            {
            }
        }

        #endregion


        #region Application Menu Events

        private void toolStripRunOnStartup_CheckChanged(object sender, EventArgs e)
        {
            AutoStart.Setup(toolStripRunOnStartup.Checked, "--silent --minimize");
            Config.Instance.AppSettings.RunOnStartup = toolStripRunOnStartup.Checked;
            Config.Save();
        }

        private void toolStripGlobalHotkey_CheckChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.UseGlobalHotkey = toolStripGlobalHotkey.Checked;
            Config.Save();
            RegisterHotkeys();
        }

        private void toolStripMouseLock_CheckChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.UseMouseLockHotkey = toolStripMouseLock.Checked;
            Config.Save();
            RegisterHotkeys();
        }

        private void useMouseHideHotkeyWinScrollLockToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.UseMouseHideHotkey = useMouseHideHotkeyWinScrollLockToolStripMenuItem.Checked;
            Config.Save();
            RegisterHotkeys();
        }

        private void startMinimizedToTrayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.StartMinimized = startMinimizedToTrayToolStripMenuItem.Checked;
            Config.Save();
        }

        private void hideBalloonTipsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.HideBalloonTips = hideBalloonTipsToolStripMenuItem.Checked;
            Config.Save();
        }

        private void closeToTrayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.CloseToTray = closeToTrayToolStripMenuItem.Checked;
            Config.Save();
        }

        private void useSlowerWindowDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.SlowWindowDetection = useSlowerWindowDetectionToolStripMenuItem.Checked;
            Config.Save();
        }

        private async void viewFullProcessDetailsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)

        {
            Config.Instance.AppSettings.ViewAllProcessDetails = viewFullProcessDetailsToolStripMenuItem.Checked;
            Config.Save();
            await _controller.RefreshProcesses();
        }

        private async void resetHiddenProcessesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await _controller.RefreshProcesses();
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
            catch
            {
            }
        }

        private void pauseAutomaticProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.AutoHandleFavorites = false;
        }

        private void toggleMouseCursorVisibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Manipulation.MouseCursorIsHidden ||
                MessageBox.Show(
                    "Do you really want to hide the mouse cursor?\r\n\r\nYou may have a difficult time finding the mouse again once it's hidden.\r\n\r\nIf you have enabled the global hotkey to toggle the mouse cursor visibility, you can press [Win + Scroll Lock] to toggle the mouse cursor on.\r\n\r\nAlso, exiting Borderless Gaming will immediately restore your mouse cursor.",
                    "Really Hide Mouse Cursor?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Manipulation.ToggleMouseCursorVisibility(this);
            }
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
            await _controller.RefreshProcesses();
        }

        private void usageGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.GotoSite("http://steamcommunity.com/app/388080/discussions/0/535151589899658778/");
        }

        #endregion

        #region Application Form Events

        private void lstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var valid_selection = false;

            if (lstProcesses.SelectedItem != null)
            {
                var pd = (ProcessDetails) lstProcesses.SelectedItem;

                valid_selection = pd.Manageable;
            }

            btnMakeBorderless.Enabled = btnRestoreWindow.Enabled = addSelectedItem.Enabled = valid_selection;
        }

        private void lstFavorites_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveFavorite.Enabled = lstFavorites.SelectedItem != null;
        }

        private void setWindowTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }

            Native.SetWindowText(pd.WindowHandle,
                InputText("Set Window Title", "Set the new window title text:",
                    Native.GetWindowTitle(pd.WindowHandle)));
        }

        private async void hideThisProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }

            Config.Instance.ExcludeProcess(pd.BinaryName.Trim().ToLower());
            await _controller.RefreshProcesses();
        }

        /// <summary>
        ///     Makes the currently selected process borderless
        /// </summary>
        private void btnMakeBorderless_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }

            _controller.RemoveBorder(pd);
        }

        private void btnRestoreWindow_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }

            Manipulation.RestoreWindow(pd);
        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by window title text)
        /// </summary>
        private void byTheWindowTitleTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }
            var favorite = new Favorite
            {
                Type = FavoriteType.Title,
                SearchText = pd.WindowTitle
            };
            Config.Instance.AddFavorite(favorite, () =>
            {
                lstFavorites.Items.Add(favorite);
            });

        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by process binary name)
        /// </summary>
        private void byTheProcessBinaryNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }
          var favorite = new Favorite
          {
              Type = FavoriteType.Process,
              SearchText = pd.BinaryName
          };
            Config.Instance.AddFavorite(favorite, () =>
            {
                lstFavorites.Items.Add(favorite);
            });
        }

        /// <summary>
        ///     adds the currently selected process to the favorites (by window title text)
        /// </summary>
        private void byTheWindowTitleTextregexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }
            var res = InputText("Add to favorites by RegEx string",
                "Regex string (see the Help menu for reference)", pd.WindowTitle);
            if (!string.IsNullOrWhiteSpace(res.Trim()))
            {
                var favorite = new Favorite
                {
                    Type = FavoriteType.Regex,
                    SearchText = res
                };
                Config.Instance.AddFavorite(favorite, () =>
                {
                    lstFavorites.Items.Add(favorite);
                });
            }
        }
        private string InputText(string sTitle, string sInstructions, string sDefaultValue = "")
        {
            try
            {
                using (var inputForm = new InputText())
                {
                    inputForm.Title = sTitle;
                    inputForm.Instructions = sInstructions;
                    inputForm.Input = sDefaultValue;

                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        return inputForm.Input;
                    }

                    return sDefaultValue;
                }
            }
            catch
            {
            }

            return string.Empty;
        }
        private void addSelectedItem_Click(object sender, EventArgs e)
        {
            if (lstProcesses.SelectedItem == null)
            {
                return;
            }

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                return;
            }

            if (!string.IsNullOrEmpty(pd.WindowTitle))
            {
                byTheWindowTitleTextToolStripMenuItem_Click(sender, e);
            }
            else
            {
                byTheProcessBinaryNameToolStripMenuItem_Click(sender, e);
            }
        }

        private void RefreshFavoritesList(Favorite fav = null)
        {
            //refreshing is done through observables so this method just readds the favorite
            //to make it look like it updated and because i dont want to change all that code
            Config.Instance.AddFavorite(fav, () =>
            {
                lstFavorites.Items.Add(fav);
            });
        }

        /// <summary>
        ///     removes the currently selected entry from the favorites
        /// </summary>
        private void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            if (lstFavorites.SelectedItem == null)
            {
                return;
            }
            var fav = (Favorite) lstFavorites.SelectedItem;
            Config.Instance.RemoveFavorite(fav, () =>
            {
              lstFavorites.Items.Remove(fav);
            });
        }

        private void removeMenusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFavorites.SelectedItem == null)
            {
                return;
            }

            var fav = (Favorite) lstFavorites.SelectedItem;

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });
            fav.RemoveMenus = removeMenusToolStripMenuItem.Checked;
            RefreshFavoritesList(fav);
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstFavorites.SelectedItem == null)
            {
                return;
            }

            var fav = (Favorite) lstFavorites.SelectedItem;
            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });
            fav.TopMost = alwaysOnTopToolStripMenuItem.Checked;
            RefreshFavoritesList(fav);
        }

        private void adjustWindowBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });
            int favOffsetL;
            int favOffsetR;
            int favOffsetT;
            int favOffsetB;
            int.TryParse(
                InputText("Adjust Window Bounds",
                    "Pixel adjustment for the left window edge (0 pixels = no adjustment):", fav.OffsetL.ToString()),
                out favOffsetL);
            int.TryParse(
               InputText("Adjust Window Bounds",
                    "Pixel adjustment for the right window edge (0 pixels = no adjustment):", fav.OffsetR.ToString()),
                out favOffsetR);
            int.TryParse(
                InputText("Adjust Window Bounds",
                    "Pixel adjustment for the top window edge (0 pixels = no adjustment):", fav.OffsetT.ToString()),
                out favOffsetT);
            int.TryParse(
                InputText("Adjust Window Bounds",
                    "Pixel adjustment for the bottom window edge (0 pixels = no adjustment):", fav.OffsetB.ToString()),
                out favOffsetB);

            fav.OffsetL = favOffsetL;
            fav.OffsetR = favOffsetR;
            fav.OffsetT = favOffsetT;
            fav.OffsetB = favOffsetB;

            RefreshFavoritesList(fav);
        }

        private void automaximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });

            fav.ShouldMaximize = automaximizeToolStripMenuItem.Checked;

            if (fav.ShouldMaximize)
            {
                fav.Size = FavoriteSize.FullScreen;
                fav.PositionX = 0;
                fav.PositionY = 0;
                fav.PositionW = 0;
                fav.PositionH = 0;
            }

            RefreshFavoritesList(fav);
        }

        private void hideMouseCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;
            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });
            fav.HideMouseCursor = hideMouseCursorToolStripMenuItem.Checked;
            RefreshFavoritesList(fav);
        }

        private void hideWindowsTaskbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;
            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });

            fav.HideWindowsTaskbar = hideWindowsTaskbarToolStripMenuItem.Checked;

            RefreshFavoritesList(fav);
        }

        private void setWindowSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;


            var result =
                MessageBox.Show(
                    "Would you like to select the area using your mouse cursor?\r\n\r\nIf you answer No, you will be prompted for specific pixel dimensions.",
                    "Select Area?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (result == DialogResult.Yes)
            {
                using (var frmSelectArea = new DesktopAreaSelector())
                {
                    if (frmSelectArea.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

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
                var favPositionX = fav.PositionX;
                var favPositionY = fav.PositionY;
                var favPositionW = fav.PositionW;
                var favPositionH = fav.PositionH;
                int.TryParse(
                    InputText("Set Window Size", "Pixel X location for the top left corner (X coordinate):",
                        fav.PositionX.ToString()), out favPositionX);
                int.TryParse(
                   InputText("Set Window Size", "Pixel Y location for the top left corner (Y coordinate):",
                        fav.PositionY.ToString()), out favPositionY);
                int.TryParse(InputText("Set Window Size", "Window width (in pixels):", fav.PositionW.ToString()),
                    out favPositionW);
                int.TryParse(
                    InputText("Set Window Size", "Window height (in pixels):", fav.PositionH.ToString()),
                    out favPositionH);
                fav.PositionX = favPositionX;
                fav.PositionH = favPositionH;
                fav.PositionW = favPositionW;
                fav.PositionY = favPositionY;
            }

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });

            if (fav.PositionW == 0 || fav.PositionH == 0)
            {
                fav.Size = FavoriteSize.FullScreen;
            }
            else
            {
                fav.Size = FavoriteSize.SpecificSize;
                fav.ShouldMaximize = false;
            }
            RefreshFavoritesList(fav);
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });


            fav.Size = fullScreenToolStripMenuItem.Checked ? FavoriteSize.FullScreen : FavoriteSize.NoChange;

            if (fav.Size == FavoriteSize.FullScreen)
            {
                fav.PositionX = 0;
                fav.PositionY = 0;
                fav.PositionW = 0;
                fav.PositionH = 0;
            }
            else
            {
                fav.ShouldMaximize = false;
            }

            RefreshFavoritesList(fav);
        }


        private void noSizeChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fav = (Favorite) lstFavorites.SelectedItem;

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });

            fav.Size = noSizeChangeToolStripMenuItem.Checked ? FavoriteSize.NoChange : FavoriteSize.FullScreen;

            if (fav.Size == FavoriteSize.NoChange)
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
            var fav = (Favorite) lstFavorites.SelectedItem;

            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });

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

            var fav = (Favorite) lstFavorites.SelectedItem;
            fullScreenToolStripMenuItem.Checked = fav.Size == FavoriteSize.FullScreen;

            muteInBackgroundToolStripMenuItem.Checked = fav.MuteInBackground;
            automaximizeToolStripMenuItem.Checked = fav.ShouldMaximize;
            alwaysOnTopToolStripMenuItem.Checked = fav.TopMost;
            hideMouseCursorToolStripMenuItem.Checked = fav.HideMouseCursor;
            hideWindowsTaskbarToolStripMenuItem.Checked = fav.HideWindowsTaskbar;
            removeMenusToolStripMenuItem.Checked = fav.RemoveMenus;

            automaximizeToolStripMenuItem.Enabled = fav.Size == FavoriteSize.FullScreen;
            adjustWindowBoundsToolStripMenuItem.Enabled = fav.Size == FavoriteSize.FullScreen && !fav.ShouldMaximize;
            setWindowSizeToolStripMenuItem.Enabled = fav.Size != FavoriteSize.FullScreen;
            setWindowSizeToolStripMenuItem.Checked = fav.Size == FavoriteSize.SpecificSize;
            noSizeChangeToolStripMenuItem.Checked = fav.Size == FavoriteSize.NoChange;

            if (Screen.AllScreens.Length < 2)
            {
                contextFavScreen.Visible = false;
            }
            else
            {
                contextFavScreen.Visible = true;

                if (contextFavScreen.HasDropDownItems)
                {
                    contextFavScreen.DropDownItems.Clear();
                }

                var superSize = Screen.PrimaryScreen.Bounds;

                foreach (var screen in Screen.AllScreens)
                {
                    superSize = Tools.GetContainingRectangle(superSize, screen.Bounds);

                    // fix for a .net-bug on Windows XP
                    var idx = screen.DeviceName.IndexOf('\0');
                    var fixedDeviceName = idx > 0 ? screen.DeviceName.Substring(0, idx) : screen.DeviceName;

                    var label = fixedDeviceName + (screen.Primary ? " (P)" : string.Empty);

                    var tsi = new ToolStripMenuItem(label);
                    tsi.Checked = fav.FavScreen?.Equals(PRectangle.ToPRectangle(screen.Bounds)) ?? false;
                    tsi.Click += (s, ea) =>
                    {
                        if (tsi.Checked)
                        {
                            fav.FavScreen =
                                new PRectangle(); // Can't null a Rectangle, so can never fully un-favorite a screen without removing the favorite.
                        }
                        else
                        {
                            fav.FavScreen = PRectangle.ToPRectangle(screen.Bounds);
                        }
                        Config.Save();
                    };

                    contextFavScreen.DropDownItems.Add(tsi);
                }

                // add supersize Option
                var superSizeItem = new ToolStripMenuItem("SuperSize!");

                superSizeItem.Click += (s, ea) => { fav.FavScreen = PRectangle.ToPRectangle(superSize); };

                contextFavScreen.DropDownItems.Add(superSizeItem);
            }
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

            var pd = (ProcessDetails) lstProcesses.SelectedItem;

            if (!pd.Manageable)
            {
                e.Cancel = true;
                return;
            }

            contextAddToFavs.Enabled = Config.Instance.CanAddFavorite(pd.BinaryName) &&
                                       Config.Instance.CanAddFavorite(pd.WindowTitle);

            if (Screen.AllScreens.Length < 2)
            {
                contextBorderlessOn.Visible = false;
            }
            else
            {
                contextBorderlessOn.Visible = true;

                if (contextBorderlessOn.HasDropDownItems)
                {
                    contextBorderlessOn.DropDownItems.Clear();
                }

                var superSize = Screen.PrimaryScreen.Bounds;

                foreach (var screen in Screen.AllScreens)
                {
                    superSize = Tools.GetContainingRectangle(superSize, screen.Bounds);

                    // fix for a .net-bug on Windows XP
                    var idx = screen.DeviceName.IndexOf('\0');
                    var fixedDeviceName = idx > 0 ? screen.DeviceName.Substring(0, idx) : screen.DeviceName;

                    var label = fixedDeviceName + (screen.Primary ? " (P)" : string.Empty);

                    var tsi = new ToolStripMenuItem(label);
                    tsi.Click += (s, ea) => { _controller.RemoveBorder_ToSpecificScreen(pd, screen); };

                    contextBorderlessOn.DropDownItems.Add(tsi);
                }

                // add supersize Option
                var superSizeItem = new ToolStripMenuItem("SuperSize!");

                superSizeItem.Click += (s, ea) => { _controller.RemoveBorder_ToSpecificRect(pd, superSize); };

                contextBorderlessOn.DropDownItems.Add(superSizeItem);
            }
        }

        private ToolStripMenuItem disableSteamIntegrationToolStripMenuItem;

        /// <summary>
        ///     Sets up the form
        /// </summary>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // set the title
            Text = "Borderless Gaming " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3) + ((Uac.Elevated) ? " [Administrator]" : "");

            var settings = Config.Instance.AppSettings;
            // load up settings
            toolStripRunOnStartup.Checked = settings.RunOnStartup;
            toolStripGlobalHotkey.Checked = settings.UseGlobalHotkey;
            toolStripCheckForUpdates.Checked = settings.CheckForUpdates;
            toolStripMouseLock.Checked = settings.UseMouseLockHotkey;
            useMouseHideHotkeyWinScrollLockToolStripMenuItem.Checked = settings.UseMouseHideHotkey;
            startMinimizedToTrayToolStripMenuItem.Checked = settings.StartMinimized;
            hideBalloonTipsToolStripMenuItem.Checked = settings.HideBalloonTips;
            closeToTrayToolStripMenuItem.Checked = settings.CloseToTray;
            viewFullProcessDetailsToolStripMenuItem.Checked = settings.ViewAllProcessDetails;
            useSlowerWindowDetectionToolStripMenuItem.Checked = settings.SlowWindowDetection;

            // minimize the window if desired (hiding done in Shown)
            if (settings.StartMinimized || Config.Instance.StartupOptions.Minimize)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }

            if (SteamApi.IsLoaded && disableSteamIntegrationToolStripMenuItem == null)
            {
                disableSteamIntegrationToolStripMenuItem =
                    new ToolStripMenuItem
                    {
                        Name = "disableSteamIntegrationToolStripMenuItem",
                        Size = new Size(254, 22),
                        Text = "Disable Steam integration/hook",
                        ToolTipText = "Prevents \"In-App\" on Steam",
                        Checked = settings.DisableSteamIntegration,
                        CheckOnClick = true
                    };
                // let's do this before registering the CheckedChanged event
                disableSteamIntegrationToolStripMenuItem.CheckedChanged +=
                    disableSteamIntegrationToolStripMenuItem_CheckChanged;
                toolsToolStripMenuItem.DropDownItems.Insert(0, disableSteamIntegrationToolStripMenuItem);
            }
        }

        private void disableSteamIntegrationToolStripMenuItem_CheckChanged(object sender, EventArgs e)
        {
            Config.Instance.AppSettings.DisableSteamIntegration = disableSteamIntegrationToolStripMenuItem.Checked;
            Config.Save();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // hide the window if desired (this doesn't work well in Load)
            if (Config.Instance.AppSettings.StartMinimized || Config.Instance.StartupOptions.Minimize)
            {
                Hide();
            }

            // initialize favorite list
            foreach (var ni in Config.Instance.Favorites)
            {
                lstFavorites.Items.Add(ni);
            }

            // start Task API controller
            _controller.Start();

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
            Manipulation.ToggleMouseCursorVisibility(this, Boolstate.True);

            // If the user didn't choose to exit from the tray icon context menu...
            if (!ClosingFromExitMenu)
            {
                // ... and they have the preference set to close-to-tray ...
                if (Config.Instance.AppSettings.CloseToTray)
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
            var ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control) sender,
                "Adds the currently-selected application to your favorites list (by the window title).");
        }

        private void btnRemoveFavorite_MouseHover(object sender, EventArgs e)
        {
            var ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control) sender, "Removes the currently-selected favorite from the list.");
        }

        private void btnMakeBorderless_MouseHover(object sender, EventArgs e)
        {
            var ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control) sender, "Makes the currently-selected application borderless.");
        }

        private void btnRestoreWindow_MouseHover(object sender, EventArgs e)
        {
            var ttTemp = new ToolTip();
            ttTemp.SetToolTip((Control) sender, "Attempts to restore a window back to its bordered state.");
        }

        #endregion

        #region Tray Icon Events

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private bool ClosingFromExitMenu;

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

                if (Config.Instance.AppSettings.HideBalloonTips && !Config.Instance.StartupOptions.Silent)
                {
                    // Display a balloon tooltip message for 2 seconds
                    trayIcon.BalloonTipText = string.Format(Resources.TrayMinimized, "Borderless Gaming");
                    trayIcon.ShowBalloonTip(2000);
                }

                if (!Manipulation.WindowsTaskbarIsHidden)
                {
                    Hide();
                }
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

            if (Config.Instance.AppSettings.UseGlobalHotkey)
            {
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), MakeBorderless_HotKeyModifier,
                    MakeBorderless_HotKey);
            }

            if (Config.Instance.AppSettings.UseMouseLockHotkey)
            {
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), 0, MouseLock_HotKey);
            }

            if (Config.Instance.AppSettings.UseMouseHideHotkey)
            {
                Native.RegisterHotKey(Handle, GetType().GetHashCode(), MouseHide_HotKeyModifier, MouseHide_HotKey);
            }
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
                var keystroke = ((uint) m.LParam >> 16) & 0x0000FFFF;
                var keystroke_modifier = (uint) m.LParam & 0x0000FFFF;

                // Global hotkey to make a window borderless
                if (keystroke == MakeBorderless_HotKey && keystroke_modifier == MakeBorderless_HotKeyModifier)
                {
                    // Find the currently-active window
                    var hCurrentActiveWindow = Native.GetForegroundWindow();

                    // Only if that window isn't Borderless Windows itself
                    if (hCurrentActiveWindow != Handle)
                    {
                        // Figure out the process details based on the current window handle
                        var pd = _controller.Processes.FromHandle(hCurrentActiveWindow);
                        if (pd == null)
                        {
                            Task.WaitAll(_controller.RefreshProcesses());
                            pd = _controller.Processes.FromHandle(hCurrentActiveWindow);
                            if (pd == null)
                            {
                                return;
                            }
                        }
                        // If we have information about this process -and- we've already made it borderless, then reverse the process
                        if (pd.MadeBorderless)
                        {
                            Manipulation.RestoreWindow(pd);
                        }
                        // Otherwise, this is a fresh request to remove the border from the current window
                        else
                        {
                            _controller.RemoveBorder(pd);
                        }
                    }

                    return; // handled the message, do not call base WndProc for this message
                }

                if (keystroke == MouseHide_HotKey && keystroke_modifier == MouseHide_HotKeyModifier)
                {
                    Manipulation.ToggleMouseCursorVisibility(this);

                    return; // handled the message, do not call base WndProc for this message
                }

                if (keystroke == MouseLock_HotKey && keystroke_modifier == 0)
                {
                    var hWnd = Native.GetForegroundWindow();

                    // get size of clientarea
                    var rect = new Native.Rect();
                    Native.GetClientRect(hWnd, ref rect);

                    // get top,left point of clientarea
                    var p = new Native.POINTAPI {X = 0, Y = 0};
                    Native.ClientToScreen(hWnd, ref p);

                    var clipRect = new Rectangle(p.X, p.Y, rect.Right - rect.Left, rect.Bottom - rect.Top);

                    Cursor.Clip = Cursor.Clip.Equals(clipRect) ? Rectangle.Empty : clipRect;

                    return; // handled the message, do not call base WndProc for this message
                }
            }

            base.WndProc(ref m);
        }

        #endregion

        private void muteInBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
      
            var fav = (Favorite)lstFavorites.SelectedItem;
            Config.Instance.RemoveFavorite(fav, () =>
            {
                lstFavorites.Items.Remove(fav);
            });
            fav.MuteInBackground = muteInBackgroundToolStripMenuItem.Checked;
            if (!fav.MuteInBackground)
            {
                if (fav.IsRunning && Native.IsMuted(fav.RunningId))
                {
                    Native.UnMuteProcess(fav.RunningId);
                }
            } else if (fav.MuteInBackground)
            {
                if (fav.IsRunning && !Native.IsMuted(fav.RunningId))
                {
                    Native.MuteProcess(fav.RunningId);
                }
            }
            RefreshFavoritesList(fav);
        }
    }
}