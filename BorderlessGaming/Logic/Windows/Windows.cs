using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using BorderlessGaming.Logic.Models;

namespace BorderlessGaming.Logic.Windows
{
    public class Windows
    {
        /// <summary>
        ///     Query the windows
        /// </summary>
        /// <param name="callback">
        ///     A callback that's called when a new window is found. This way the functionality is the same as
        ///     before
        /// </param>
        /// <param name="windowPtrSet">A set of current window ptrs</param>
        public void QueryProcessesWithWindows(Action<ProcessDetails> callback, HashSet<long> windowPtrSet)
        {
            var ptrList = new List<IntPtr>();

            bool Del(IntPtr hwnd, uint lParam)
            {
                return GetMainWindowForProcess_EnumWindows(ptrList, hwnd, lParam);
            }

            Native.EnumWindows(Del, 0);
            Native.EnumWindows(Del, 1);
            foreach (var ptr in ptrList)
            {
                if (Native.GetWindowRect(ptr, out Native.Rect rect))
                {
                    if (((Rectangle) rect).IsEmpty)
                    {
                        continue;
                    }
                    //check if we already have this window in the list so we can avoid calling
                    //GetWindowThreadProcessId(its costly)
                    if (windowPtrSet.Contains(ptr.ToInt64()))
                    {
                        continue;
                    }
                    uint processId;
                    Native.GetWindowThreadProcessId(ptr, out processId);
                    callback(new ProcessDetails(Process.GetProcessById((int) processId), ptr)
                    {
                        Manageable = true
                    });
                }
            }
        }

        private static bool GetMainWindowForProcess_EnumWindows(List<IntPtr> ptrList, IntPtr hWndEnumerated,
            uint lParam)
        {
            var styleCurrentWindowStandard = Native.GetWindowLong(hWndEnumerated, WindowLongIndex.Style);

            switch (lParam)
            {
                case 0:
                    if (Native.IsWindowVisible(hWndEnumerated))
                    {
                        if
                        (
                            (styleCurrentWindowStandard & WindowStyleFlags.Caption) > 0
                            && (
                                (styleCurrentWindowStandard & WindowStyleFlags.Border) > 0
                                || (styleCurrentWindowStandard & WindowStyleFlags.ThickFrame) > 0
                            )
                        )
                        {
                            ptrList.Add(hWndEnumerated);
                        }
                    }
                    break;
                case 1:
                    if (Native.IsWindowVisible(hWndEnumerated))
                    {
                        if ((uint) styleCurrentWindowStandard != 0)
                        {
                            ptrList.Add(hWndEnumerated);
                        }
                    }
                    break;
            }
            return true;
        }
    }
}