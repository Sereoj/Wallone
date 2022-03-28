﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Controls.ViewModels
{
    public class NoConnectServerViewModel : BindableBase
    {
        private bool status;
        public bool Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }
        public void SetStatus(bool status)
        {
            Status = status;
        }

        public bool IsShow()
        {
            return Status;
        }
    }
}