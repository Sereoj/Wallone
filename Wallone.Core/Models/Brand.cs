using Wallone.Core.Interfaces;

namespace Wallone.Core.Models
{
    public class Brand : IBrand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Tag { get; set; }
        public bool Status { get; set; }
    }
}