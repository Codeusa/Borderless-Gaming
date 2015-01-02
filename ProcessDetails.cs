using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using BorderlessGaming.Properties;

namespace BorderlessGaming
{
    public class ProcessDetails
    {
        public string ID = "";
        public string DescriptionOverride = "";
        public string BinaryName = "";
        public string WindowTitle = "";
        public string WindowClass = ""; // note: this isn't used, currently
        public IntPtr WindowHandle = IntPtr.Zero;
        public bool Manageable = false;
        public bool MadeBorderless = false;
        public WindowsApi.WindowStyleFlags OriginalStyleFlags_Standard = 0;
        public WindowsApi.WindowStyleFlags OriginalStyleFlags_Extended = 0;
        public Rectangle OriginalLocation = new Rectangle();
        
        public override string ToString() // so that the ListView control knows how to display this object to the user
        {
            try
            {
                if (!string.IsNullOrEmpty(this.DescriptionOverride))
                    return this.DescriptionOverride;

                if (Settings.Default.ViewAllProcessDetails)
                {
                    if (this.WindowTitle.Trim().Length == 0)
                        return this.BinaryName + " [#" + this.ID + "]";

                    return this.WindowTitle.Trim() + " [" + this.BinaryName + ", #" + this.ID + "]";
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

            WindowsApi.Native.SetWindowLong(this.WindowHandle, WindowsApi.WindowLongIndex.Style, this.OriginalStyleFlags_Standard);
            WindowsApi.Native.SetWindowLong(this.WindowHandle, WindowsApi.WindowLongIndex.ExtendedStyle, this.OriginalStyleFlags_Extended);
            WindowsApi.Native.SetWindowPos(this.WindowHandle, IntPtr.Zero, this.OriginalLocation.X, this.OriginalLocation.Y, this.OriginalLocation.Width, this.OriginalLocation.Height, WindowsApi.SetWindowPosFlags.ShowWindow | WindowsApi.SetWindowPosFlags.NoZOrder);
            WindowsApi.Native.SetWindowPos(this.WindowHandle, WindowsApi.Native.HWND_NOTTOPMOST, 0, 0, 0, 0, WindowsApi.SetWindowPosFlags.NoActivate | WindowsApi.SetWindowPosFlags.NoMove | WindowsApi.SetWindowPosFlags.NoSize);
            this.MadeBorderless = false;
            return;
        }
    }
}
