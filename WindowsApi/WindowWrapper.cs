using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace BorderlessGaming.WindowsApi
{
    public class Window
    {
        public IntPtr WindowHandle { get; set; }

        public string Title
        {
            get
            {
                var stringBuilder = new StringBuilder(250);
                Native.GetWindowText(WindowHandle, stringBuilder, 250);
                return stringBuilder.ToString();
            }

            set
            {
                if (!Native.SetWindowText(WindowHandle, value))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        public string Module { get; set; }

        public WindowStyleFlags StyleAttributes
        {
            get { return Native.GetWindowLong(WindowHandle, WindowLongIndex.Style); }
            set { Native.SetWindowLong(WindowHandle, WindowLongIndex.Style, value); }
        }

        public WindowStyleFlags ExtendedAttributes
        {
            get { return Native.GetWindowLong(WindowHandle, WindowLongIndex.ExtendedStyle); }
            set { Native.SetWindowLong(WindowHandle, WindowLongIndex.ExtendedStyle, value); }
        }

        public WindowStyleFlags OriginalStyleAttributes { get; set; }

        public WindowStyleFlags OriginalExtendedAttributes { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    public struct Attribute
    {
        public string Name { get; set; }

        public uint Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}