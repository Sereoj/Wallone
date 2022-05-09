using System;
using System.Collections.Generic;
using Wallone.Core.Interfaces;

namespace Wallone.Core.Models
{
    public class Theme
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CategoryShort> Categories { get; set; }
        public BrandShort Brand { get; set; }
        public string Preview { get; set; }
        public List<Image> Images { get; set; }
        public UserShort User { get; set; }
        public string Created_at { get; set; }
        public string Resolution { get; set; }
        public string HashCode { get; set; }
    }
}