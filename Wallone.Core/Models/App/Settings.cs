using System;
using Prism.Mvvm;
using Wallone.Core.Interfaces;

namespace Wallone.Core.Models.App
{
    public class Settings : BindableBase, ISettings
    {
        private string email;

        [NonSerialized] private string host;

        private string language;

        //Город
        private string location;

        //Режим
        private int locationMode;

        [NonSerialized] private string prefix;

        private string readme;

        private string theme;

        private string token; // Для авторизации

        public string README
        {
            get => readme;
            set => SetProperty(ref readme, value);
        }

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public int LocationMode
        {
            get => locationMode;
            set => SetProperty(ref locationMode, value);
        }

        public string Prefix
        {
            get => prefix;
            set => SetProperty(ref prefix, value);
        }

        public string Theme
        {
            get => theme;
            set => SetProperty(ref theme, value);
        }

        public string Language
        {
            get => language;
            set => SetProperty(ref language, value);
        }

        public string Host
        {
            get => host;
            set => SetProperty(ref host, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Token
        {
            get => token;
            set => SetProperty(ref token, value);
        }
    }
}