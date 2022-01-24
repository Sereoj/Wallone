using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ItemTemplateViewModel : BindableBase
    {
        private string text;
        public string Text { get => text; set => SetProperty(ref text, value); }
        public ItemTemplateViewModel()
        {

        }
    }
}
