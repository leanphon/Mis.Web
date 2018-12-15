using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Utility
{
    public class LogHelper
    {
        public static readonly string filename = "D:\\mis-log.txt";
        public static void WriteLog(string msg)
        {
            try
            {
                FileStream fs = File.Open(filename, FileMode.Append | FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                byte[] data = System.Text.Encoding.Default.GetBytes(msg);

                fs.Write(data, 0, data.Length);

                fs.Flush();
                fs.Close();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void Error(string msg)
        {
            string fmt = string.Format("[{0}][ERROR]{1}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg);
           WriteLog(fmt);
        }

    }
}
