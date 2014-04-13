using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
   
namespace OAuth2Test
{
    class Program
    {
        public enum Method { GET, POST, DELETE };

        static void Main(string[] args)
        {
            //GET u9lrankemqfefpsbhmmbfikkhmhg4iao
            string url =
                "https://www.box.com/api/oauth2/authorize?response_type=code&client_id=zb185q3gef41kbee9qi3y0kej2tdeq2f&state=security_token%3Dd66d5824-ae41-4249-902e-c32ad5c5b244";

            using (var client = new WebClient())
            {
                var responseString = client.DownloadString(url);
            }


            var psi = new ProcessStartInfo("firefox.exe");
            psi.Arguments = url;
            Process.Start(psi);

            string reply = string.Empty;
            Console.WriteLine("Enter 'exit' to well... exit.");

            do
            {
                reply = Console.ReadLine();

                using (var client = new WebClient())
                {
                    
                   // var responseString = client.DownloadString("http://www.mydomain.com/recepticle.aspx");
                }

            } while (reply.ToLower() != "exit");

            //string response = WebRequest(Method.GET, url, String.Empty);

        }

        public static string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            //webRequest.UserAgent  = "Identify your application please.";
            //webRequest.Timeout = 20000;

            if (method == Method.POST || method == Method.DELETE)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = WebResponseGet(webRequest);

            webRequest = null;

            return responseData;

        }

        public static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }
}
