using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorderlessGaming.Logic.Models
{
    public sealed class ProcessDetailsList : ObservableCollection<ProcessDetails>
    {
        private readonly HashSet<string> _processTitleSet;

        public ProcessDetailsList()
        {
            WindowPtrSet = new HashSet<long>();
            WindowTitleSet = new HashSet<string>();
            _processTitleSet = new HashSet<string>();
            CollectionChanged += OnCollectionChanged;
        }

        //public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public HashSet<long> WindowPtrSet { get; }

        public HashSet<string> WindowTitleSet { get; }

        public HashSet<string> ProcessTitleSet => WindowTitleSet;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var newItems = e.NewItems.Cast<ProcessDetails>();
                foreach (var ni in newItems)
                {
                    WindowPtrSet.Add(ni.WindowHandle.ToInt64());
                    WindowTitleSet.Add(ni.WindowTitle);
                    _processTitleSet.Add(ni.Proc.ProcessName);
                }
            }
            if (e.OldItems != null)
            {
                var oldItems = e.OldItems.Cast<ProcessDetails>();
                foreach (var oi in oldItems)
                {
                    WindowPtrSet.Remove(oi.WindowHandle.ToInt64());
                    WindowTitleSet.Remove(oi.WindowTitle);
                    _processTitleSet.Remove(oi.Proc.ProcessName);
                }
            }
        }

        /// <summary>
        ///     Clear isnt firing the collectionchanged event so i made my own implementation
        /// </summary>
        public void ClearProcesses()
        {
            var copy = this.ToList();
            foreach (var pd in copy)
            {
                Remove(pd);
            }
        }

        public ProcessDetails FromHandle(IntPtr hCurrentActiveWindow)
        {
            return this.FirstOrDefault(pd => pd.WindowHandle == hCurrentActiveWindow);
        }
    }
}
