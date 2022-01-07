using System;
using Prism.Mvvm;

namespace WinDynamicDesktop.Core.Models.App
{
    [Serializable]
    public class Settings : BindableBase
    {
        public string Token { get; set; } // Для авторизации
        public string Language { get; set; } // Выбор язык
        // Светлая, ночная, автоопределение
        public string Theme { get; set; } // Тема
    }
}
