namespace Wallone.Core.Models.Settings
{
    public class User
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}