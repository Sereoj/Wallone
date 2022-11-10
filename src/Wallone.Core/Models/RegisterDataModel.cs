using System.Collections.Generic;

namespace Wallone.Core.Models
{
    public class RegisterDataModel
    {
        public string message { get; set; }

        public RegisterErrors errors { get; set; }
    }

    public class RegisterErrors
    {
        public List<string> username { get; set; }
        public List<string> email { get; set; }
        public List<string> password { get; set; }
        public List<string> password_confirmation { get; set; }
    }
}