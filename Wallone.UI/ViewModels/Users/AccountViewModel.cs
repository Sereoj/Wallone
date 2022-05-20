using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Users
{
    public class AccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private User account;

        private Uri avatar;
        private string avatar_path;

        private Uri cover;

        private string description;

        private DateTime? dOB;

        private string facebook;

        private string github;

        private string header = "Аккаунт";

        private string name;

        private string textInformation;

        private string twitter;

        private string vk;

        public AccountViewModel()
        {
        }

        public AccountViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            PersonPictureCommand = new DelegateCommand(OnPersonPicture);
            SaveCommand = new DelegateCommand(OnSave);
            ExitCommand = new DelegateCommand(OnExit);
        }

        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Uri Avatar
        {
            get => avatar;
            set
            {
                SetProperty(ref avatar, value);
                RaisePropertyChanged();
            }
        }

        public Uri Cover
        {
            get => cover;
            set
            {
                SetProperty(ref cover, value);
                RaisePropertyChanged();
            }
        }

        public DateTime? DOB
        {
            get => dOB;
            set => SetProperty(ref dOB, value);
        }

        public string Github
        {
            get => github;
            set => SetProperty(ref github, value);
        }

        public string Facebook
        {
            get => facebook;
            set => SetProperty(ref facebook, value);
        }

        public string VK
        {
            get => vk;
            set => SetProperty(ref vk, value);
        }

        public string Twitter
        {
            get => twitter;
            set => SetProperty(ref twitter, value);
        }

        public string TextInformation
        {
            get => textInformation;
            set => SetProperty(ref textInformation, value);
        }

        public DelegateCommand PersonPictureCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Loaded();
            LoadText();
        }

        private void OnExit()
        {
            var settings = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();
            settings.SetToken(null);

            UserService.Close();
            SettingsService.Save();

            regionManager.RequestNavigate("ContentRegion", "Login");
        }

        private async void OnSave()
        {
            var param = new List<Parameter>();
            if (avatar_path != null) param.Add(new Parameter { Name = "avatar", Type = "file", Value = avatar_path });
            if (AccountService.GetCover() != null)
                param.Add(new Parameter { Name = "cover", Type = "file", Value = AccountService.GetCover() });

            _ = await AccountService.EditUserPageAsync(update(), param);
        }

        private void OnPersonPicture()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                avatar_path = fileDialog.FileName;
                Avatar = UriHelper.Get(fileDialog.FileName);
            }
        }

        public async void LoadText()
        {
            try
            {
                var info = await AccountService.GetPageGuidsAsync();
                if (!string.IsNullOrEmpty(info))
                {
                    var text = JsonConvert.DeserializeObject<Advertisement>(info);

                    TextInformation = text?.text ?? "Информация не доступна";
                }
            }
            catch (Exception ex)
            {
                TextInformation = ex.Message;
            }
        }

        public async void Loaded()
        {
            try
            {
                var data = await AccountService.GetPageAsync();

                if (!string.IsNullOrEmpty(data))
                {
                    //var jArray = JArray.Parse(data);
                    account = JsonConvert.DeserializeObject<User>(data);
                    AccountService.Load(account);

                    Name = AccountService.GetUsername();
                    Description = AccountService.GetDescription();

                    DOB = AccountService.GetDOB();

                    Github = AccountService.GetGithub();
                    Facebook = AccountService.GetFacebook();
                    VK = AccountService.GetVK();
                    Twitter = AccountService.GetTwitter();
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    {"Text", ex.Message}
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }

        private User update()
        {
            var list = AccountService.getUser();

            if (Name != AccountService.GetUsername()) list.username = Name;

            if (Description != AccountService.GetDescription()) list.description = Description;

            if (DOB != AccountService.GetDOB()) list.dob = DOB.Value.ToShortDateString();

            if (Github != AccountService.GetGithub()) list.github = Github;

            if (Facebook != AccountService.GetFacebook()) list.facebook = Facebook;
            if (VK != AccountService.GetVK()) list.vk = VK;
            if (Twitter != AccountService.GetTwitter()) list.twitter = Twitter;

            return list;
        }
    }
}