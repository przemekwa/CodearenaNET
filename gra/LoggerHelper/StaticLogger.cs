using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LoggerHelper
{
    public static class StaticLogger
    {
        public static Logger log { get; set; }

        static StaticLogger()
        {
            log = LogManager.GetLogger("Gra");
        }

        
    }
}
