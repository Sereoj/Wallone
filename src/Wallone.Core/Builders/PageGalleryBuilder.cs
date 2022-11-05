using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Builders
{
    public class PageGalleryBuilder : IPageBuilder
    {
        private string routerApplication;
        private string routerWebsite;

        private List<Parameter> Parameters = new List<Parameter>();

        private int paginationId;

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
                "new" => Pages.News,
                "popular" => Pages.Popular,
                "wait" => Pages.Soon,
                "favourite" => Pages.Favorites,
                _ => Pages.Main
            };

            return this;
        }

        public string GetWebsiteRouter()
        {
            return routerWebsite;
        }

        public PageGalleryBuilder SetPagination(int pageId)
        {
            paginationId = pageId;
            return this;
        }

        public int GetPagination()
        {
            return paginationId;
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

        public void CreatePageQuery()
        {
            if (routerWebsite != null)
            {
                Parameters.Add(new Parameter()
                {
                    Name = "page",
                    Value = paginationId.ToString()
                });
            }
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