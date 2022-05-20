using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Wallone.Core.Helpers
{
    public class ColorHelper
    {
        private readonly List<SolidColorBrush> brushes = new List<SolidColorBrush>
        {
            new SolidColorBrush(Color.FromRgb(148, 126, 195)),
            new SolidColorBrush(Color.FromRgb(151, 196, 184)),
            new SolidColorBrush(Color.FromRgb(254, 251, 231)),
            new SolidColorBrush(Color.FromRgb(255, 161, 161)),
            new SolidColorBrush(Color.FromRgb(130, 115, 151)),
            new SolidColorBrush(Color.FromRgb(246, 137, 137))
        };

        public SolidColorBrush Get()
        {
            return brushes[new Random().Next(0, brushes.Count)];
        }
    }
}