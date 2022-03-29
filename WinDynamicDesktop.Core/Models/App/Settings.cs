﻿using Prism.Mvvm;
using System;
using WinDynamicDesktop.Core.Interfaces;

namespace WinDynamicDesktop.Core.Models.App
{
    public class Settings : BindableBase, ISettings
    {
        //Город
        private string location;
        public string Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        //Режим
        private int locationMode;
        public int LocationMode
        {
            get { return locationMode; }
            set { SetProperty(ref locationMode, value); }
        }

        private string theme;
        public string Theme
        {
            get { return theme; }
            set { SetProperty(ref theme, value); }
        }

        private string language;
        public string Language
        {
            get { return language; }
            set { SetProperty(ref language, value); }
        }

        [NonSerialized]
        private string host;
        public string Host
        {
            get { return host; }
            set { SetProperty(ref host, value); }
        }

        [NonSerialized]
        private string prefix;
        public string Prefix
        {
            get { return prefix; }
            set { SetProperty(ref prefix, value); }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string token; // Для авторизации
        public string Token
        {
            get { return token; }
            set { SetProperty(ref token, value); }
        }
    }
}
