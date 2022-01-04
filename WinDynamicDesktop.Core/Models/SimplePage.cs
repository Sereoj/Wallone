using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Core.Models
{
    public class SimplePage
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string views { get; set; }
        public string likes { get; set; }
        public List<Images> images { get; set; }
        public Category category { get; set; }
        public Brand brand { get; set; }
        public User user { get; set; }
        public string download { get; set; }
    }
}
