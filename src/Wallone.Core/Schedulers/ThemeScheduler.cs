using System;
using System.Diagnostics;
using System.Timers;
using Windows.UI.StartScreen;
using Wallone.Core.Controllers;
using Wallone.Core.Services;

namespace Wallone.Core.Schedulers
{
    public class ThemeScheduler
    {
        private readonly ThemeController themeController;
        private static readonly Timer SchedulerTimer = new Timer();
        public ThemeScheduler(ThemeController themeController)
        {
            this.themeController = themeController;
            SchedulerTimer.Interval = 1000;
            SchedulerTimer.Elapsed += SchedulerTimer_Elapsed;
        }

        private void SchedulerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (themeController.IsAwake())
            {
                themeController.Set(ThemeService.Get());
                long ticks = themeController.GetSpan().Ticks;

                var spanNextImage = Time(ticks);
                SetInterval(spanNextImage);

                Stop();
                Trace.WriteLine("Выполнить через: " + TimeSpan.FromMilliseconds(spanNextImage));
                Start();
            }
        }

        public double Time(long tick)
        {
            return new TimeSpan(tick).TotalMilliseconds;
        }

        public static void SetInterval(double value)
        {
            if (value == 0)
                value = 1000;
            SchedulerTimer.Interval = value;
        }
        public void Start()
        {
            SchedulerTimer.Start();
        }
        public static void Run()
        {
            SchedulerTimer.Start();
        }

        public static void Stop()
        {
            SchedulerTimer.Stop();
        }
    }
}
