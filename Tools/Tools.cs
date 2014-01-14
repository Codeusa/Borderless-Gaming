using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BorderlessGaming.Tools
{
    public static class Tools
    {
        private static void CheckLastError(Expression<Func<bool>> assertionExpression)
        {
            if (!assertionExpression.Compile()())
            {
                var nativeException = new Win32Exception(Marshal.GetLastWin32Error());
                throw new Exception(string.Format("Assertion Failed: {0}\r\nGetLastError: {1}", assertionExpression.Body, nativeException.Message), nativeException);
            }
        }
    }
}
