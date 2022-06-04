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

        public void Load(Theme theme)
        {
            if (theme != null)
            {
                ThemeService.Set(theme);
                ThemeService.Save();
            }
        }

        public void Set(Theme theme)
        {
            if (theme != null)
            {
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
            }
        }

        private void UseWebLocation(Theme theme, Phase phaseModel, double lat, double lng)
        {
            DateTime time = DateTime.Now;
            DateTime oldTime = default;
            Times currentPhase = Times.Dawn; 

            var sunPhases = GetSunPhases(DateTime.Today, lat, lng).ToList();

            phaseModel.dawnSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dawn);
            phaseModel.sunriseSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunrise);
            phaseModel.daySolarTime = GetSolarTime(sunPhases, SunPhaseName.SolarNoon);
            phaseModel.goldenSolarTime = GetSolarTime(sunPhases, SunPhaseName.GoldenHour);
            phaseModel.sunsetSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunset);
            phaseModel.duskSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dusk);
            phaseModel.nightSolarTime = GetSolarTime(sunPhases, SunPhaseName.Night);

            if (phaseModel.dawnSolarTime < time && phaseModel.sunriseSolarTime > time)
            {
                Trace.WriteLine($"Заря {phaseModel.dawnSolarTime} {phaseModel.sunriseSolarTime}");

                oldTime = phaseModel.dawnSolarTime;
                currentPhase = Times.Dawn;

                var images = GetImagesWithTime(theme.Images, currentPhase);
                SetSpan(phaseModel.sunriseSolarTime, time, images.Count);

            }
            if (phaseModel.sunriseSolarTime < time && phaseModel.daySolarTime > time)
            {
                Trace.WriteLine($"Утро {phaseModel.sunriseSolarTime} {phaseModel.daySolarTime}");

                oldTime = phaseModel.sunriseSolarTime;
                currentPhase = Times.Sunrise;

                var images = GetImagesWithTime(theme.Images, currentPhase);
                SetSpan(phaseModel.daySolarTime, time, images.Count);
            }
            if (phaseModel.daySolarTime < time && phaseModel.goldenSolarTime > time)
            {
                Trace.WriteLine($"День {phaseModel.daySolarTime} {phaseModel.goldenSolarTime}");

                oldTime = phaseModel.daySolarTime;
                currentPhase = Times.Day;
                var images = GetImagesWithTime(theme.Images, currentPhase);

                SetSpan(phaseModel.goldenSolarTime, time, images.Count);
            }
            if (phaseModel.goldenSolarTime < time && phaseModel.sunsetSolarTime > time)
            {
                Trace.WriteLine($"Золотое время {phaseModel.goldenSolarTime} {phaseModel.sunsetSolarTime}");

                oldTime = phaseModel.goldenSolarTime;
                currentPhase = Times.GoldenHour;
                var images = GetImagesWithTime(theme.Images, currentPhase);

                SetSpan(phaseModel.sunsetSolarTime, time, images.Count);
            }
            if (phaseModel.sunsetSolarTime < time && phaseModel.duskSolarTime > time)
            {
                oldTime = phaseModel.sunsetSolarTime;
                currentPhase = Times.Sunset;
                var images = GetImagesWithTime(theme.Images, currentPhase);

                Trace.WriteLine($"Закат {phaseModel.sunsetSolarTime} {phaseModel.duskSolarTime}");
                SetSpan(phaseModel.duskSolarTime, time, images.Count);
            }
            else
            {
                if (phaseModel.duskSolarTime < time)
                {
                    oldTime = phaseModel.duskSolarTime;
                    currentPhase = Times.Night;
                    var images = GetImagesWithTime(theme.Images, currentPhase);

                    var nextSunPhases = GetSunPhases(DateTime.Today.AddDays(1), lat, lng).ToList();
                    phaseModel.dawnSolarTime = GetSolarTime(nextSunPhases, SunPhaseName.Dawn);
                    phaseModel.sunriseSolarTime = GetSolarTime(nextSunPhases, SunPhaseName.Sunrise);
                    phaseModel.daySolarTime = GetSolarTime(nextSunPhases, SunPhaseName.SolarNoon);
                    phaseModel.goldenSolarTime = GetSolarTime(nextSunPhases, SunPhaseName.GoldenHour);
                    phaseModel.sunsetSolarTime = GetSolarTime(nextSunPhases, SunPhaseName.Sunset);
                    phaseModel.duskSolarTime = GetSolarTime(nextSunPhases, SunPhaseName.Dusk);
                    phaseModel.nightSolarTime = GetSolarTime(nextSunPhases, SunPhaseName.Night);

                    Trace.WriteLine($"Ночь {oldTime}");
                    Trace.WriteLine($"Утро {phaseModel.dawnSolarTime}");
                    Trace.WriteLine($"Получается {phaseModel.dawnSolarTime - DateTime.Now}");
                    SetSpan(phaseModel.dawnSolarTime, time, images.Count);
                }
            }

            PhaseModel.currentPhase = currentPhase;
            PhaseService.SetModel(phaseModel);
            SetCurrentImage(theme, PhaseModel, oldTime, DateTime.Now, PhaseModel.currentPhase);
        }

        public void SetSpan(DateTime date2, DateTime date1, int count)
        {
            var time = Span(date2, date1, count);
            PhaseModel.nextPhaseSpan = time;
            Trace.WriteLine($"SetSpan {time}");
            ThemeService.SetTimeSpan(time);
        }

        public TimeSpan GetSpan()
        {
            return ThemeService.GetTimeSpan();
        }

        public void SetCurrentImage(Theme theme, Phase phaseModel, DateTime date1, DateTime nowDateTime, Times times)
        {
            var images = GetImagesWithTime(theme.Images, times);

            PhaseService.SetCurrentPhase(times);
            if (images.Count != 0)
            {
                var id = GetSpanId(nowDateTime, date1);
                SetImage(images.Count > id ? images[id].location : images.LastOrDefault()!.location);
            }
            else
            {
                //рекурсия, пока не найдем
                NextTime(times);
                SetCurrentImage(theme, phaseModel, date1, nowDateTime, PhaseModel.nextPhase);
            }
        }


        public void NextTime(Times times)
        {
            switch (times)
            {
                case Times.Dawn:
                    PhaseModel.nextPhase = Times.Sunrise;
                    break;
                case Times.Sunrise:
                    PhaseModel.nextPhase = Times.Day;
                    break;
                case Times.Day:
                    PhaseModel.nextPhase = Times.GoldenHour;
                    break;
                case Times.GoldenHour:
                    PhaseModel.nextPhase = Times.Sunset;
                    break;
                case Times.Sunset:
                    PhaseModel.nextPhase = Times.Night;
                    break;
                case Times.Night:
                    PhaseModel.nextPhase = Times.Dawn;
                    break;
            }
        }

        public TimeSpan Span(DateTime date2, DateTime date1, int count)
        {
            if (count == 0)
                count = 1;
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

        private static List<SunPhase> GetSunPhases(DateTime dateTime,double latitude, double longitude)
        {
            return SunCalcNet.SunCalc.GetSunPhases(dateTime, latitude, longitude).ToList();
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
            return ThemeService.Get() != null;
        }
    }
}