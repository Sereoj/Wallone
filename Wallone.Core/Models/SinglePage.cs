using System.Collections.Generic;

namespace Wallone.Core.Models
{
    public class SinglePage
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string views { get; set; }
        public string likes { get; set; }
        public string downloads { get; set; }
        public List<Category> category { get; set; }
        public List<Tag> tags { get; set; }
        public Brand brand { get; set; }
        public string preview { get; set; }
        public List<Images> images { get; set; }
        public string favorite { get; set; }
        public string install { get; set; }
        public string reaction { get; set; }
        public User user { get; set; }
        public string download { get; set; }
        public List<Thumb> posts { get; set; }
        public string created_at { get; set; }
    }
}