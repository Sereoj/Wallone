
using System;
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
            throw new NotImplementedException();
        }

        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
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
        public DateTime GetCurrentDateTime()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetNextDateTime()
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
        }

        public DateTime GetPreviousDateTime()
        {
            throw new NotImplementedException();
        }

        public string GetPreviousImage()
        {
            throw new NotImplementedException();
        }
    }

    public class ThemeWindowsLocation : ICore
    {
        public string GetPreviousImage()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentImage()
        {
            throw new NotImplementedException();
        }

        public string GetNextImage()
        {
            throw new NotImplementedException();
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
        private readonly Theme controller;
        private GeolocationController<Location> geolocationController;
        private ICore core;
        public ThemeController(Theme controller, GeolocationController<Location> geolocationController)
        {
            this.controller = controller;
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
            ThemeRepository.ThemeService.Set(controller);
        }

        public ICore Core()
        {
            return core;
        }

        public bool ValidateFields()
        {
            if (!string.IsNullOrEmpty(controller.Uuid))
            {
                if (controller.Images != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}