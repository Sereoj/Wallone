using System;

namespace Wallone.Core.Models
{
    [Flags]
    public enum Geolocation
    {
        Auto = 0,
        Windows = 1,
        Custom = 2
    }
    public class Location
    {
        public string country { get; set; }
        public string city { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}