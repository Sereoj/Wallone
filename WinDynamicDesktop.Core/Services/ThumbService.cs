using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ThumbService
    {
        public static ObservableCollection<Thumb> GetThumbs(string page = null)
        {
            var request = RequestRouter.Get("wallpapers", page);
            return request.StatusCode == System.Net.HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<ObservableCollection<Thumb>>(request.Content)
                : null;
        }
    }
}
