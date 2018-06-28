using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace YumDMBWindowsService
{
    public class AzureVMOperation
    {
        public AzureVMOperation()
        {

        }
        static X509Certificate2 Certificate;
        public static bool CheckCertificate(string certString)
        {
            string certPassword = "";
            try
            {
                Certificate = new X509Certificate2(Convert.FromBase64String(certString), certPassword);
            }
            catch (Exception ex)
            {
                Log.WriteLog("请检查App.config文件的thumbprint参数," + ex.Message);
            }
            return true;
        }

        public static void StartVM(string subscriptionID, string cloudServiceName, string deploymentsName, string vmName, int threadSleepSecond)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri("https://management.core.chinacloudapi.cn/" + subscriptionID
            + "/services/hostedservices/" + cloudServiceName + "/deployments/" + deploymentsName + "/roleinstances/" + vmName + "/Operations"));

            request.Method = "POST";
            request.ClientCertificates.Add(Certificate);
            request.ContentType = "application/xml";
            request.Headers.Add("x-ms-version", "2014-04-01");

            // Add body to the request
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load("..\\..\\StartVM.xml");

            xmlDoc.Load(ConfigurationManager.AppSettings["startvmxmlpath"].ToString());

            Stream requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream, System.Text.UTF8Encoding.UTF8);
            xmlDoc.Save(streamWriter);

            streamWriter.Close();
            requestStream.Close();

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                    Thread.Sleep(threadSleepSecond);
                }
            }
            catch (WebException ex)
            {
                Log.WriteLog(ex.Message);
            }
        }

        public static void StopVM(string subscriptionID, string serviceName, string deploymentsName, string vmName, bool Deallocated, int threadSleepSecond)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri("https://management.core.chinacloudapi.cn/" + subscriptionID
                + "/services/hostedservices/" + serviceName + "/deployments/" + deploymentsName + "/roleinstances/" + vmName + "/Operations"));

            request.Method = "POST";
            request.ClientCertificates.Add(Certificate);
            request.ContentType = "application/xml";
            request.Headers.Add("x-ms-version", "2014-04-01");

            //Add body to the reqeust 
            XmlDocument xmlDoc = new XmlDocument();
            if (Deallocated)
            {
                xmlDoc.Load(ConfigurationManager.AppSettings["stopvmdeallocatedxmlpath"].ToString());
            }
            else
            {
                //xmlDoc.Load("..\\..\\StopVM.xml");
                xmlDoc.Load(ConfigurationManager.AppSettings["stopvmxmlpath"].ToString());
            }

            Stream requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream, System.Text.UTF8Encoding.UTF8);
            xmlDoc.Save(streamWriter);

            streamWriter.Close();
            requestStream.Close();

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                    Thread.Sleep(threadSleepSecond);
                }
            }
            catch (WebException ex)
            {
                //Log.WriteLog(ex.Message);
            }

        }

    }
}
