using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using BorderlessGaming.Utilities;
using BorderlessGaming.WindowsAPI;

namespace BorderlessGaming.Common
{
    public class ProcessDetails
    {
        //public string WindowClass = ""; // note: this isn't used, currently
        public IntPtr _WindowHandle = IntPtr.Zero;

        public string DescriptionOverride = "";
        public bool MadeBorderless = false;
        public int MadeBorderlessAttempts = 0;
        public bool Manageable = false;
        public bool NoAccess;
        public Rectangle OriginalLocation = new Rectangle();
        public WindowStyleFlags OriginalStyleFlags_Extended = 0;
        public WindowStyleFlags OriginalStyleFlags_Standard = 0;
        public Process Proc;
        public string WindowTitle = "<unknown>";

        public ProcessDetails(Process p, IntPtr hWnd)
        {
            Proc = p;

            WindowHandle = hWnd;
            WindowTitle = "<error>";
            Tools.StartTaskAndWait(() => { WindowTitle = Native.GetWindowTitle(WindowHandle); },
                AppEnvironment.SettingValue("SlowWindowDetection", false) ? 10 : 2);
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
                    {
                        return IntPtr.Zero;
                    }

                    if (!Native.IsWindow(_WindowHandle))
                    {
                        _WindowHandle = Native.GetMainWindowForProcess(Proc);
                    }
                }
                catch
                {
                }

                return _WindowHandle;
            }
            set => _WindowHandle = value;
        }

        public bool ProcessHasExited
        {
            get
            {
                try
                {
                    if (NoAccess)
                    {
                        return false;
                    }

                    if (Proc != null)
                    {
                        return Proc.HasExited;
                    }
                }
                catch (Win32Exception)
                {
                    NoAccess = true;

                    return false; // Access is denied
                }
                catch
                {
                }

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

        private string WindowTitle_ForComparison => WindowTitle.Trim().ToLower().Replace(" ", "").Replace("_", "");

        private string BinaryName_ForComparison => BinaryName.Trim().ToLower().Replace(" ", "").Replace("_", "");

        // Detect whether or not the window needs border changes
        public bool WindowHasTargetableStyles
        {
            get
            {
                var targetable = false;
                Tools.StartTaskAndWait(() =>
                {
                    var styleCurrentWindowStandard = Native.GetWindowLong(WindowHandle, WindowLongIndex.Style);
                    var styleCurrentWindowExtended = Native.GetWindowLong(WindowHandle, WindowLongIndex.ExtendedStyle);
                    targetable = styleCurrentWindowStandard.HasTargetStyles() ||
                                 styleCurrentWindowExtended.HasExtendedStyles();
                }, AppEnvironment.SettingValue("SlowWindowDetection", false)
                    ? 10
                    : 2);
                return targetable;
            }
        }

        public override string ToString() // so that the ListView control knows how to display this object to the user
        {
            try
            {
                if (!string.IsNullOrEmpty(DescriptionOverride))
                {
                    return DescriptionOverride;
                }

                if (AppEnvironment.SettingValue("ViewAllProcessDetails", false))
                {
                    var styleCurrentWindow_standard = Native.GetWindowLong(WindowHandle, WindowLongIndex.Style);
                    var styleCurrentWindow_extended = Native.GetWindowLong(WindowHandle, WindowLongIndex.ExtendedStyle);

                    var extra_details = string.Format(" [{0:X8}.{1:X8}]", (uint) styleCurrentWindow_standard,
                        (uint) styleCurrentWindow_extended);

                    if (WindowTitle.Trim().Length == 0)
                    {
                        return BinaryName + " [#" + Proc.Id + "]" + extra_details;
                    }

                    return WindowTitle.Trim() + " [" + BinaryName + ", #" + Proc.Id + "]" + extra_details;
                }

                if (WindowTitle.Trim().Length == 0)
                {
                    return BinaryName;
                }

                var ProcessNameIsDissimilarToWindowTitle = true;
                if (WindowTitle_ForComparison.Length >= 5)
                {
                    if (BinaryName_ForComparison.Length >= 5)
                    {
                        if (BinaryName_ForComparison.Substring(0, 5) == WindowTitle_ForComparison.Substring(0, 5))
                        {
                            ProcessNameIsDissimilarToWindowTitle = false;
                        }
                    }
                }

                if (ProcessNameIsDissimilarToWindowTitle)
                {
                    return WindowTitle.Trim() + " [" + BinaryName + "]";
                }

                return WindowTitle.Trim();
            }
            catch
            {
            }

            return "<error>";
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