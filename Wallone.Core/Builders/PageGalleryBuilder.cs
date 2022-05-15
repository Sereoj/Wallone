using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Wallone.Core.Models;

namespace Wallone.Core.Builders
{
    public class PageGalleryBuilder : IPageBuilder
    {
        private string routerApplication;
        private string routerWebsite;
        private string routerQueryWebsite;

        private List<Parameter> Parameters = new List<Parameter>();

        private string queryPage = "?page=";
        private string queryCategory = "&category_id=";
        private string queryBrand = "&brand_id=";
        private int PaginationID;

        public PageGalleryBuilder SetApplicationRouter(string nameRouter)
        {
            routerApplication = nameRouter;
            return this;
        }
        public string GetApplicationRouter()
        {
            return routerApplication;
        }

        public PageGalleryBuilder ValidateRouter()
        {
            routerWebsite = routerApplication.ToLower() switch
            {
                "new" => "wallpapers/new",
                "popular" => "wallpapers/popular",
                "wait" => "wallpapers/wait",
                "favourite" => "wallpapers/favorite",
                _ => "wallpapers"
            };

            return this;
        }

        public string GetWebsiteRouter()
        {
            return routerWebsite;
        }

        public PageGalleryBuilder SetPagination(int pageId)
        {
            PaginationID = pageId;
            return this;
        }

        public int GetPagination()
        {
            return PaginationID;
        }

        public PageGalleryBuilder SetBrand(string page, string pageId)
        {
            if (page.Contains("brands", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(pageId))
            {
                Parameters.Add(new Parameter()
                {
                    Name = "brand_id",
                    Value = pageId
                });
            }
            return this;
        }

        public PageGalleryBuilder SetCategory(string page, string pageId)
        {
            if (page.Contains("categories", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(pageId))
            {
                Parameters.Add(new Parameter()
                {
                    Name = "category_id",
                    Value = pageId
                });
            }
            return this;
        }

        public PageGalleryBuilder CreatePageQuery()
        {
            if (routerWebsite != null)
            {
                Parameters.Add(new Parameter()
                {
                    Name = "page",
                    Value = PaginationID.ToString()
                });
            }
            return this;
        }

        public List<Parameter> GetPageQuery()
        {
            return Parameters;
        }

        public void ClearQuery()
        {
            Parameters.Clear();
        }
    }
}