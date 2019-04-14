using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ruanmou.Common
{
    public class LogManager
    {
        private static string path = "..\\log.txt";
        static LogManager()
        {

            if (!File.Exists(path))
            {
                File.Create(path);
            }
           

        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo"></param>
        public static void WriteLog(string className,string methodName,string errorMessage)
        {
            using (StreamWriter sw = new StreamWriter(path,true))
            {
                sw.WriteLine(className+"(cls)"+methodName+"(method):"+errorMessage);
            }

        }

    }
}
