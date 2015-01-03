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
        /// <summary>
        ///     list of currently running processes
        /// </summary>
        public static List<ProcessDetails> List = new List<ProcessDetails>();

        public Process Proc = null;
        public string DescriptionOverride = "";
        public string WindowTitle = "";
        //public string WindowClass = ""; // note: this isn't used, currently
        public IntPtr WindowHandle = IntPtr.Zero;
        public bool Manageable = false;
        public bool MadeBorderless = false;
        public WindowsAPI.WindowStyleFlags OriginalStyleFlags_Standard = 0;
        public WindowsAPI.WindowStyleFlags OriginalStyleFlags_Extended = 0;
        public Rectangle OriginalLocation = new Rectangle();

        // Code commented (but not removed) by psouza4 2015/01/02: there were no references to this method, so no need to compile it and bloat the software.
        //public ProcessDetails(Process p)
        //{
        //    this.Proc = p;

        //    this.WindowHandle = this.Proc.MainWindowHandle;
        //    this.WindowTitle = WindowsAPI.Native.GetWindowTitle(this.WindowHandle);
        //    //this.WindowClass = WindowsAPI.Native.GetWindowClassName(this.WindowHandle); // note: this isn't used, currently
        //}

        public ProcessDetails(Process p, IntPtr hWnd)
        {
            this.Proc = p;

            this.WindowHandle = hWnd;
            this.WindowTitle = WindowsAPI.Native.GetWindowTitle(this.WindowHandle);
            //this.WindowClass = WindowsAPI.Native.GetWindowClassName(this.WindowHandle); // note: this isn't used, currently
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

                if (AppEnvironment.SettingValue("ViewAllProcessDetails", false))
                {
                    if (this.WindowTitle.Trim().Length == 0)
                        return this.BinaryName + " [#" + this.Proc.Id.ToString() + "]";

                    return this.WindowTitle.Trim() + " [" + this.BinaryName + ", #" + this.Proc.Id.ToString() + "]";
                }

                if (this.WindowTitle.Trim().Length == 0)
                    return this.BinaryName;

                bool ProcessNameIsDissimilarToWindowTitle = false;
                if (this.WindowTitle_ForComparison.Length < 5)
                    ProcessNameIsDissimilarToWindowTitle = true;
                else if (this.BinaryName_ForComparison.Length < 5)
                    ProcessNameIsDissimilarToWindowTitle = true;
                else if (this.BinaryName_ForComparison.Substring(0, 5) != this.WindowTitle_ForComparison.Substring(0, 5))
                    ProcessNameIsDissimilarToWindowTitle = true;

                if (ProcessNameIsDissimilarToWindowTitle)
                    return this.WindowTitle.Trim() + " [" + this.BinaryName + "]";

                return this.WindowTitle.Trim();

            }
            catch { }

            return "<error>";
        }

        private string WindowTitle_ForComparison
        {
            get
            {
                return this.WindowTitle.Trim().ToLower().Replace(" ", "").Replace("_", "");
            }
        }

        private string BinaryName_ForComparison
        {
            get
            {
                return this.BinaryName.Trim().ToLower().Replace(" ", "").Replace("_", "");
            }
        }

        public static implicit operator ProcessDetails(Process process)
        {
            for (int i = 0; i < ProcessDetails.List.Count; i++)
                if ((ProcessDetails.List[i].Proc.Id == process.Id) && (ProcessDetails.List[i].Proc.ProcessName == process.ProcessName))
                    return ProcessDetails.List[i];

            return null;
        }

        public static implicit operator ProcessDetails(IntPtr hWnd)
        {
            for (int i = 0; i < ProcessDetails.List.Count; i++)
                if (ProcessDetails.List[i].WindowHandle == hWnd)
                    return ProcessDetails.List[i];

            return null;
        }

        public static implicit operator Process(ProcessDetails pd)
        {
            if (pd == null)
                return null;

            return pd.Proc;
        }

        public static implicit operator IntPtr(ProcessDetails pd)
        {
            if (pd == null)
                return IntPtr.Zero;

            return pd.WindowHandle;
        }
    }
}
