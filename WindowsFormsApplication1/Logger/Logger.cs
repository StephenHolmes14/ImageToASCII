using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logger
{
    public static class Logger
    {
        private static StreamWriter m_fileStream;

        public static void Initialize(string filename)
        {
            var loggerPath = Path.Combine(Directory.GetCurrentDirectory(), filename);

            m_fileStream = File.CreateText(loggerPath);
        }

        public static void WriteLine(string line)
        {
            m_fileStream.WriteLine(line);
        }

        public static void Dispose()
        {
            m_fileStream.Dispose();
        }
    }
}