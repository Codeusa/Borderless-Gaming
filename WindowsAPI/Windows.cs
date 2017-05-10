using BorderlessGaming.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BorderlessGaming.WindowsAPI;

namespace BorderlessGaming.WindowsAPI
{
	public class Windows
	{

		/// <summary>
		/// Query the windows
		/// </summary>
		/// <param name="callback">A callback that's called when a new window is found. This way the functionality is the same as before</param>
		/// <param name="windowPtrSet">A set of current window ptrs</param>
		public void QueryProcessesWithWindows(Action<ProcessDetails> callback, HashSet<long> windowPtrSet)
		{
			var ptrList = new List<IntPtr>();
			Native.EnumWindows_CallBackProc del = (hwnd, lParam) => GetMainWindowForProcess_EnumWindows(ptrList, hwnd, lParam);
			Native.EnumWindows(del, 0);
			Native.EnumWindows(del, 1);
		    
		    foreach (var ptr in ptrList)
			{

			    if (Native.GetWindowRect(ptr, out Native.Rect rect))
			    {
			        if (((Rectangle)rect).IsEmpty)
			        {
			            continue;
			        }
                    //check if we already have this window in the list so we can avoid calling
                    //GetWindowThreadProcessId(its costly)
                    if (windowPtrSet.Contains(ptr.ToInt64()))
                        continue;
                    uint processId;
                    Native.GetWindowThreadProcessId(ptr, out processId);
                    callback(new ProcessDetails(Process.GetProcessById((int)processId), ptr)
                    {
                        Manageable = true
                    });
                }
			}
		}

		private static bool GetMainWindowForProcess_EnumWindows(List<IntPtr> ptrList, IntPtr hWndEnumerated, uint lParam)
		{
			WindowStyleFlags styleCurrentWindowStandard = Native.GetWindowLong(hWndEnumerated, WindowLongIndex.Style);

			if (lParam == 0) // strict: windows that are visible and have a border
			{
				if (Native.IsWindowVisible(hWndEnumerated))
				{
					if
					(
						   ((styleCurrentWindowStandard & WindowStyleFlags.Caption) > 0)
						&& (
								((styleCurrentWindowStandard & WindowStyleFlags.Border) > 0)
							 || ((styleCurrentWindowStandard & WindowStyleFlags.ThickFrame) > 0)
						   )
					)
					{
						ptrList.Add(hWndEnumerated);
					}
				}
			}
			else if (lParam == 1) // loose: windows that are visible
			{
				if (Native.IsWindowVisible(hWndEnumerated))
				{
					if (((uint)styleCurrentWindowStandard) != 0)
					{
						ptrList.Add(hWndEnumerated);
					}
				}
			}
			return true;
		}
		
	}
}