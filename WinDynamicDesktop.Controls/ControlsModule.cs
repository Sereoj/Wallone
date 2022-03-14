using Prism.Ioc;
using Prism.Modularity;

namespace WinDynamicDesktop.Controls
{
    public class ControlsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.LoadingControl, ViewModels.LoadingViewModel>(); //Загрузка данных
            containerRegistry.RegisterForNavigation<Views.NoNetworkControl, ViewModels.NoNetworkViewModel>(); //Соединение
            containerRegistry.RegisterForNavigation<Views.NoConnectServer, ViewModels.NoConnectServerViewModel>(); //Сервер
            containerRegistry.RegisterForNavigation<Views.UpdateControl, ViewModels.UpdateViewModel>(); //Обновление
            containerRegistry.RegisterForNavigation<Views.NoItemControl, ViewModels.NoItemViewModel>(); //Нет результата
        }
    }
}
