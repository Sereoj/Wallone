
using System;
using System.Collections.Generic;
using System.Linq;
using Wallone.Core.Builders;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Locations;

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
    }

    public interface ICore
    {
        //Изображения
        string GetPreviousImage(); // Предыдущее
        string GetCurrentImage(); // Текущее
        string GetNextImage(); //Следущее

        //Время
        DateTime GetPreviousDateTime();
        DateTime GetCurrentDateTime();
        DateTime GetNextDateTime();
    }

    public class ThemeWebLocation : ICore
    {
        public string GetPreviousImage()
        {
            return ThemeRepository.ThemeService.GetPrevious();
        }

        public string GetCurrentImage()
        {
            return ThemeRepository.ThemeService.GetCurrentImage();
        }

        public string GetNextImage()
        {
            return ThemeRepository.ThemeService.GetNext();
        }

        public DateTime GetPreviousDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextDateTime()
        {
            throw new NotImplementedException();
        }
    }

    public class ThemeNoUseLocation : ICore
    {
        public string GetPreviousImage()
        {
            return ThemeRepository.ThemeService.GetPrevious();
        }

        public string GetCurrentImage()
        {
            return ThemeRepository.ThemeService.GetCurrentImage();
        }

        public string GetNextImage()
        {
            return ThemeRepository.ThemeService.GetNext();
        }

        public DateTime GetPreviousDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextDateTime()
        {
            throw new NotImplementedException();
        }
    }

    public class ThemeCustomTime : ICore
    {
        public string GetPreviousImage()
        {
            return ThemeRepository.ThemeService.GetPrevious();
        }

        public string GetCurrentImage()
        {
            return ThemeRepository.ThemeService.GetCurrentImage();
        }

        public string GetNextImage()
        {
            return ThemeRepository.ThemeService.GetNext();
        }
        public DateTime GetCurrentDateTime()
        {
            throw new NotImplementedException();
        }



        public DateTime GetNextDateTime()
        {
            throw new NotImplementedException();
        }


        public DateTime GetPreviousDateTime()
        {
            throw new NotImplementedException();
        }
    }

    public class ThemeWindowsLocation : ICore
    {
        public string GetPreviousImage()
        {
            return ThemeRepository.ThemeService.GetPrevious();
        }

        public string GetCurrentImage()
        {
            return ThemeRepository.ThemeService.GetCurrentImage();
        }

        public string GetNextImage()
        {
            return ThemeRepository.ThemeService.GetNext();
        }

        public DateTime GetPreviousDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentDateTime()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextDateTime()
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

            switch (geolocationController.GetGeolocationMode())
            {
                case Mode.UseWebLocation:
                    core = new ThemeWebLocation();
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
                    core = new ThemeWebLocation();
                    LocationService.SetLocation(Mode.UseWebLocation, true);
                    break;
            }
            ThemeRepository.ThemeService.SetImageId("2");
            ThemeRepository.ThemeService.Set(themeModel);
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
    }
}