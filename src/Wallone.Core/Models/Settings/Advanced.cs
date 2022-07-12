namespace Wallone.Core.Models.Settings
{
    public class Advanced
    {
        public Geolocation Type { get; set; } = Geolocation.Auto;
        public bool Log { get; set; } = false;
    }
}