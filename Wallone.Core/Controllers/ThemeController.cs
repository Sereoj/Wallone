using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.System.UserProfile;
using SunCalcNet.Model;
using Wallone.Core.Builders;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.Core.Controllers
{
    public class ThemeController
    {
        private static readonly uint SPI_SETDESKWALLPAPER = 20;
        private static readonly uint SPIF_UPDATEINIFILE = 0x1;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        private Phase PhaseModel { get; set; }
        private SettingsItemBuilder SettingsItemBuilder { get; set; }
        private Times nextTime;
        private bool isDone;
        private double lat;
        private double lng;
        public ThemeController()
        {
            PhaseModel = PhaseService.GetPhase();

            SettingsItemBuilder = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();
            GetLocation();
        }

        public void GetLocation()
        {
            lat = SettingsItemBuilder.GetLatitude();
            lng = SettingsItemBuilder.GetLongitude();
        }

        public void Set(Theme theme)
        {
            if (theme != null)
            {
                ThemeService.Set(theme);

                switch (SettingsItemBuilder.GetMode())
                {
                    case Mode.UseWebLocation:
                        UseWebLocation(theme, PhaseModel, lat, lng);
                        break;
                    case Mode.NoUseLocation:
                        var image = GetCurrentImageByTime(theme.Images);
                        SetImage(image.location);
                    break;
                }
                ThemeService.Save();
            }
        }

        private void UseWebLocation(Theme theme, Phase phaseModel, double lat, double lng)
        {
            DateTime time = DateTime.Now;
            var imageCount = theme.Images.Count;

            var sunPhases = GetSunPhases(lat, lng).ToList();

            phaseModel.dawnSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dawn);
            phaseModel.sunriseSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunrise);
            phaseModel.daySolarTime = GetSolarTime(sunPhases, SunPhaseName.SolarNoon);
            phaseModel.goldenSolarTime = GetSolarTime(sunPhases, SunPhaseName.GoldenHour);
            phaseModel.sunsetSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunset);
            phaseModel.duskSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dusk);
            phaseModel.nightSolarTime = GetSolarTime(sunPhases, SunPhaseName.Night);

            if (phaseModel.dawnSolarTime < time && phaseModel.sunriseSolarTime > time)
            {
                Trace.WriteLine("Заря");
                SetSpan(phaseModel.sunriseSolarTime, phaseModel.dawnSolarTime, imageCount);
                SetCurrentImage(theme, phaseModel, phaseModel.dawnSolarTime, time, Times.Dawn);
            }
            if (phaseModel.sunriseSolarTime < time && phaseModel.daySolarTime > time)
            {
                Trace.WriteLine("Утро");
                SetSpan(phaseModel.daySolarTime, phaseModel.sunriseSolarTime, imageCount);
                SetCurrentImage(theme, phaseModel, phaseModel.sunriseSolarTime, time, Times.Sunrise);
            }
            if (phaseModel.daySolarTime < time && phaseModel.goldenSolarTime > time)
            {
                Trace.WriteLine("День");
                SetSpan(phaseModel.goldenSolarTime, phaseModel.daySolarTime, imageCount);
                SetCurrentImage(theme, phaseModel, phaseModel.daySolarTime, time, Times.Day);
            }
            if (phaseModel.goldenSolarTime < time && phaseModel.sunsetSolarTime > time)
            {
                Trace.WriteLine("Золотое время");
                SetSpan(phaseModel.sunsetSolarTime, phaseModel.goldenSolarTime, imageCount);
                SetCurrentImage(theme, phaseModel, phaseModel.goldenSolarTime, time, Times.GoldenHour);
            }
            if (phaseModel.sunsetSolarTime < time && phaseModel.duskSolarTime > time)
            {
                Trace.WriteLine("Закат");
                SetSpan(phaseModel.duskSolarTime, phaseModel.sunsetSolarTime, imageCount);
                SetCurrentImage(theme, phaseModel, phaseModel.sunsetSolarTime, time, Times.Sunset);
            }
            else
            {
                if (phaseModel.duskSolarTime < time)
                {
                    Trace.WriteLine("Ночь");
                    SetSpan(phaseModel.duskSolarTime, phaseModel.dawnSolarTime, imageCount);
                    SetCurrentImage(theme, phaseModel, phaseModel.duskSolarTime, time, Times.Night);
                }
            }
        }

        public void SetSpan(DateTime date2, DateTime date1, int count)
        {
            var time = Span(date2, date1, count);
            ThemeService.SetTimeSpan(time);
        }

        public TimeSpan GetSpan()
        {
            return ThemeService.GetTimeSpan();
        }

        public void SetCurrentImage(Theme theme, Phase phaseModel, DateTime date1, DateTime nowDateTime, Times times)
        {
            var images = GetImagesWithTime(theme.Images, times);

            if (images.Count != 0)
            {
                var id = GetSpanId(nowDateTime, date1);
                SetImage(images.Count > id ? images[id].location : images.LastOrDefault()!.location);
            }
            else
            {
                //рекурсия, пока не найдем
                NextTime(times);
                SetCurrentImage(theme, phaseModel, date1, nowDateTime, nextTime);
            }
        }


        public void NextTime(Times times)
        {
            switch (times)
            {
                case Times.Dawn:
                    nextTime = Times.Sunrise;
                    break;
                case Times.Sunrise:
                    nextTime = Times.Day;
                    break;
                case Times.Day:
                    nextTime = Times.GoldenHour;
                    break;
                case Times.GoldenHour:
                    nextTime = Times.Sunset;
                    break;
                case Times.Sunset:
                    nextTime = Times.Night;
                    break;
                case Times.Night:
                    nextTime = Times.Dawn;
                    break;
            }
        }

        public TimeSpan Span(DateTime date2, DateTime date1, int count)
        {
            return (date2 - date1) / count;
        }

        public int GetSpanId(DateTime now, DateTime date1)
        {
            return (now - date1).Hours;
        }

        public static bool SetImage(string filename)
        {
            if (filename == null || !AppSettingsService.ExistsFile(filename)) return false;
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
            return true;
        }

        private static List<SunPhase> GetSunPhases(double latitude, double longitude)
        {
            return SunCalcNet.SunCalc.GetSunPhases(DateTime.Today, latitude, longitude).ToList();
        }

        private static DateTime GetSolarTime(List<SunPhase> sunPhases, SunPhaseName desiredPhase)
        {
            SunPhase sunPhase = sunPhases.FirstOrDefault(sp => sp.Name.Value == desiredPhase.Value);
            return sunPhase.PhaseTime.ToLocalTime();
        }


        public List<Image> GetImagesWithTime(List<Image> images, Times time)
        {
            return images.Where(image => image.times == time).ToList();
        }


        public Image GetCurrentImageByTime(List<Image> images)
        {
            var hours = int.Parse(DateTime.Now.ToString("HH"));

            if (6 <= hours && hours <= 11) return images.FirstOrDefault(image => image.times == Times.Sunrise);

            if (12 <= hours && hours <= 17) return images.FirstOrDefault(image => image.times == Times.Day);

            if (18 <= hours && hours <= 23) return images.FirstOrDefault(image => image.times == Times.Sunset);

            if (24 >= hours && hours <= 6) return images.FirstOrDefault(image => image.times == Times.Night);

            return null;
        }

        public bool IsAwake()
        {
            return true;
        }

        public bool IsDone()
        {
            return isDone;
        }

        public void Done()
        {
            isDone = true;
        }
    }
}