using WinDynamicDesktop.Core.Interfaces;

namespace WinDynamicDesktop.Core.Builders
{
    //Созданная тема, где пользователь может скачать, установить, удалить.
    public class ThemeCreatedBuilder : IThemeCreatedBuilder
    {
        // Скачать тему
        public ThemeCreatedBuilder Download()
        {
            return this;
        }

        //Удалить тему
        public ThemeCreatedBuilder Remove()
        {
            return this;
        }

        //Скачать и установить
        public ThemeCreatedBuilder DownloadAndInstall()
        {
            return this;
        }

        //Установить атрибуты к папке
        public ThemeCreatedBuilder SetAttibuteDirectory()
        {
            return this;
        }

        //Установить атрибуты к файлам
        public ThemeCreatedBuilder SetAttibuteFiles()
        {
            return this;
        }

        //Проверка на существования
        public bool Exist()
        {
            return false;
        }
    }
}