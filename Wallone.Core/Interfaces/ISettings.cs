using Wallone.Core.Builders;

namespace Wallone.Core.Interfaces
{
    public interface ISettings : IAppSettings
    {
        string Email { get; set; }
        string Token { get; set; }
        string Language { get; set; }
        string WindowTheme { get; set; }
        string Host { get; set; }
        string Prefix { get; set; }
        string Theme { get; set; }
        string README { get; set; }
    }
}