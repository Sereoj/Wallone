using WinDynamicDesktop.Core.Builders;

namespace WinDynamicDesktop.Core.Interfaces
{
    public interface IThemeCreatedBuilder : IThemeBulder
    {
        public ThemeCreatedBuilder Download();
        public ThemeCreatedBuilder Remove();
        public ThemeCreatedBuilder DownloadAndInstall();
        public ThemeCreatedBuilder SetAttibuteDirectory();
        public ThemeCreatedBuilder SetAttibuteFiles();
    }
}