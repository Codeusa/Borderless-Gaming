using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using BorderlessGaming.Logic.System.Utilities;
using BorderlessGaming.Logic.Windows;

namespace BorderlessGaming.Logic.Models
{
    public class ProcessDetails
    {
        //public string WindowClass = ""; // note: this isn't used, currently
        private IntPtr _windowHandle = IntPtr.Zero;

        public string DescriptionOverride = "";
        public bool MadeBorderless = false;
        public int MadeBorderlessAttempts = 0;
        public bool Manageable = false;
        public bool NoAccess;
        public Rectangle OriginalLocation = new Rectangle();
        public WindowStyleFlags OriginalStyleFlagsExtended = 0;
        public WindowStyleFlags OriginalStyleFlagsStandard = 0;
        public Process Proc;
        public string WindowTitle = "<unknown>";

        public ProcessDetails(Process p, IntPtr hWnd)
        {
            Proc = p;

            WindowHandle = hWnd;
            WindowTitle = Native.GetWindowTitle(WindowHandle);
          //  GetWindowTitle();

            //this.WindowClass = WindowsAPI.Native.GetWindowClassName(this.WindowHandle); // note: this isn't used, currently
        }

        private async void GetWindowTitle()
        {
           await TaskUtilities.StartTaskAndWait(() => { WindowTitle = Native.GetWindowTitle(WindowHandle); },
                Config.Instance.AppSettings.SlowWindowDetection ? 10 : 2);
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

                    if (!Native.IsWindow(_windowHandle))
                    {
                        _windowHandle = Native.GetMainWindowForProcess(Proc).GetAwaiter().GetResult();
                    }
                }
                catch
                {
                }

                return _windowHandle;
            }
            set => _windowHandle = value;
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

        private string WindowTitleForComparison => WindowTitle.Trim().ToLower().Replace(" ", "").Replace("_", "");

        private string BinaryNameForComparison => BinaryName.Trim().ToLower().Replace(" ", "").Replace("_", "");

        // Detect whether or not the window needs border changes
        public async Task<bool> WindowHasTargetableStyles()
        {
            var targetable = false;
            await TaskUtilities.StartTaskAndWait(() =>
             {
                 var styleCurrentWindowStandard = Native.GetWindowLong(WindowHandle, WindowLongIndex.Style);
                 var styleCurrentWindowExtended = Native.GetWindowLong(WindowHandle, WindowLongIndex.ExtendedStyle);
                 targetable = styleCurrentWindowStandard.HasTargetStyles() || styleCurrentWindowExtended.HasExtendedStyles();
             }, Config.Instance.AppSettings.SlowWindowDetection ? 10 : 2);
            return targetable;
        }

        public override string ToString() // so that the ListView control knows how to display this object to the user
        {
            try
            {
                if (!string.IsNullOrEmpty(DescriptionOverride))
                {
                    return DescriptionOverride;
                }

                if (Config.Instance.AppSettings.ViewAllProcessDetails)
                {
                    var styleCurrentWindowStandard = Native.GetWindowLong(WindowHandle, WindowLongIndex.Style);
                    var styleCurrentWindowExtended = Native.GetWindowLong(WindowHandle, WindowLongIndex.ExtendedStyle);

                    var extraDetails =
                        $" [{(uint) styleCurrentWindowStandard:X8}.{(uint) styleCurrentWindowExtended:X8}]";
                    return string.IsNullOrWhiteSpace(WindowTitle.Trim())
                        ? $"{BinaryName} [#{Proc.Id}]{extraDetails}"
                        : $"{WindowTitle.Trim()} [{BinaryName}, #{Proc.Id}]{extraDetails}";
                }

                if (string.IsNullOrWhiteSpace(WindowTitle.Trim()))
                {
                    return BinaryName;
                }

                var processNameIsDissimilarToWindowTitle = true;
                if (WindowTitleForComparison.Length >= 5)
                {
                    if (BinaryNameForComparison.Length >= 5)
                    {
                        if (BinaryNameForComparison.Substring(0, 5) == WindowTitleForComparison.Substring(0, 5))
                        {
                            processNameIsDissimilarToWindowTitle = false;
                        }
                    }
                }

                return processNameIsDissimilarToWindowTitle
                    ? $"{WindowTitle.Trim()} [{BinaryName}]"
                    : WindowTitle.Trim();
            }
            catch
            {
                // ignored
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