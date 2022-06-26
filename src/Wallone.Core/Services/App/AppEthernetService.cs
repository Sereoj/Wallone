using System;
using System.Net;

namespace Wallone.Core.Services.App
{
    public class AppEthernetService
    {
        private static HttpStatusCode codeStatus;

        public static bool IsConnect(string domain)
        {
            try
            {
                var request = WebRequest.Create(domain);
                var response = (HttpWebResponse) request.GetResponse();
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