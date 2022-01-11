using System;
using Prism.Mvvm;

namespace WinDynamicDesktop.Core.Models.App
{
    public class Settings : BindableBase
    {
        private string token; // Для авторизации
        public string Token
        {
            get { return token; }
            set { SetProperty(ref token, value); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string language;
        public string Language
        {
            get { return language; }
            set { SetProperty(ref language, value); }
        }

        private string theme;
        public string Theme
        {
            get { return theme; }
            set { SetProperty(ref theme, value); }
        }
    }
}
