using System;

namespace Wallone.Core.Models
{
    public class ThemePreviewItem
    {
        public ThemePreviewItem(Times name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }

        public Times Name { get; set; }
        public Uri Uri { get; set; }
    }
}