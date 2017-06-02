using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using BorderlessGaming.Properties;

namespace BorderlessGaming.Utilities
{
    public static class Tools
    {
        // A sort of nullable boolean
        public enum Boolstate
        {
            True,
            False,
            Indeterminate
        }

        public static void GotoSite(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch { }
        }

        /// <summary>
        ///     Gets the smallest Rectangle containing two input Rectangles
        /// </summary>
        public static Rectangle GetContainingRectangle(Rectangle a, Rectangle b)
        {
            Point amin = new Point(a.X, a.Y);
            Point amax = new Point(a.X + a.Width, a.Y + a.Height);
            Point bmin = new Point(b.X, b.Y);
            Point bmax = new Point(b.X + b.Width, b.Y + b.Height);
            Point nmin = new Point(0, 0);
            Point nmax = new Point(0, 0);

            nmin.X = (amin.X < bmin.X) ? amin.X : bmin.X;
            nmin.Y = (amin.Y < bmin.Y) ? amin.Y : bmin.Y;
            nmax.X = (amax.X > bmax.X) ? amax.X : bmax.X;
            nmax.Y = (amax.Y > bmax.Y) ? amax.Y : bmax.Y;

            return new Rectangle(nmin, new Size(nmax.X - nmin.X, nmax.Y - nmin.Y));
        }

        public static string Input_Text(string sTitle, string sInstructions, string sDefaultValue = "")
        {
            try
            {
                using (Forms.InputText inputForm = new Forms.InputText())
                {
                    inputForm.Title = sTitle;
                    inputForm.Instructions = sInstructions;
                    inputForm.Input = sDefaultValue;
                    
                    if (inputForm.ShowDialog() == DialogResult.OK)
                        return inputForm.Input;

                    return sDefaultValue;
                }
            }
            catch { }

            return string.Empty;
        }

        public static List<string> StartupParameters
        {
            get
            {
                try
                {
                    List<string> startup_parameters_mixed = new List<string>();
                    startup_parameters_mixed.AddRange(Environment.GetCommandLineArgs());

                    List<string> startup_parameters_lower = new List<string>();
                    foreach (string s in startup_parameters_mixed)
                        startup_parameters_lower.Add(s.Trim().ToLower());

                    startup_parameters_mixed.Clear();

                    return startup_parameters_lower;
                }
                catch
                {
                    try { return new List<string>(Environment.GetCommandLineArgs()); } catch { }
                }

                return new List<string>();
            }
        }

		public static string GetDataPath()
		{

			try
			{
				// No version!
				return Environment.GetEnvironmentVariable("AppData").Trim() + "\\" + Application.CompanyName + "\\" + Application.ProductName;
			}
			catch { }

			try
			{
				// Version, but chopped out
				return Application.UserAppDataPath.Substring(0, Application.UserAppDataPath.LastIndexOf("\\"));
			}
			catch
			{
				try
				{
					// App launch folder
					return Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"));
				}
				catch
				{
					try
					{
						// Current working folder
						return Environment.CurrentDirectory;
					}
					catch
					{
						try
						{
							// Desktop
							return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
						}
						catch
						{
							// Also current working folder
							return ".";
						}
					}
				}
			}
		}


        public static void StartMethodMultithreadedAndWait(Action target)
        {
            StartMethodMultithreadedAndWait(target, 0);
        }

        public static void StartMethodMultithreadedAndWait(Action target, int iHowLongToWait)
        {
            try
            {
                var tsGenericMethod = new ThreadStart(() => { try { target(); } catch { } });
                var trdGenericThread = new Thread(tsGenericMethod) {IsBackground = true};
                trdGenericThread.Start();

                var dtStartTime = DateTime.Now;

                for (;;)
                {
                    if (iHowLongToWait > 0)
                    {
                        if ((DateTime.Now - dtStartTime).TotalSeconds > iHowLongToWait)
                        {
                            try { trdGenericThread.Abort(); } catch { }
                            break;
                        }
                    }

                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.Stopped) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.StopRequested) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.Aborted) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.AbortRequested) break;

                    Thread.Sleep(15);
                    Forms.MainWindow.DoEvents();
                }
            }
            catch
            {
                // ignored
            }
        }

        public static void StartMethodMultithreadedAndWait(Action target, double dHowLongToWait)
        {
            try
            {
                ThreadStart tsGenericMethod = new ThreadStart(() => { try { target(); } catch { } });
                Thread trdGenericThread = new Thread(tsGenericMethod);
                trdGenericThread.IsBackground = true;
                trdGenericThread.Start();

                DateTime dtStartTime = DateTime.Now;

                for (; ; )
                {
                    if (dHowLongToWait > 0.0)
                    {
                        if ((DateTime.Now - dtStartTime).TotalMilliseconds > dHowLongToWait)
                        {
                            try { trdGenericThread.Abort(); } catch { }
                            break;
                        }
                    }

                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.Stopped) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.StopRequested) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.Aborted) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.AbortRequested) break;

                    Thread.Sleep(15);
                    Forms.MainWindow.DoEvents();
                }
            }
            catch { }
        }

        public static void StartMethodAndWait(Action target, double dHowLongToWait)
        {
            try
            {
                ThreadStart tsGenericMethod = new ThreadStart(() => { try { target(); } catch { } });
                Thread trdGenericThread = new Thread(tsGenericMethod);
                trdGenericThread.IsBackground = false;
                trdGenericThread.Start();

                DateTime dtStartTime = DateTime.Now;

                for (; ; )
                {
                    if (dHowLongToWait > 0.0)
                    {
                        if ((DateTime.Now - dtStartTime).TotalMilliseconds > dHowLongToWait)
                        {
                            try { trdGenericThread.Abort(); } catch { }
                            break;
                        }
                    }

                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.Stopped) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.StopRequested) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.Aborted) break;
                    if (trdGenericThread.ThreadState == System.Threading.ThreadState.AbortRequested) break;

                    Thread.Sleep(15);
                }
            }
            catch { }
        }

        public static void StartMethodMultithreaded(Action target)
        {
            ThreadStart tsGenericMethod = new ThreadStart(() => { try { target(); } catch { } });
            Thread trdGenericThread = new Thread(tsGenericMethod);
            trdGenericThread.IsBackground = true;
            trdGenericThread.Start();
        }
    }
}