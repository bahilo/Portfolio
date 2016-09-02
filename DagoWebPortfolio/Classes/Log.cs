using DagoWebPortfolio.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Classes
{
    public static class Log
    {      
        public static void write(string message, string messageType, [CallerMemberName] string callerName = null)
        {
            string fileName = "log_" + DateTime.Now.ToString("yyyy_MM") + ".txt";
            string directory = Utility.getDirectory("bin", "Logs");// Path.Combine(_baseDirectory, "bin", "Logs");
            string fileFullPath = Utility.getFileFullPath(directory, fileName);// Path.Combine(_directory, _fileName);

            File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), messageType, callerName, message) });
        }
    }
}
