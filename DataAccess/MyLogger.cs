using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MyLogger : IMyLogger
    {
        public void Information(string message)
        {
            LogToFile("(" + DateTime.Now + ") " + "Information: " + message);
        }

        public void Warning(string message)
        {
            LogToFile("(" + DateTime.Now + ") " + "Warning: " + message);
        }

        public void Error(string message)
        {
            LogToFile("(" + DateTime.Now + ") " + "Error: " + message);
        }

        public void LogToFile(string message)
        {
            using (StreamWriter sw = new("log.txt", true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
