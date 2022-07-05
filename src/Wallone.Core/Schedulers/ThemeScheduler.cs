using System.Timers;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Builders;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Schedulers
{
    public class ThemeScheduler
    {
        private readonly ThemeController<Theme> themeController;

        private static Timer timer;

        public ThemeScheduler(ThemeController<Theme> themeController)
        {
            this.themeController = themeController;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var useAnimation = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetAnimation();

            var path = themeController.Core().GetCurrentImage();

            LoggerService.Log(this, $"Текущая тема: {themeController.GetThemeName()}");
            LoggerService.Log(this, $"Текущая фаза: {themeController.Core().GetPhase()}");
            LoggerService.Log(this, $"Текущее изображение: {themeController.Core().GetCurrentImage()}");
            LoggerService.Log(this, $"Следующее изображение: {themeController.Core().GetNextImage()}");

            WallpaperInstaller.Animation.EnableTransitions(useAnimation);
            WallpaperInstaller.Controller.Set(path);

            timer.Stop();


        }

        public static void Start()
        {
            if (timer == null)
            {
                LoggerService.Log(typeof(ThemeScheduler), "Невозможно запустить таймер, поскольку не существует экземпляра", Message.Error);
            }
            else
            {
                timer.Start();
            }
        }

        public static void Stop()
        {
            if (timer == null)
            {
                LoggerService.Log(typeof(ThemeScheduler), "Невозможно остановить таймер, поскольку не существует экземпляра", Message.Error);
            }
            else
            {
                timer.Stop();
            }
        }

        public static void Refresh()
        {
            Stop();
            Start();
        }
    }
}
