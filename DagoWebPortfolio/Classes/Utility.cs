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

        public static string getDirectory(string directory,  params string[] pathElements)
        {
            string path = Path.Combine(_baseDirectory, directory);
            foreach (string pathElement in pathElements)
            {
                path = Path.Combine(path, pathElement);
            }

            string pathBase = Path.GetDirectoryName(path);

            if (!Directory.Exists(pathBase))
                Directory.CreateDirectory(pathBase);

            return pathBase;
        }

        public static string getFileFullPath(string directory, string fileName)
        {
            return Path.Combine(_baseDirectory, directory, fileName);
        }
    }
}
