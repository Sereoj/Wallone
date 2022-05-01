namespace Wallone.Core.Models
{
    public class Link
    {
        public string id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string format { get; set; }
        public string resolution { get; set; }
    }

    public class Image
    {
        public string id { get; set; }
        public string type { get; set; }
        public string location { get; set; }
    }
}