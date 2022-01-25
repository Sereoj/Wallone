namespace WinDynamicDesktop.Core.Models
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string email_verified_at { get; set; }
        public string description { get; set; }
        public string country { get; set; }
        public string dob { get; set; }
        public string cover { get; set; }
        public string avatar { get; set; }
        public string lang { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string github { get; set; }
        public string vk { get; set; }
        public int role { get; set; }
        public bool reported { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
