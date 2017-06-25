using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BorderlessGaming.Logic.System.Utilities
{
    public static class TaskUtilities
    {
        public static void StartTaskAndWait(Action target)
        {
            StartTaskAndWait(target, 0);
        }

        public static void WaitAndStartTask(Action target, int iHowLongToWait)
        {
            var ts = new CancellationTokenSource();
            var ct = ts.Token;
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(iHowLongToWait), ct);
                target();
            }, ct).Wait(ct);
        }

        public static void StartTaskAndWait(Action target, int iHowLongToWait)
        {
            try
            {
                Task.Run(async () =>
                {
                    var ts = new CancellationTokenSource();
                    var ct = ts.Token;
                    var task = Task.Factory.StartNew(target, ct);
                    var dtStartTime = DateTime.Now;
                    while (true)
                    {
                        if (task.IsCompleted || task.IsCanceled || task.IsFaulted)
                        {
                            break;
                        }
                        if (iHowLongToWait > 0)
                        {
                            if ((DateTime.Now - dtStartTime).TotalSeconds > iHowLongToWait)
                            {
                                try
                                {
                                    ts.Cancel();
                                }
                                catch
                                {
                                    // ignored
                                }
                                break;
                            }
                        }
                        await Task.Delay(15, ct);
                        //MainWindow.DoEvents();
                    }
                }).Wait();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
