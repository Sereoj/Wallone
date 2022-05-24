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

        public void Set(Theme theme, Mode mode)
        {
            var phaseModel = PhaseService.GetPhase();

            var itemBuilder = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            if (theme != null)
            {
                ThemeService.Set(theme);

                var lat = itemBuilder.GetLatitude();
                var lng = itemBuilder.GetLongitude();

                switch (mode)
                {
                    case Mode.UseWebLocation:
                        UseWebLocation(theme, phaseModel, lat, lng);
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
                SetCurrentImage(theme, phaseModel, phaseModel.dawnSolarTime, time, Times.Dawn);
            }
            if (phaseModel.sunriseSolarTime < time && phaseModel.daySolarTime > time)
            {
                Trace.WriteLine("Утро");
                SetCurrentImage(theme, phaseModel, phaseModel.sunriseSolarTime, time, Times.Sunrise);
            }
            if (phaseModel.daySolarTime < time && phaseModel.goldenSolarTime > time)
            {
                Trace.WriteLine("День");
                SetCurrentImage(theme, phaseModel, phaseModel.daySolarTime, time, Times.Day);
            }
            if (phaseModel.goldenSolarTime < time && phaseModel.sunsetSolarTime > time)
            {
                Trace.WriteLine("Золотое время");
                SetCurrentImage(theme, phaseModel, phaseModel.goldenSolarTime, time, Times.GoldenHour);
            }
            if (phaseModel.sunsetSolarTime < time && phaseModel.duskSolarTime > time)
            {
                Trace.WriteLine("Закат");
                SetCurrentImage(theme, phaseModel, phaseModel.sunsetSolarTime, time, Times.Sunset);
            }
        }

        public void SetCurrentImage(Theme theme, Phase phaseModel, DateTime date1, DateTime nowDateTime, Times times)
        {
            var images = GetImagesWithTime(theme.Images, times);
            //TimeSpan timeSpan = Span(phaseModel.goldenSolarTime, phaseModel.daySolarTime, images.Count);
            var id = GetSpanId(nowDateTime, date1);

            SetImage(images.Count >= id ? images[id].location : images.LastOrDefault()!.location);
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
    }
}