using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class TabSunTimesViewModel : BindableBase, INavigationAware
    {
        private string sunriseTime;

        public string SunriseTime
        {
            get => sunriseTime;
            set => SetProperty(ref sunriseTime, value);
        }

        private string dayTime;
        public string DayTime
        {
            get => dayTime;
            set => SetProperty(ref dayTime, value);
        }

        private string sunsetTime;
        public string SunsetTime
        {
            get => sunsetTime;
            set => SetProperty(ref sunsetTime, value);
        }

        private string nightTime;
        public string NightTime
        {
            get => nightTime;
            set => SetProperty(ref nightTime, value);
        }

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        private double opacity = 0;
        public double Opacity
        {
            get => opacity;
            set => SetProperty(ref opacity, value);
        }

        public TabSunTimesViewModel()
        {
        }

        public async void Start()
        {
            var phase = PhaseService.GetPhase();

            SunriseTime = phase.sunriseSolarTime.ToLocalTime().ToShortTimeString();
            DayTime = phase.daySolarTime.ToLocalTime().ToShortTimeString();
            SunsetTime = phase.sunsetSolarTime.ToLocalTime().ToShortTimeString();
            NightTime = phase.duskSolarTime.ToLocalTime().ToShortTimeString();

            Opacity = 0;
            IsVisible = true;
            for (int i = 0; i <= 10; i++)
            {
                if (Opacity <= 1)
                {
                    await Task.Delay(50);
                    Opacity += 0.1;
                }
            }

            Opacity = 1.0;
            await Task.Delay(5000);

            for (int i = 0; i <= 10; i++)
            {
                if (Opacity >= 0)
                {
                    await Task.Delay(50);
                    Opacity -= 0.1;
                }
            }
            IsVisible = false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Start();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}