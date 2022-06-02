namespace Wallone.Core.Interfaces
{
    public interface ISettings
    {
        public string Information { get; set; }
        public Models.Settings.User User { get; set; }
        public Models.Settings.General General { get; set; }
        public Models.Settings.Advanced Advanced { get; set; }
        public Models.Settings.Server Server { get; set; }
    }
}