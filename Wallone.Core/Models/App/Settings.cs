using Prism.Mvvm;
using Wallone.Core.Interfaces;
using Wallone.Core.Models.Settings;

namespace Wallone.Core.Models.App
{
    public class Settings : ISettings
    {
        public string Information { get; set; }
        public Models.Settings.User User { get; set; } = new Models.Settings.User();
        public Models.Settings.General General { get; set; } = new General();
        public Models.Settings.Advanced Advanced { get; set; } = new Advanced();
        public Models.Settings.Server Server { get; set; } = new Server();
    }
}