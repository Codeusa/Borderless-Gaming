using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace BorderlessGaming.Common
{
	public class ProcessDetailsList : ObservableCollection<ProcessDetails>
	{
		private readonly HashSet<long> _windowPtrSet;
		private readonly HashSet<string> _windowTitleSet;
		private readonly HashSet<string> _processTitleSet;

		//public override event NotifyCollectionChangedEventHandler CollectionChanged;

		public HashSet<long> WindowPtrSet => _windowPtrSet;
	    public HashSet<string> WindowTitleSet => _windowTitleSet;
	    public HashSet<string> ProcessTitleSet => _windowTitleSet;

	    public ProcessDetailsList()
		{
			_windowPtrSet = new HashSet<long>();
			_windowTitleSet = new HashSet<string>();
			_processTitleSet = new HashSet<string>();
			CollectionChanged += OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				var newItems = e.NewItems.Cast<ProcessDetails>();
				foreach (var ni in newItems)
				{
					_windowPtrSet.Add(ni.WindowHandle.ToInt64());
					_windowTitleSet.Add(ni.WindowTitle);
					_processTitleSet.Add(ni.Proc.ProcessName);
				}
			}
			if (e.OldItems != null)
			{
				var oldItems = e.OldItems.Cast<ProcessDetails>();
				foreach (var oi in oldItems)
				{
					_windowPtrSet.Remove(oi.WindowHandle.ToInt64());
					_windowTitleSet.Remove(oi.WindowTitle);
					_processTitleSet.Remove(oi.Proc.ProcessName);
				}
			}
		}

		/// <summary>
		/// Clear isnt firing the collectionchanged event so i made my own implementation
		/// </summary>
		public void ClearProcesses()
		{
			var copy = this.ToList();
			foreach (var pd in copy)
				Remove(pd);
        }

		internal ProcessDetails FromHandle(IntPtr hCurrentActiveWindow)
		{
			return this.FirstOrDefault(pd => pd.WindowHandle == hCurrentActiveWindow);
		}
	}
}
