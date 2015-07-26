using BorderlessGaming.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
				string windowTitle = Native.GetWindowTitle(ptr);
				//check if we already have this window in the list so we can avoid calling
				//GetWindowThreadProcessId(its costly)
				if (string.IsNullOrEmpty(windowTitle) || windowPtrSet.Contains(ptr.ToInt64()))
					continue;
				uint processId;
				Native.GetWindowThreadProcessId(ptr, out processId);
				callback(new ProcessDetails(Process.GetProcessById((int)processId), ptr)
				{
					Manageable = true
				});
			}
		}

		private static bool GetMainWindowForProcess_EnumWindows(List<IntPtr> ptrList, IntPtr hWndEnumerated, uint lParam)
		{
			WindowStyleFlags styleCurrentWindow_standard = Native.GetWindowLong(hWndEnumerated, WindowLongIndex.Style);

			if (lParam == 0) // strict: windows that are visible and have a border
			{
				if (Native.IsWindowVisible(hWndEnumerated))
				{
					if
					(
						   ((styleCurrentWindow_standard & WindowStyleFlags.Caption) > 0)
						&& (
								((styleCurrentWindow_standard & WindowStyleFlags.Border) > 0)
							 || ((styleCurrentWindow_standard & WindowStyleFlags.ThickFrame) > 0)
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
					if (((uint)styleCurrentWindow_standard) != 0)
					{
						ptrList.Add(hWndEnumerated);
					}
				}
			}
			return true;
		}
	}
}