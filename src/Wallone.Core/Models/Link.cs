using System;

namespace Wallone.Core.Models
{
    [Flags]
    public enum Times
    {
        Dawn,
        Sunrise,
        Day,
        GoldenHour,
        Sunset,
        Night,
        NotFound
    }

    [Flags]
    public enum ResolutionMode
    {
        Custom,
        Template,
        Auto,
        NotFound
    }
    public class Link
    {
        public string id { get; set; }
        public Times name { get; set; }
        public string location { get; set; }
        public string format { get; set; }
        public string resolution { get; set; }
    }

    public class Image
    {
        public int id { get; set; }
        public Times times { get; set; }
        public string location { get; set; }
    }
}