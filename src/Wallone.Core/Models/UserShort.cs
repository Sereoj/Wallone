using Wallone.Core.Interfaces;

namespace Wallone.Core.Models
{
    public class UserShort : IUserShort
    {
        public string id { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
    }
}