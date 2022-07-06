using System.Timers;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Builders;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using System;

namespace Wallone.Core.Schedulers
{
    public class ThemeScheduler
    {
        private readonly ThemeController<Theme> themeController;

        private static Timer timer;

        public ThemeScheduler(ThemeController<Theme> themeController)
        {
            this.themeController = themeController;

            timer = new Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var useAnimation = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetAnimation();

            themeController.Core().Init();

            AutoReset(false);
            _= LoggerService.LogAsync(this, $"AutoReset {timer.AutoReset}");
            if (themeController.Core().IsNotNull())
            {
                var path = themeController.Core().GetCurrentImage();
                _ = LoggerService.LogAsync(this, $"Текущая тема: {themeController.GetThemeName()}");
                _ = LoggerService.LogAsync(this, $"Текущая фаза: {themeController.Core().GetPhase()}");
                _ = LoggerService.LogAsync(this, $"Текущее изображение: {path}");

                WallpaperInstaller.Animation.EnableTransitions(useAnimation);
                WallpaperInstaller.Controller.Set(path);

                var time = Time(themeController.Core().GetNextImageDateTime().Ticks);
                _ = LoggerService.LogAsync(this, $"Ждем: {TimeSpan.FromMilliseconds(time)}");
                SetInterval(time);
            }
            else
            {
                themeController.Core().SkipPhase();

                var path = themeController.Core().GetCurrentImage();

                _ = LoggerService.LogAsync(this, $"Текущая тема: {themeController.GetThemeName()}");
                _ = LoggerService.LogAsync(this, $"Текущая фаза: {themeController.Core().GetPhase()}");
                _ = LoggerService.LogAsync(this, $"Текущее изображение: {path}");

                WallpaperInstaller.Animation.EnableTransitions(useAnimation);
                WallpaperInstaller.Controller.Set(path);

                var time = Time(themeController.Core().GetNextImageDateTime().Ticks);
                _ = LoggerService.LogAsync(this, $"Ждем снова: {TimeSpan.FromMilliseconds(time)}");
                SetInterval(time);
            }
            AutoReset(true);
            _ = LoggerService.LogAsync(this, $"AutoReset {timer.AutoReset}");
        }
        private void AutoReset(bool value)
        {
            timer.AutoReset = value;
        }
        public void SetInterval(double value)
        {
            if(value > 0)
            {
                timer.Interval = value;
            }
            else
            {
                _ =LoggerService.LogAsync(this, $"Таймер не должен быть {value}");
                Stop();
            }
        }
            private double Time(long tick)
        {
            return new TimeSpan(tick).TotalMilliseconds;
        }

        public static void Start()
        {
            if (timer == null)
            {
                _ = LoggerService.LogAsync(typeof(ThemeScheduler), "Невозможно запустить таймер, поскольку не существует экземпляра", Message.Error);
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
                _ = LoggerService.LogAsync(typeof(ThemeScheduler), "Невозможно остановить таймер, поскольку не существует экземпляра", Message.Error);
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
