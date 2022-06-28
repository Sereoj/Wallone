using System;
using System.Diagnostics;
using System.Timers;
using Windows.UI.StartScreen;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Schedulers
{
    public class ThemeScheduler
    {
        private readonly ThemeController<Theme> themeController;

        public ThemeScheduler(ThemeController<Theme> themeController)
        {
            this.themeController = themeController;
            LoggerService.Log(this, $"GetCurrentImage:" + this.themeController.Core().GetCurrentImage());
            LoggerService.Log(this, $"GetNextImage:" + this.themeController.Core().GetNextImage());
            LoggerService.Log(this, $"GetNextImage:" + this.themeController.Core().GetNextImage());
            LoggerService.Log(this, $"GetNextImage:" + this.themeController.Core().GetNextImage());
            LoggerService.Log(this, $"GetNextImage:" + this.themeController.Core().GetNextImage());
            LoggerService.Log(this, $"GetPreviousImage:" + this.themeController.Core().GetPreviousImage());
            LoggerService.Log(this, $"GetPreviousImage:" + this.themeController.Core().GetPreviousImage());
            LoggerService.Log(this, $"GetPreviousImage:" + this.themeController.Core().GetPreviousImage());
            LoggerService.Log(this, $"GetPreviousImage:" + this.themeController.Core().GetPreviousImage());
            LoggerService.Log(this, $"GetCurrentImage:" + this.themeController.Core().GetCurrentImage());
        }

        public static void Start()
        {
        }

        public static void Refresh()
        {
            //throw new NotImplementedException();
        }
    }
}
