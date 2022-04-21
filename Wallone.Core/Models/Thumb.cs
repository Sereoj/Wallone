using System;

namespace Wallone.Core.Models
{
    public class Thumb
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Uri Preview { get; set; }

        public string Views { get; set; }
        public string Downloads { get; set; }
    }
}