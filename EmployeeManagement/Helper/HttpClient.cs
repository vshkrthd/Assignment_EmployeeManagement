using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeManagement
{
    public class HttpClient
    {
        private static readonly string _apiKey = ConfigurationManager.AppSettings["apiKey"];
       
        public static string Create(HttpMethod httpMethod, string url, string data = null)
        {
            string responseFromServer = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = httpMethod.ToString();
            request.Headers.Add("api-key", _apiKey);
            request.Accept = "/";
            request.ContentType = "application/json";

            if (!string.IsNullOrEmpty(data))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            else
            {
                WebResponse response = request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                responseFromServer = sr.ReadToEnd();
                sr.Close();
            }
           
            return responseFromServer;
        }
    }
}
