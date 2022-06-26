namespace Wallone.Core.Services.Routers
{
    public class Router
    {
        public static string domainExample { get; set; } = "https://example.com";
        public static string domainApi { get; set; }

        public static string domain { get; set; }
        //public static string domainApi = "http://v3.w2me.ru/public/api";
        //public static string domain = "http://v3.w2me.ru";

        public static void SetDomainApi(string value)
        {
            domainApi = value;
        }

        public static void SetDomain(string value)
        {
            domain = value;
        }

        public static string OnlyNameDomainApi()
        {
            return domainApi.Replace("https://", null).Replace("http://", null);
        }

        public static string OnlyNameDomain()
        {
            return domain.Replace("https://", null).Replace("http://", null);
        }
    }
}