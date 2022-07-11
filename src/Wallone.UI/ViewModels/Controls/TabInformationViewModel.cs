using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class TabInformationViewModel : BindableBase, INavigationAware
    {
        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
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

        public TabInformationViewModel()
        {
        }

        public async void Start(string text)
        {
            Text = text ?? "Возникла ошибка";
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
            await Task.CompletedTask;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            string text = (string)navigationContext.Parameters["text"];
            Start(text);
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