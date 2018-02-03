using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Utils
{
    public static class Logger
    {
        public static void Log(string fileName, string text, bool newLine)
        {
            using (var writer = new StreamWriter(fileName, true))
            {
                if (newLine)
                    writer.WriteLine(text);
                else
                    writer.Write(text);

                writer.Close();
            }
        }
    }
}
