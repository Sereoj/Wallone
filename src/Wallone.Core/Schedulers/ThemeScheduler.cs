using System;
using System.Diagnostics;
using System.Timers;
using Windows.UI.StartScreen;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.Core.Schedulers
{
    public class ThemeScheduler
    {
        private readonly ThemeController<Theme> themeController;

        public ThemeScheduler(ThemeController<Theme> themeController)
        {
            this.themeController = themeController;
        }

        public static void Start()
        {
            //throw new NotImplementedException();
        }

        public static void Refresh()
        {
            //throw new NotImplementedException();
        }
    }
}
