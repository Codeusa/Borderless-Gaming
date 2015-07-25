using BorderlessGaming.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BorderlessGaming.WindowsAPI.Native;

namespace BorderlessGaming.WindowsAPI
{
	public class Windows
	{
		private readonly LimitedConcurrencyLevelTaskScheduler _scheduler;

		public Windows()
		{
			//init stuff
			_scheduler = new LimitedConcurrencyLevelTaskScheduler(1);
		}
				
		/// <summary>
		/// Query the windows
		/// </summary>
		/// <param name="callback">A callback that's called when a new window is found. This way the functionality is the same as before</param>
		/// <param name="windowPtrSet"></param>
		public void QueryProcessesWithWindows(Action<ProcessDetails> callback, HashSet<long> windowPtrSet, HashSet<string> windowTitleSet)
		{
			var ptrList = new List<IntPtr>();
			EnumWindows_CallBackProc del = (hwnd, lParam) => GetMainWindowForProcess_EnumWindows(ptrList, hwnd, lParam);
            EnumWindows(del, 0);
			EnumWindows(del, 1);
			var taskList = new List<Task>();
			foreach (var ptr in ptrList)
			{
				string windowTitle = GetWindowTitle(ptr);
				if (string.IsNullOrEmpty(windowTitle) || windowPtrSet.Contains(ptr.ToInt64()) || windowTitleSet.Contains(windowTitle))
					continue;
				taskList.Add(Task.Factory.StartNew(() =>
				{
					uint processId;
					GetWindowThreadProcessId(ptr, out processId);
					callback(new ProcessDetails(Process.GetProcessById((int)processId), ptr)
					{
						Manageable = true
					});
				}, CancellationToken.None, TaskCreationOptions.PreferFairness, _scheduler));
			}
			Task.WaitAll(taskList.ToArray());
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


	// Provides a task scheduler that ensures a maximum concurrency level while  
	// running on top of the thread pool. 
	public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
	{
		// Indicates whether the current thread is processing work items.
		[ThreadStatic]
		private static bool _currentThreadIsProcessingItems;

		// The list of tasks to be executed  
		private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 

		// The maximum concurrency level allowed by this scheduler.  
		private readonly int _maxDegreeOfParallelism;

		// Indicates whether the scheduler is currently processing work items.  
		private int _delegatesQueuedOrRunning = 0;

		// Creates a new instance with the specified degree of parallelism.  
		public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
		{
			if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
			_maxDegreeOfParallelism = maxDegreeOfParallelism;
		}

		// Queues a task to the scheduler.  
		protected sealed override void QueueTask(Task task)
		{
			// Add the task to the list of tasks to be processed.  If there aren't enough  
			// delegates currently queued or running to process tasks, schedule another.  
			lock (_tasks)
			{
				_tasks.AddLast(task);
				if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
				{
					++_delegatesQueuedOrRunning;
					NotifyThreadPoolOfPendingWork();
				}
			}
		}

		// Inform the ThreadPool that there's work to be executed for this scheduler.  
		private void NotifyThreadPoolOfPendingWork()
		{
			ThreadPool.UnsafeQueueUserWorkItem(_ =>
			{
				// Note that the current thread is now processing work items. 
				// This is necessary to enable inlining of tasks into this thread.
				_currentThreadIsProcessingItems = true;
				try
				{
					// Process all available items in the queue. 
					while (true)
					{
						Task item;
						lock (_tasks)
						{
							// When there are no more items to be processed, 
							// note that we're done processing, and get out. 
							if (_tasks.Count == 0)
							{
								--_delegatesQueuedOrRunning;
								break;
							}

							// Get the next item from the queue
							item = _tasks.First.Value;
							_tasks.RemoveFirst();
						}

						// Execute the task we pulled out of the queue 
						base.TryExecuteTask(item);
					}
				}
				// We're done processing items on the current thread 
				finally { _currentThreadIsProcessingItems = false; }
			}, null);
		}

		// Attempts to execute the specified task on the current thread.  
		protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			// If this thread isn't already processing a task, we don't support inlining 
			if (!_currentThreadIsProcessingItems) return false;

			// If the task was previously queued, remove it from the queue 
			if (taskWasPreviouslyQueued)
				// Try to run the task.  
				if (TryDequeue(task))
					return base.TryExecuteTask(task);
				else
					return false;
			else
				return base.TryExecuteTask(task);
		}

		// Attempt to remove a previously scheduled task from the scheduler.  
		protected sealed override bool TryDequeue(Task task)
		{
			lock (_tasks) return _tasks.Remove(task);
		}

		// Gets the maximum concurrency level supported by this scheduler.  
		public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

		// Gets an enumerable of the tasks currently scheduled on this scheduler.  
		protected sealed override IEnumerable<Task> GetScheduledTasks()
		{
			bool lockTaken = false;
			try
			{
				Monitor.TryEnter(_tasks, ref lockTaken);
				if (lockTaken) return _tasks;
				else throw new NotSupportedException();
			}
			finally
			{
				if (lockTaken) Monitor.Exit(_tasks);
			}
		}
	}
}