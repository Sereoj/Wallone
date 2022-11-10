using Wallone.Core.Interfaces;

namespace Wallone.Core.Models
{
    public class Category : ICategory
    {
        public string id { get; set; }
        public string name_ru { get; set; }
        public string name_en { get; set; }
        public string slug { get; set; }
        public string image_path { get; set; }
        public string user_id { get; set; }
        public bool Status { get; set; } = true;
    }
}