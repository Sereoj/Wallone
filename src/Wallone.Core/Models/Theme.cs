using System.Collections.Generic;

namespace Wallone.Core.Models
{
    public class Theme
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Preview { get; set; }
        public List<Image> Images { get; set; }
        public UserShort User { get; set; }
        public string Created_at { get; set; }
        public string Resolution { get; set; }
    }
}