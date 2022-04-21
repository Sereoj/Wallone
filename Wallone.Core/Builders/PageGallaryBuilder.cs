using System.Collections.Generic;
using Wallone.Core.Models;

namespace Wallone.Core.Builders
{
    public class PageGallaryBuilder : IPageBulder
    {
        private string catalog;
        private string field;
        private string page;

        private List<Parameter> Fileds { get; } = new List<Parameter>();

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
            if (field != null) Fileds.Add(new Parameter(field, page));
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
                _ => "wallpapers"
            };
        }
    }
}