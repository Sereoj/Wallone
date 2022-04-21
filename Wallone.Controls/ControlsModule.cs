using Prism.Ioc;
using Prism.Modularity;
using Wallone.Controls.ViewModels;
using Wallone.Controls.Views;

namespace Wallone.Controls
{
    public class ControlsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoadingControl, LoadingViewModel>(); //Загрузка данных
            containerRegistry.RegisterForNavigation<NoNetworkControl, NoNetworkViewModel>(); //Соединение
            containerRegistry.RegisterForNavigation<NoConnectServer, NoConnectServerViewModel>(); //Сервер
            containerRegistry.RegisterForNavigation<UpdateControl, UpdateViewModel>(); //Обновление
            containerRegistry.RegisterForNavigation<NoItemControl, NoItemViewModel>(); //Нет результата
        }
    }
}