using BorderlessGaming.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BorderlessGaming.WindowsAPI.Native;

namespace BorderlessGaming.WindowsAPI
{
	public class Windows
	{

		public Windows()
		{
			//init stuff
		}

		public Task<List<ProcessDetails>> GetAsync()
		{
			return Task.Factory.StartNew(Get);
		}

		public List<ProcessDetails> Get()
		{
			var pdList = new List<ProcessDetails>();
			var ptrList = new List<IntPtr>();
			EnumWindows_CallBackProc del = (hwnd, lParam) => GetMainWindowForProcess_EnumWindows(ptrList, hwnd, lParam);
            EnumWindows(del, 0);
			EnumWindows(del, 1);
			foreach (var ptr in ptrList)
			{
				uint processId;
				GetWindowThreadProcessId(ptr, out processId);
				pdList.Add(new ProcessDetails(Process.GetProcessById((int)processId), ptr));
			}
			return pdList;
		}

		private static bool GetMainWindowForProcess_EnumWindows(List<IntPtr> ptrList, IntPtr hWndEnumerated, uint lParam)
		{
			WindowStyleFlags styleCurrentWindow_standard = GetWindowLong(hWndEnumerated, WindowLongIndex.Style);

			if (lParam == 0) // strict: windows that are visible and have a border
			{
				if (IsWindowVisible(hWndEnumerated))
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
				if (IsWindowVisible(hWndEnumerated))
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