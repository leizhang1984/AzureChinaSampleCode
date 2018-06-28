using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YumDMBWindowsService
{
    public class Log
    {
        public static void WriteLog(string msg)
        {
            string logPath = ConfigurationManager.AppSettings["logpath"].Trim().ToString();

            //判断是否有该文件夹           
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string logFileName = logPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//生成日志文件  
            if (!File.Exists(logFileName))//判断日志文件是否为当天  
            {
                File.Create(logFileName);//创建文件  
            }


            try
            {
                using (FileStream fs = new System.IO.FileStream(logFileName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter writer = new System.IO.StreamWriter(fs, System.Text.Encoding.Default))
                    {
                        try
                        {
                            writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + msg);
                            writer.Flush();
                            writer.Close();
                            fs.Close();
                        }
                        catch (Exception ex)
                        {
                            writer.WriteLine(DateTime.Now.ToString("日志记录错误HH:mm:ss") + " " + ex.Message + " " + msg);
                            writer.Flush();
                            writer.Close();
                            fs.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            { 

            }
        }
    }
}
