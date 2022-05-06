using Prism.Mvvm;
using System;
using Wallone.Core.Interfaces;

namespace Wallone.Core.Models.App
{
    public class Settings : BindableBase, ISettings
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Language { get; set; }
        public string WindowTheme { get; set; }
        public string Theme { get; set; }
        public string Host { get; set; }
        public string Prefix { get; set; }
        public string README { get; set; }
    }
}