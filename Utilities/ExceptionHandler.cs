using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BorderlessGaming.Utilities
{
    public static class ExceptionHandler
    {
        public static void AddHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var filePath = string.Format("./UnhandledException{0}.json", DateTime.Now.ToShortDateString());
                File.WriteAllText(filePath, JsonConvert.SerializeObject(args.ExceptionObject));
                MessageBox.Show(string.Format("An Unhandled Exception was Caught and Logged to:\r\n{0}", filePath), "Exception Caught");
            };


            Application.ThreadException += (sender, args) =>
            {
                var filePath = string.Format("./ThreadException{0}.json", DateTime.Now.ToShortDateString());
                File.WriteAllText(string.Format("./ThreadException{0}.json", DateTime.Now.ToShortDateString()), JsonConvert.SerializeObject(args.Exception));
                MessageBox.Show(string.Format("An Unhandled Thread Exception was Caught and Logged to:\r\n{0}", filePath), "Thread Exception Caught");
            };
        }
    }
}
