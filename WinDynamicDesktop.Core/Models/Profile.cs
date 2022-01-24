﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Core.Models
{
    public class Profile
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string country { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public string friends_count { get; set; }
        public string publish_count { get; set; }
        public string users_likes { get; set; }
        public string is_friend { get; set; }
        public List<Thumb> posts { get; set; }
    }
}
