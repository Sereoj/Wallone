namespace Wallone.Core.Models
{
    public class Config
    {
        public string themeId { get; set; }
        public string displayName { get; set; }
        public string imageFilename { get; set; }
        public string imageCredits { get; set; }
        public int? dayHighlight { get; set; }
        public int? nightHighlight { get; set; }
        public int[] sunriseImageList { get; set; }
        public int[] dayImageList { get; set; }
        public int[] sunsetImageList { get; set; }
        public int[] nightImageList { get; set; }
    }
}