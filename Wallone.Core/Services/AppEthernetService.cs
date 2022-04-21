using System;
using System.Net;

namespace Wallone.Core.Services
{
    public class AppEthernetService
    {
        private static HttpStatusCode codeStatus;
        public static bool IsConnect(string domain)
        {
            try
            {
                WebRequest request = WebRequest.Create(domain);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                codeStatus = response.StatusCode;
                response.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SetStatus(HttpStatusCode code)
        {
            codeStatus = code;
        }

        public static HttpStatusCode GetStatus()
        {
            return codeStatus;
        }
    }
}
