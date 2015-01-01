using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BorderlessGaming
{
    public class ProcessDetails
    {
        public string description_override = "";
        public string process_id = "";
        public string process_binary = "";
        public string window_title = "";
        public string window_class = ""; // note: this isn't used, currently
        public IntPtr window_handle = IntPtr.Zero;
        public bool hidable = false;
        public bool borderless = false;

        public override string ToString() // so that the ListView control knows how to display this object to the user
        {
            try
            {
                if (!string.IsNullOrEmpty(this.description_override))
                    return this.description_override;

                if (this.window_title.Trim().Length == 0)
                    return this.process_binary + " [" + this.process_binary + ", #" + this.process_id + "]";

                return this.window_title.Trim() + " [" + this.process_binary + ", #" + this.process_id + "]";
            }
            catch { }

            return "<error>";
        }

        public WindowsApi.WindowStyleFlags original_style_flags_standard = 0;
        public WindowsApi.WindowStyleFlags original_style_flags_extended = 0;
        public Rectangle original_location = new Rectangle();

        public void Restore()
        {
            if (!this.borderless)
                return;

            WindowsApi.Native.SetWindowLong(this.window_handle, WindowsApi.WindowLongIndex.Style, this.original_style_flags_standard);
            WindowsApi.Native.SetWindowLong(this.window_handle, WindowsApi.WindowLongIndex.ExtendedStyle, this.original_style_flags_extended);
            WindowsApi.Native.SetWindowPos(this.window_handle, IntPtr.Zero, this.original_location.X, this.original_location.Y, this.original_location.Width, this.original_location.Height, WindowsApi.SetWindowPosFlags.ShowWindow | WindowsApi.SetWindowPosFlags.NoZOrder);
            this.borderless = false;
        }
    }
}
