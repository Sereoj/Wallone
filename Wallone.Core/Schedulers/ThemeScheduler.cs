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
                if (!themeController.IsDone())
                {
                    themeController.Set(ThemeService.Get());
                }
                //выполнить следующий раз через..

                var time = Time(themeController.GetSpan().Ticks);

                SetInterval(time);
                Stop();
                Trace.WriteLine("Выполнить через: " + TimeSpan.FromMilliseconds(time));
                Start();
            }
        }

        public double Time(long tick)
        {
            return new TimeSpan(tick).TotalMilliseconds;
        }

        public static void SetInterval(double value)
        {
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
