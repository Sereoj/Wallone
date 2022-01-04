using System;

namespace WinDynamicDesktop.Core.Models
{
    public class ThemePreviewItem
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }

        public ThemePreviewItem(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }
    }
}
