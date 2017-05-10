using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using BorderlessGaming.Properties;
using BorderlessGaming.Utilities;

namespace BorderlessGaming.Common
{
    public class ProcessDetails
    {
        public Process Proc = null;
        public string DescriptionOverride = "";
        public string WindowTitle = "<unknown>";
        //public string WindowClass = ""; // note: this isn't used, currently
        public IntPtr _WindowHandle = IntPtr.Zero;
        public bool Manageable = false;
        public bool MadeBorderless = false;
        public bool NoAccess = false;
        public int MadeBorderlessAttempts = 0;
        public WindowsAPI.WindowStyleFlags OriginalStyleFlags_Standard = 0;
        public WindowsAPI.WindowStyleFlags OriginalStyleFlags_Extended = 0;
        public Rectangle OriginalLocation = new Rectangle();
		
		public ProcessDetails(Process p, IntPtr hWnd)
        {
            Proc = p;

            WindowHandle = hWnd;
            WindowTitle = "<error>";
            //this.WindowTitle = WindowsAPI.Native.GetWindowTitle(this.WindowHandle);
            Tools.StartMethodMultithreadedAndWait(() => { WindowTitle = WindowsAPI.Native.GetWindowTitle(WindowHandle); }, (AppEnvironment.SettingValue("SlowWindowDetection", false)) ? 10 : 2);
            //this.WindowClass = WindowsAPI.Native.GetWindowClassName(this.WindowHandle); // note: this isn't used, currently
        }

        // Automatically detects changes to the window handle
        public IntPtr WindowHandle
        {
            get
            {
                try
                {
                    if (ProcessHasExited)
                        return IntPtr.Zero;

                    if (!WindowsAPI.Native.IsWindow(_WindowHandle))
                        _WindowHandle = WindowsAPI.Native.GetMainWindowForProcess(Proc);
                }
                catch { }

                return _WindowHandle;
            }
            set
            {
                _WindowHandle = value;
            }
        }

        public bool ProcessHasExited
        {
            get
            {
                try
                {
                    if (NoAccess)
                        return false;

                    if (Proc != null)
                        return Proc.HasExited;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    NoAccess = true;

                    return false; // Access is denied
                }
                catch { }

                return true;
            }
        }
        
        public string BinaryName
        {
            get
            {
                try
                {
                    return NoAccess ? "<error>" : Proc.ProcessName;
                }
                catch
                {
                    NoAccess = true;
                }

                return "<error>";
            }
        }

        public override string ToString() // so that the ListView control knows how to display this object to the user
        {
            try
            {
                if (!string.IsNullOrEmpty(DescriptionOverride))
                    return DescriptionOverride;

                if (AppEnvironment.SettingValue("ViewAllProcessDetails", false))
                {
                    WindowsAPI.WindowStyleFlags styleCurrentWindow_standard = WindowsAPI.Native.GetWindowLong(WindowHandle, WindowsAPI.WindowLongIndex.Style);
                    WindowsAPI.WindowStyleFlags styleCurrentWindow_extended = WindowsAPI.Native.GetWindowLong(WindowHandle, WindowsAPI.WindowLongIndex.ExtendedStyle);

                    string extra_details = string.Format(" [{0:X8}.{1:X8}]", (UInt32)styleCurrentWindow_standard, (UInt32)styleCurrentWindow_extended);

                    if (WindowTitle.Trim().Length == 0)
                        return BinaryName + " [#" + Proc.Id.ToString() + "]" + extra_details;

                    return WindowTitle.Trim() + " [" + BinaryName + ", #" + Proc.Id.ToString() + "]" + extra_details;
                }

                if (WindowTitle.Trim().Length == 0)
                    return BinaryName;

                bool ProcessNameIsDissimilarToWindowTitle = true;
                if (WindowTitle_ForComparison.Length >= 5)
                    if (BinaryName_ForComparison.Length >= 5)
                        if (BinaryName_ForComparison.Substring(0, 5) == WindowTitle_ForComparison.Substring(0, 5))
                            ProcessNameIsDissimilarToWindowTitle = false;

                if (ProcessNameIsDissimilarToWindowTitle)
                    return WindowTitle.Trim() + " [" + BinaryName + "]";

                return WindowTitle.Trim();

            }
            catch { }

            return "<error>";
        }

        private string WindowTitle_ForComparison
        {
            get
            {
                return WindowTitle.Trim().ToLower().Replace(" ", "").Replace("_", "");
            }
        }

        private string BinaryName_ForComparison
        {
            get
            {
                return BinaryName.Trim().ToLower().Replace(" ", "").Replace("_", "");
            }
        }

        // Detect whether or not the window needs border changes
        public bool WindowHasTargetableStyles
        {
            get
            {
                bool targetable = false;

                Tools.StartMethodMultithreadedAndWait(() =>
                {
                    var styleCurrentWindowStandard = WindowsAPI.Native.GetWindowLong(WindowHandle, WindowsAPI.WindowLongIndex.Style);

                    if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.Border) > 0) targetable = true;
                    if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.DialogFrame) > 0) targetable = true;
                    if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ThickFrame) > 0) targetable = true;
                    if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.SystemMenu) > 0) targetable = true;
                    if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.MaximizeBox) > 0) targetable = true;
                    if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.MinimizeBox) > 0) targetable = true;

                    if (!targetable)
                    {
                        var styleCurrentWindowExtended = WindowsAPI.Native.GetWindowLong(WindowHandle, WindowsAPI.WindowLongIndex.ExtendedStyle);

                        if (!targetable) if ((styleCurrentWindowExtended | WindowsAPI.WindowStyleFlags.ExtendedDlgModalFrame) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedComposited) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedWindowEdge) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedClientEdge) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedLayered) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedStaticEdge) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedToolWindow) > 0) targetable = true;
                        if (!targetable) if ((styleCurrentWindowStandard | WindowsAPI.WindowStyleFlags.ExtendedAppWindow) > 0) targetable = true;
                    }
                }, (AppEnvironment.SettingValue("SlowWindowDetection", false)) ? 10 : 2);
                return targetable;
            }
        }

        public static implicit operator Process(ProcessDetails pd)
        {
            return pd?.Proc;
        }

        public static implicit operator IntPtr(ProcessDetails pd)
        {
            return pd?.WindowHandle ?? IntPtr.Zero;
        }
    }
}
