
using System;
using System.Collections.Generic;
using System.Linq;
using SunCalcNet.Model;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Locations;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Controllers
{
    public class GeolocationController<T>
    {
        private Location geolocationItems;

        public GeolocationController(Location geolocationItems)
        {
            this.geolocationItems = geolocationItems;
            ValidateMode(GetGeolocationMode());
        }

        public Mode GetGeolocationMode()
        {
            return new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetMode();
        }

        private void ValidateMode(Mode mode)
        {
            bool isGeolocation;
            switch (mode)
            {
                case Mode.UseWebLocation:
                    isGeolocation = true;
                    break;
                case Mode.NoUseLocation:
                    isGeolocation = false;
                    break;
                case Mode.UseCustomTime:
                    isGeolocation = true;
                    break;
                case Mode.UseWindowsLocation:
                    isGeolocation = true;
                    break;
                default:
                    mode = Mode.UseWebLocation;
                    isGeolocation = true;
                    break;
            }

            LocationService.SetLocation(mode, isGeolocation);
        }

        public Location GetLocation()
        {
            return new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetLocation();
        }
    }

    public interface ICore
    {
        //Изображения
        string GetPreviousImage(); // Предыдущее
        string GetCurrentImage(); // Текущее
        string GetNextImage(); //Следущее

        //Время
        DateTime GetPreviousDateTime(Times times);
        DateTime GetCurrentDateTime(Times times);
        DateTime GetNextDateTime(Times times);
        Times GetPhase();
    }

    public class ThemeWebLocation : ICore
    {
        private readonly Location getLocation;

        public ThemeWebLocation(Location getLocation)
        {
            this.getLocation = getLocation;

            DateTime dateTime = DateTime.Now;

            var phaseModel = PhaseRepository.Set(new Phase());

            SetLocationForPhases(phaseModel, dateTime); // Устанавливаем время на каждый промежуток.

            FindCurrentPhase(dateTime);
            CreateCollection();
            GetCurrentImage();
        }

        /// <summary>
        /// Создание коллекции с изображениями с периодом времени.
        /// </summary>
        private void CreateCollection()
        {
            var images = GetImagesWithPhase(ThemeRepository.GetImagesOrderBy(), PhaseRepository.PhaseService.CurrentPhase());

            //Если есть изображения в данном Phase
            if(images != null)
            {
                int index = 0;
                int count = images.Count;

                var StartedAt = GetCurrentDateTime(PhaseRepository.PhaseService.CurrentPhase());
                var EndedAt = GetCurrentDateTime(PhaseRepository.PhaseService.GetNextPhase());
                var nextImagesTimeSpan = PhaseRepository.Math.CalcSunTimes(StartedAt, EndedAt, count); // следующее изображение появится в то время..

                foreach (var item in images)
                {
                    index++;
                    //Начало
                    if (images.FirstOrDefault().id == item.id)
                    {
                        ThemeRepository.Collection.Add(new ThemeCollection()
                        {
                            Id = item.id,
                            Location = item.location,
                            Phase = item.times,
                            Duration = nextImagesTimeSpan,
                            StartedAt = StartedAt,
                            EndAt = StartedAt + nextImagesTimeSpan,
                        });
                        continue;
                    }

                    ThemeRepository.Collection.Add(new ThemeCollection()
                    {
                        Id = item.id,
                        Location = item.location,
                        Phase = item.times,
                        Duration = nextImagesTimeSpan,
                        StartedAt = StartedAt + nextImagesTimeSpan * (index - 1),
                        EndAt = StartedAt + (nextImagesTimeSpan * index),
                    });
                }
                PhaseRepository.Time.SetNextTime(nextImagesTimeSpan);
            }
            else
            {
                ThemeRepository.Collection.Clear();
                PhaseRepository.PhaseService.NextPhase(); // меняем на следующее
                CreateCollection(); // снова выполняем эту функцию пока не найдем.
            }
        }

        /// <summary>
        /// Поиск текущего Phase
        /// </summary>
        /// <param name="dateTime"></param>
        private void FindCurrentPhase(DateTime dateTime)
        {
            IsDawnSolarTime(dateTime);
            IsSunriseSolarTime(dateTime);
            IsDaySolarTime(dateTime);
            IsGoldenSolarTime(dateTime);
            IsSunsetSolarTime(dateTime);
            IsDuskSolarTime(dateTime);
        }

        public string GetPreviousImage()
        {
            LoggerService.Log(null, null);

            var collection = ThemeRepository.Collection.Get();

            var oldDateTime = DateTime.Now - PhaseRepository.Time.GetNextTime();

            foreach (var item in collection)
            {
                if (item.StartedAt < oldDateTime && oldDateTime < item.EndAt)
                {
                    return ThemeRepository.ThemeService.GetCurrentImage(item);
                }
            }
            return null;
        }

        public string GetCurrentImage()
        {
            LoggerService.Log(null, null);

            var collection = ThemeRepository.Collection.Get();

            foreach (var item in collection)
            {
                if(item.StartedAt < DateTime.Now && DateTime.Now < item.EndAt)
                {
                    ThemeRepository.ThemeService.SetImageId(item.Id);
                    return ThemeRepository.ThemeService.GetCurrentImage(item);
                }
            }
            return null;
        }

        public string GetNextImage()
        {
            LoggerService.Log(null, null);

            var collection = ThemeRepository.Collection.Get();

            var newDateTime = DateTime.Now + PhaseRepository.Time.GetNextTime();

            foreach (var item in collection)
            {
                if (item.StartedAt < newDateTime && newDateTime < item.EndAt)
                {
                    return ThemeRepository.ThemeService.GetCurrentImage(item);
                }
            }
            return null;
        }

        public DateTime GetPreviousDateTime(Times currentPhase)
        {
            switch (currentPhase)
            {
                case Times.Dawn:
                    return PhaseRepository.Get().duskSolarTime;
                case Times.Sunrise:
                    return PhaseRepository.Get().dawnSolarTime;
                case Times.Day:
                    return PhaseRepository.Get().sunriseSolarTime;
                case Times.GoldenHour:
                    return PhaseRepository.Get().daySolarTime;
                case Times.Sunset:
                    return PhaseRepository.Get().goldenSolarTime;
                case Times.Night:
                    return PhaseRepository.Get().dawnSolarTime;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DateTime GetCurrentDateTime(Times times)
        {
            switch (times)
            {
                case Times.Dawn:
                    return PhaseRepository.Get().dawnSolarTime;
                case Times.Sunrise:
                    return PhaseRepository.Get().sunriseSolarTime;
                case Times.Day:
                    return PhaseRepository.Get().daySolarTime;
                case Times.GoldenHour:
                    return PhaseRepository.Get().goldenSolarTime;
                case Times.Sunset:
                    return PhaseRepository.Get().sunsetSolarTime;
                case Times.Night:
                    return PhaseRepository.Get().duskSolarTime;
                default:
                    PhaseRepository.PhaseService.SetCurrentPhase(Times.NotFound);
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DateTime GetNextDateTime(Times currentPhase)
        {
            switch (currentPhase)
            {
                case Times.Dawn:
                    return PhaseRepository.Get().sunriseSolarTime;
                case Times.Sunrise:
                    return PhaseRepository.Get().daySolarTime;
                case Times.Day:
                    return PhaseRepository.Get().goldenSolarTime;
                case Times.GoldenHour:
                    return PhaseRepository.Get().sunsetSolarTime;
                case Times.Sunset:
                    return PhaseRepository.Get().duskSolarTime;
                case Times.Night:
                    return PhaseRepository.Get().dawnSolarTime;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Задать необходимые промежутки
        /// </summary>
        /// <param name="nowDateTime"></param>
        private void SetLocationForPhases(Phase phaseModel, DateTime nowDateTime)
        {
            var sunPhases = GetSunPhases(nowDateTime, getLocation.latitude, getLocation.longitude).ToList();

            phaseModel.dawnSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dawn);
            phaseModel.sunriseSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunrise);
            phaseModel.daySolarTime = GetSolarTime(sunPhases, SunPhaseName.SolarNoon);
            phaseModel.goldenSolarTime = GetSolarTime(sunPhases, SunPhaseName.GoldenHour);
            phaseModel.sunsetSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunset);
            phaseModel.duskSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dusk);
            phaseModel.nightSolarTime = GetSolarTime(sunPhases, SunPhaseName.Night);
            PhaseRepository.Load(phaseModel);
        }

        private void UpdateLocationForPhases(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var sunPhases = GetSunPhases(nowDateTime, getLocation.latitude, getLocation.longitude).ToList();

            phaseModel.dawnSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dawn);
            phaseModel.sunriseSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunrise);
            phaseModel.daySolarTime = GetSolarTime(sunPhases, SunPhaseName.SolarNoon);
            PhaseRepository.Load(phaseModel);
        }

        /// <summary>
        /// Проверка на раннее утро
        /// </summary>
        /// <param name="nowDateTime"></param>
        /// <returns></returns>
        protected bool IsDawnSolarTime(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var date1 = phaseModel.dawnSolarTime;
            var date2 = phaseModel.sunriseSolarTime;

            if (date1 < nowDateTime && date2 > nowDateTime)
            {
                PhaseRepository.PhaseService.SetCurrentPhase(Times.Dawn);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на утро
        /// </summary>
        /// <param name="nowDateTime"></param>
        /// <returns></returns>
        protected bool IsSunriseSolarTime(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var date1 = phaseModel.sunriseSolarTime;
            var date2 = phaseModel.daySolarTime;

            if (date1 < nowDateTime && date2 > nowDateTime)
            {
                PhaseRepository.PhaseService.SetCurrentPhase(Times.Sunrise);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на полдень
        /// </summary>
        /// <param name="nowDateTime"></param>
        /// <returns></returns>
        protected bool IsDaySolarTime(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var date1 = phaseModel.daySolarTime;
            var date2 = phaseModel.goldenSolarTime;

            if (date1 < nowDateTime && date2 > nowDateTime)
            {
                PhaseRepository.PhaseService.SetCurrentPhase(Times.Day);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Золотой час, солнце опускается
        /// </summary>
        /// <param name="nowDateTime"></param>
        /// <returns></returns>
        protected bool IsGoldenSolarTime(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var date1 = phaseModel.goldenSolarTime;
            var date2 = phaseModel.sunsetSolarTime;
            if (date1 < nowDateTime && date2 > nowDateTime)
            {
                PhaseRepository.PhaseService.SetCurrentPhase(Times.GoldenHour);
                return true;
            }
            return false;
        }

        protected bool IsSunsetSolarTime(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var date1 = phaseModel.sunsetSolarTime;
            var date2 = phaseModel.duskSolarTime;

            if (date1 < nowDateTime && date2 > nowDateTime)
            {
                PhaseRepository.PhaseService.SetCurrentPhase(Times.Sunset);
                return true;
            }
            return false;
        }

        protected bool IsDuskSolarTime(DateTime nowDateTime)
        {
            var phaseModel = PhaseRepository.Get();

            var date1 = phaseModel.duskSolarTime;

            if (date1 < nowDateTime)
            {
                PhaseRepository.PhaseService.SetCurrentPhase(Times.Night);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить весь список 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private static List<SunPhase> GetSunPhases(DateTime dateTime, double latitude, double longitude)
        {
            return SunCalcNet.SunCalc.GetSunPhases(dateTime, latitude, longitude).ToList();
        }

        /// <summary>
        /// Получить определенный промежуток
        /// </summary>
        /// <param name="sunPhases"></param>
        /// <param name="desiredPhase"></param>
        /// <returns></returns>
        private static DateTime GetSolarTime(List<SunPhase> sunPhases, SunPhaseName desiredPhase)
        {
            SunPhase sunPhase = sunPhases.FirstOrDefault(sp => sp.Name.Value == desiredPhase.Value);
            return sunPhase.PhaseTime.ToLocalTime();
        }

        public List<Image> GetImagesWithPhase(List<Image> images, Times time)
        {
            return images.Where(image => image.times == time).ToList();
        }

        public Times GetPhase()
        {
            return PhaseRepository.Get().currentPhase;
        }
    }

    public class ThemeNoUseLocation : ICore
    {
        public string GetPreviousImage()
        {
            return null;
        }

        public string GetCurrentImage()
        {
            return null;
        }

        public string GetNextImage()
        {
            return null;
        }

        public DateTime GetPreviousDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextDateTime(Times times)
        {
            throw new NotImplementedException();
        }
        public Times GetPhase()
        {
            return PhaseRepository.Get().currentPhase;
        }
    }

    public class ThemeCustomTime : ICore
    {
        public string GetPreviousImage()
        {
            return null;
        }

        public string GetCurrentImage()
        {
            return null;
        }

        public string GetNextImage()
        {
            return null;
        }

        public DateTime GetCurrentDateTime(Times times)
        {
            throw new NotImplementedException();
        }



        public DateTime GetNextDateTime(Times times)
        {
            throw new NotImplementedException();
        }


        public DateTime GetPreviousDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public Times GetPhase()
        {
            return PhaseRepository.Get().currentPhase;
        }
    }

    public class ThemeWindowsLocation : ICore
    {
        public string GetPreviousImage()
        {
            return null;
        }

        public string GetCurrentImage()
        {
            return null;
        }

        public string GetNextImage()
        {
            return null;
        }

        public DateTime GetPreviousDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public Times GetPhase()
        {
            return PhaseRepository.Get().currentPhase;
        }
    }

    public class ThemeController<T>
    {
        private readonly Theme themeModel;
        private GeolocationController<Location> geolocationController;
        private ICore core;

        public ThemeController(Theme themeModel, GeolocationController<Location> geolocationController)
        {
            this.themeModel = themeModel;
            this.geolocationController = geolocationController;

            ThemeRepository.ThemeService.Set(themeModel);

            switch (geolocationController.GetGeolocationMode())
            {
                case Mode.UseWebLocation:
                    core = new ThemeWebLocation(geolocationController.GetLocation());
                    break;
                case Mode.NoUseLocation:
                    core = new ThemeNoUseLocation();
                    break;
                case Mode.UseCustomTime:
                    core = new ThemeCustomTime();
                    break;
                case Mode.UseWindowsLocation:
                    core = new ThemeWindowsLocation();
                    break;
                default:
                    core = new ThemeWebLocation(geolocationController.GetLocation());
                    LocationService.SetLocation(Mode.UseWebLocation, true);
                    break;
            }
        }

        public ICore Core()
        {
            return core;
        }

        public bool ValidateFields()
        {
            if (!string.IsNullOrEmpty(themeModel.Uuid))
            {
                if (themeModel.Images != null)
                {
                    return true;
                }
            }

            return false;
        }

        public string GetThemeName()
        {
            return AppFormat.Format(ThemeRepository.Get().Name);
        }
    }
}