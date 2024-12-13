using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IMyLogger
    {
        void Information(string message);

        void Warning(string message);

        void Error(string message);

        void LogToFile(string message);
    }
}
