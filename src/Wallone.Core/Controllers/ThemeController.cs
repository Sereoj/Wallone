
using System;
using System.Collections.Generic;
using System.Linq;
using SunCalcNet.Model;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Schedulers;
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
        void Init();
        //Изображения
        string GetPreviousImage(); // Предыдущее
        string GetCurrentImage(); // Текущее
        string GetNextImage(); //Следущее
        void SkipPhase();
        bool IsNotNull();

        //Время и Phase
        DateTime GetPreviousPhaseDateTime(Times times);
        DateTime GetCurrentPhaseDateTime(Times times);
        DateTime GetNextPhaseDateTime(Times times);
        Times GetPhase();

        //Время и изображения в Phase
        TimeSpan GetNextImageDateTime();
    }

    public class ThemeWebLocation : ICore
    {
        private readonly Location getLocation;
        private bool isNotNull;
        private Phase phaseNextDay;
        public ThemeWebLocation(Location getLocation)
        {
            this.getLocation = getLocation;
            DateTime dateTime = DateTime.Now;
            var phaseModel = PhaseRepository.Set(new Phase());
            phaseModel = SetLocationForPhases(phaseModel, dateTime); // Устанавливаем время на каждый промежуток.
            PhaseRepository.Load(phaseModel);
        }

        public void Init()
        {
            DateTime dateTime = DateTime.Now;
            FindCurrentPhase(dateTime);
            CreateCollection(dateTime);
        }

        public List<Image> GetImagesWithPhase(List<Image> images, Times time)
        {
            return images.Where(image => image.times == time).ToList();
        }
        public Times GetPhase()
        {
            return PhaseRepository.Get().currentPhase;
        }

        public string GetPreviousImage()
        {
            _ = LoggerService.LogAsync(null, null);

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
            _ = LoggerService.LogAsync(null, null);
            var collection = ThemeRepository.Collection.Get();
            _ = LoggerService.LogAsync(this, $"Collection {collection.Count}");
            foreach (var item in collection)
            {
                _ = LoggerService.LogAsync(this, $"S: {item.StartedAt} N: {DateTime.Now} E: {item.EndAt}");
                if(item.StartedAt < DateTime.Now && DateTime.Now < item.EndAt)
                {
                    ThemeRepository.ThemeService.SetImageId(item.Id);
                    return ThemeRepository.ThemeService.GetCurrentImage(item);
                }
            }
            return ThemeRepository.ThemeService.GetCurrentImage(GetPhase());
        }

        public string GetNextImage()
        {
            _ = LoggerService.LogAsync(null, null);

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

        public TimeSpan GetNextImageDateTime()
        {
            var nextTime = PhaseRepository.Time.GetNextTime();
            if (nextTime.TotalSeconds != 0)
            {
                return PhaseRepository.Time.GetNextTime();
            }
            return TimeSpan.FromSeconds(10);
        }

        public DateTime GetPreviousPhaseDateTime(Times currentPhase)
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
        public DateTime GetCurrentPhaseDateTime(Times times)
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
        public DateTime GetNextPhaseDateTime(Times currentPhase)
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

        public bool IsNotNull()
        {
            return isNotNull;
        }
        public void SkipPhase()
        {
            DateTime nowDateTime = DateTime.Now;

            PhaseRepository.PhaseService.NextPhase(); // меняем на следующее
            CreateCollection(nowDateTime); // снова выполняем эту функцию пока не найдем.
            _ = LoggerService.LogAsync(this, $"Пропуск фазы: {PhaseRepository.PhaseService.GetPreviousPhase()}");
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

        /// <summary>
        /// Создание коллекции с изображениями с периодом времени.
        /// </summary>
        private void CreateCollection(DateTime nowDateTime)
        {
            ThemeRepository.Collection.Clear();

            var images = GetImagesWithPhase(ThemeRepository.GetImagesOrderBy(), PhaseRepository.PhaseService.CurrentPhase());

            //Если есть изображения в данном Phase
            if (images != null)
            {
                int index = 0;
                int count = images.Count;
                if (images.Count > 0)
                {
                    var currentPhase = PhaseRepository.PhaseService.CurrentPhase();
                    var nextPhase = PhaseRepository.PhaseService.GetNextPhase();

                    var StartedAt = GetCurrentPhaseDateTime(currentPhase);
                    var EndedAt = GetCurrentPhaseDateTime(nextPhase);

                    if (StartedAt > EndedAt)
                    {
                        UpdateLocationForPhases(nowDateTime);
                        EndedAt = phaseNextDay.dawnSolarTime;
                    }

                    var nextImagesTimeSpan = PhaseRepository.Math.CalcSunTimes(StartedAt, EndedAt, count); // следующее изображение появится в то время..
                    
                    PhaseRepository.Time.SetNextTime(nextImagesTimeSpan);

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
                    isNotNull = true;
                }
                else
                {
                    isNotNull = false;
                    PhaseRepository.Time.SetNextTime(TimeSpan.FromSeconds(10));
                }
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

        /// <summary>
        /// Задать необходимые промежутки
        /// </summary>
        /// <param name="nowDateTime"></param>
        private Phase SetLocationForPhases(Phase phaseModel, DateTime nowDateTime)
        {
            var sunPhases = GetSunPhases(nowDateTime, getLocation.latitude, getLocation.longitude).ToList();

            phaseModel.dawnSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dawn);
            phaseModel.sunriseSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunrise);
            phaseModel.daySolarTime = GetSolarTime(sunPhases, SunPhaseName.SolarNoon);
            phaseModel.goldenSolarTime = GetSolarTime(sunPhases, SunPhaseName.GoldenHour);
            phaseModel.sunsetSolarTime = GetSolarTime(sunPhases, SunPhaseName.Sunset);
            phaseModel.duskSolarTime = GetSolarTime(sunPhases, SunPhaseName.Dusk);
            phaseModel.nightSolarTime = GetSolarTime(sunPhases, SunPhaseName.Night);

            //phaseModel.dawnSolarTime = nowDateTime = nowDateTime.AddSeconds(30);
            //phaseModel.sunriseSolarTime = nowDateTime = nowDateTime.AddSeconds(30);
            //phaseModel.daySolarTime = nowDateTime = nowDateTime.AddSeconds(30);
            //phaseModel.goldenSolarTime = nowDateTime = nowDateTime.AddSeconds(30);
            //phaseModel.sunsetSolarTime = nowDateTime = nowDateTime.AddSeconds(30);
            //phaseModel.duskSolarTime = nowDateTime = nowDateTime.AddSeconds(30);
            //phaseModel.nightSolarTime = nowDateTime = nowDateTime.AddSeconds(30);

            SetLocationForPhasesLogger(phaseModel);

            return phaseModel;
        }

        private void SetLocationForPhasesLogger(Phase phaseModel)
        {
            _ = LoggerService.LogAsync(this, $"dawnSolarTime: {phaseModel.dawnSolarTime}");
            _ = LoggerService.LogAsync(this, $"sunriseSolarTime: {phaseModel.sunriseSolarTime}");
            _ = LoggerService.LogAsync(this, $"daySolarTime: {phaseModel.daySolarTime}");
            _ = LoggerService.LogAsync(this, $"goldenSolarTime: {phaseModel.goldenSolarTime}");
            _ = LoggerService.LogAsync(this, $"sunsetSolarTime: {phaseModel.sunsetSolarTime}");
            _ = LoggerService.LogAsync(this, $"duskSolarTime: {phaseModel.duskSolarTime}");
            _ = LoggerService.LogAsync(this, $"nightSolarTime: {phaseModel.nightSolarTime}");
        }

        public void UpdateLocationForPhases(DateTime nowDateTime)
        {
            phaseNextDay = SetLocationForPhases(new Phase(), nowDateTime.AddDays(1));
        }
    }

    public class ThemeNoUseLocation : ICore
    {
        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetNextImageDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public Times GetPhase()
        {
            throw new NotImplementedException();
        }

        public string GetPreviousImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetPreviousPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsNotNull()
        {
            throw new NotImplementedException();
        }

        public void SkipPhase()
        {
            throw new NotImplementedException();
        }
    }

    public class ThemeCustomTime : ICore
    {
        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetNextImageDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public Times GetPhase()
        {
            throw new NotImplementedException();
        }

        public string GetPreviousImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetPreviousPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsNotNull()
        {
            throw new NotImplementedException();
        }

        public void SkipPhase()
        {
            throw new NotImplementedException();
        }
    }

    public class ThemeWindowsLocation : ICore
    {
        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetNextImageDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public Times GetPhase()
        {
            throw new NotImplementedException();
        }

        public string GetPreviousImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetPreviousPhaseDateTime(Times times)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsNotNull()
        {
            throw new NotImplementedException();
        }

        public void SkipPhase()
        {
            throw new NotImplementedException();
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