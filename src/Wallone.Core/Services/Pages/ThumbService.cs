using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services.Pages
{
    public class ThumbService
    {
        /// <summary>
        /// Получение значений со сервера
        /// </summary>
        /// <param name="router"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Task<List<Thumb>> GetThumbsAsync(string router, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync(router, null, parameters);
            return items;
        }

        /// <summary>
        /// Проверка списка на null
        /// </summary>
        /// <param name="items"></param>
        /// <returns>true or false</returns>
        public static bool IsNotNull(List<Thumb> items)
        {
            return items != null;
        }

        /// <summary>
        /// Проверка id на null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsIdNotNull(string id)
        {
            return id != null;
        }

        /// <summary>
        /// Валидация имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ValidateName(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }

        /// <summary>
        /// Валидация просмотров
        /// </summary>
        /// <param name="views"></param>
        /// <returns></returns>
        public static string ValidateViews(string views)
        {
            return views ?? "0";
        }

        /// <summary>
        /// Валидация загрузок
        /// </summary>
        /// <param name="downloads"></param>
        /// <returns></returns>
        public static string ValidateDownloads(string downloads)
        {
            return downloads ?? "0";
        }

        /// <summary>
        /// Валидация изображений
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Uri Validate(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                if (uri.IsFile)
                {
                    if (File.Exists(uri.LocalPath)) return uri;
                    return UriHelper.Get("/Wallone.Common;component/Images/Placeholder.png");
                }

                return uri;
            }

            return UriHelper.Get("/Wallone.Common;component/Images/Placeholder.png");
        }

        /// <summary>
        /// Валидация превью
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Uri ValidatePreview(Uri uri)
        {
            return Validate(uri);
        }
    }
}