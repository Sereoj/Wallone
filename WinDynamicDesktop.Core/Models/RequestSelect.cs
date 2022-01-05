using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Core.Models
{
    public class RequestSelect
    {
        public string category_id { get; set; }
        public string brand_id { get; set; }
        public string user_id { get; set; }

        public RequestSelect(string cat, string brand, string user)
        {
            category_id = cat;
            brand_id = brand;
            user_id = user;
        }
    }
}
