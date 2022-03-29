using System.Collections.Generic;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Builders
{
    public class PageGallaryBuilder : IPageBulder
    {
        private string catalog;
        private string page;
        private string field;
        private List<Parameter> Fileds { get; set; } = new List<Parameter>();
        public PageGallaryBuilder()
        {

        }

        public bool IsAds => false;

        public IPageBulder HasConnection()
        {
            return this;
        }

        public IPageBulder HasData()
        {
            return this;
        }

        public IPageBulder Catalog(string root)
        {
            catalog = root;
            return this;
        }

        public IPageBulder Page(string id)
        {
            page = id;
            return this;
        }

        public IPageBulder Validate()
        {
            switch (catalog)
            {
                case "Gallery":
                    field = null;
                    break;
                case "Brands":
                    field = "brand_id";
                    break;
                case "Categories":
                    field = "category_id";
                    break;
                default:
                    catalog = "Gallery";
                    break;
            }
            return this;
        }
        public IPageBulder ShowAds(bool value)
        {
            return this;
        }
        public IPageBulder Build()
        {
            if (field != null)
            {
                Fileds.Add(new Parameter(field, page));
            }
            return this;
        }

        public List<Parameter> GetFields()
        {
            return Fileds;
        }
        public string GetCatalog()
        {
            return catalog;
        }

        public string GetPage()
        {
            return page;
        }

        public string GetRouter()
        {
            return page.ToLower() switch
            {
                "new" => "wallpapers/new",
                "popular" => "wallpapers/popular",
                "wait" => "wallpapers/wait",
                "favourite" => "wallpapers/favorite",
                "downloads" => "wallpapers/downloads",
                _ => "wallpapers",
            };
        }
    }
    public class PageSingleBuilder
    {
        private SinglePage simplePage;

        public PageSingleBuilder(SinglePage simplePage)
        {
            this.simplePage = simplePage;
        }
    }
}
