namespace Wallone.Core.Services.Routers
{
    public class AppRouter
    {
        public const Verison Currect = Verison.dev;

        public enum Verison
        {
            dev,
            prod,
            test
        }
    }
    public class Pages
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string Logout = "logout";
        public const string Token = "user";
        public const string Policy = "policy";

        public const string Main = "themes";
        public const string News = "themes/new";
        public const string Popular = "themes/popular";
        public const string Soon = "themes/soon";
        public const string Favorites = "themes/favorites";

        public const string Single = "themes/{theme}/show";
    }
    public class Router
    {
        public static string domainExample { get; set; } = "https://example.com";
        public static string domainApi { get; set; }
        public static string domain { get; set; }

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
            return HttpToNull(domainApi);
        }

        public static string OnlyNameDomain()
        {
            return HttpToNull(domain);
        }
        
        private static string HttpToNull(string value)
        {
            return value.Replace("https://", null).Replace("http://", null);
        }
    }
}