using System;
using System.Collections.Generic;
using System.Text;

namespace Wallone.Core.Models
{
    public class Theme
    {
        public string Name { get; set; }
        public string Preview { get; set; }
        public string Views { get; set; }
        public string Downloads { get; set; }
        public string User { get; set; }
        public string Resolution { get; set; }
        public List<Image> ImagesList { get; set; }
        public string DATA_KEY { get; set; }
        public string HashCode { get; set; }
    }
}
