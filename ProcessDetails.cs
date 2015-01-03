using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using BorderlessGaming.Properties;

namespace BorderlessGaming
{
    public class ProcessDetails
    {
        /// <summary>
        ///     list of currently running processes
        /// </summary>
        public static List<ProcessDetails> processCache = new List<ProcessDetails>();

        public Process Proc = null;
        public string DescriptionOverride = "";
        public string WindowTitle = "";
        public string WindowClass = ""; // note: this isn't used, currently
        public IntPtr WindowHandle = IntPtr.Zero;
        public bool Manageable = false;
        public bool MadeBorderless = false;
        public WindowsAPI.WindowStyleFlags OriginalStyleFlags_Standard = 0;
        public WindowsAPI.WindowStyleFlags OriginalStyleFlags_Extended = 0;
        public Rectangle OriginalLocation = new Rectangle();

        public ProcessDetails(Process p)
        {
            this.Proc = p;

            this.WindowHandle = this.Proc.MainWindowHandle;
            this.WindowTitle = WindowsAPI.Native.GetWindowTitle(this.WindowHandle);
            this.WindowClass = WindowsAPI.Native.GetWindowClassName(this.WindowHandle); // note: this isn't used, currently
        }

        public ProcessDetails(Process p, IntPtr hWnd)
        {
            this.Proc = p;

            this.WindowHandle = hWnd;
            this.WindowTitle = WindowsAPI.Native.GetWindowTitle(this.WindowHandle);
            this.WindowClass = WindowsAPI.Native.GetWindowClassName(this.WindowHandle); // note: this isn't used, currently
        }

        public string BinaryName
        {
            get
            {
                return this.Proc.ProcessName;
            }
        }

        public override string ToString() // so that the ListView control knows how to display this object to the user
        {
            try
            {
                if (!string.IsNullOrEmpty(this.DescriptionOverride))
                    return this.DescriptionOverride;

                if (Settings.Default.ViewAllProcessDetails)
                {
                    if (this.WindowTitle.Trim().Length == 0)
                        return this.BinaryName + " [#" + this.Proc.Id.ToString() + "]";

                    return this.WindowTitle.Trim() + " [" + this.BinaryName + ", #" + this.Proc.Id.ToString() + "]";
                }

                bool ProcessNotLikeTitle = false;
                if (this.ComparisonTitle.Length < 5)
                    ProcessNotLikeTitle = true;
                else if (this.ComparisonProcess.Length < 5)
                    ProcessNotLikeTitle = true;
                else if (this.ComparisonProcess.Substring(0, 5) != this.ComparisonTitle.Substring(0, 5))
                    ProcessNotLikeTitle = true;

                if (ProcessNotLikeTitle)
                {
                    if (this.WindowTitle.Trim().Length == 0)
                        return this.BinaryName;

                    return this.WindowTitle.Trim() + " [" + this.BinaryName + "]";
                }

                if (this.WindowTitle.Trim().Length == 0)
                    return this.BinaryName;

                return this.WindowTitle.Trim();

            }
            catch { }

            return "<error>";
        }

        private string ComparisonTitle
        {
            get
            {
                return this.WindowTitle.Trim().ToLower().Replace(" ", "").Replace("_", "");
            }
        }

        private string ComparisonProcess
        {
            get
            {
                return this.BinaryName.Trim().ToLower().Replace(" ", "").Replace("_", "");
            }
        }

        public void AttemptWindowRestoration()
        {
            if (!this.MadeBorderless || this.OriginalStyleFlags_Standard == 0)
                return;

            WindowsAPI.Native.SetWindowLong(this.WindowHandle, WindowsAPI.WindowLongIndex.Style, this.OriginalStyleFlags_Standard);
            WindowsAPI.Native.SetWindowLong(this.WindowHandle, WindowsAPI.WindowLongIndex.ExtendedStyle, this.OriginalStyleFlags_Extended);
            WindowsAPI.Native.SetWindowPos(this.WindowHandle, IntPtr.Zero, this.OriginalLocation.X, this.OriginalLocation.Y, this.OriginalLocation.Width, this.OriginalLocation.Height, WindowsAPI.SetWindowPosFlags.ShowWindow | WindowsAPI.SetWindowPosFlags.NoZOrder);
            WindowsAPI.Native.SetWindowPos(this.WindowHandle, WindowsAPI.Native.HWND_NOTTOPMOST, 0, 0, 0, 0, WindowsAPI.SetWindowPosFlags.NoActivate | WindowsAPI.SetWindowPosFlags.NoMove | WindowsAPI.SetWindowPosFlags.NoSize);
            this.MadeBorderless = false;
            return;
        }

        public static implicit operator ProcessDetails(int iProcessID)
        {
            for (int i = 0; i < ProcessDetails.processCache.Count; i++)
                if (ProcessDetails.processCache[i].Proc.Id == iProcessID)
                    return ProcessDetails.processCache[i];

            return null;
        }

        public static implicit operator ProcessDetails(string sBinaryNameOfProcess)
        {
            for (int i = 0; i < ProcessDetails.processCache.Count; i++)
                if (ProcessDetails.processCache[i].Proc.ProcessName.Trim() == sBinaryNameOfProcess.Trim())
                    return ProcessDetails.processCache[i];

            return null;
        }

        public static implicit operator ProcessDetails(Process process)
        {
            for (int i = 0; i < ProcessDetails.processCache.Count; i++)
                if ((ProcessDetails.processCache[i].Proc.Id == process.Id) && (ProcessDetails.processCache[i].Proc.ProcessName == process.ProcessName))
                    return ProcessDetails.processCache[i];

            return null;
        }

        public static implicit operator ProcessDetails(IntPtr hWnd)
        {
            for (int i = 0; i < ProcessDetails.processCache.Count; i++)
                if (ProcessDetails.processCache[i].WindowHandle == hWnd)
                    return ProcessDetails.processCache[i];

            return null;
        }

        public static implicit operator IntPtr(ProcessDetails pd)
        {
            if (pd == null)
                return IntPtr.Zero;

            return pd.WindowHandle;
        }
    }
}
