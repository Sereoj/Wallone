namespace Wallone.Core.Models
{
    public enum Geolocation
    {
        Custom,
        Auto,
        Windows
    }
    public class Location
    {
        public string country { get; set; }
        public string city { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}