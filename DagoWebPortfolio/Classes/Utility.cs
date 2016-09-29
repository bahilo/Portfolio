using QCBDManagementCommon.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DagoWebPortfolio.Classes
{
    public static class Utility
    {
        static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static string getDirectory(string directory, params string[] pathElements)
        {
            var dirElements = directory.Split('/');//.Replace("/", @"\").Split('\\');
            var allPathElements = dirElements.Concat(pathElements).ToArray();
            string path = _baseDirectory;
            foreach (string pathElement in allPathElements)
            {
                if (!string.IsNullOrEmpty(pathElement))
                    path = Path.Combine(path, pathElement);
            }

            var pathChecking = path.Split('.'); // check if it is a full path file or only directory 

            if (!File.Exists(path) && !Directory.Exists(path))
            {

                if (pathChecking.Count() > 1)
                {
                    var dir = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                else
                    Directory.CreateDirectory(path);
            }

            return Path.GetFullPath(path);
        }

        public static string getFileFullPath(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }
    }
}
