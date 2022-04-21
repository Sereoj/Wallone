using System;

namespace Wallone.Core.Models
{
    public class ThemePreviewItem
    {
        public ThemePreviewItem(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; set; }
        public Uri Uri { get; set; }
    }
}