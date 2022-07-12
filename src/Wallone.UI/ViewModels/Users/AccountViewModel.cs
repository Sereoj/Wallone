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
using Wallone.Core.Services.App;
using Wallone.Core.Services.Pages;
using Wallone.Core.Services.Users;
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

        private string country;
        public string Country
        {
            get => country;
            set => SetProperty(ref country, value);
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
            var settings = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder();
            settings.SetToken(null);

            UserRepository.Close();
            SettingsRepository.Save();

            regionManager.RequestNavigate("ContentRegion", "Main");
        }

        private async void OnSave()
        {
            var param = new List<Parameter>();
            if (avatar_path != null) param.Add(new Parameter { Name = "avatar", Type = "file", Value = avatar_path });
            if (AccountRepository.AccountService.GetCover() != null)
                param.Add(new Parameter { Name = "cover", Type = "file", Value = AccountRepository.AccountService.GetCover() });

            _ = await AccountRepository.AccountService.EditUserPageAsync(UpdateUser(), param);
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
                var info = await AccountRepository.AccountService.GetPageGuidsAsync();
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
                var data = await AccountRepository.AccountService.GetPageAsync();

                if (!string.IsNullOrEmpty(data))
                {
                    //var jArray = JArray.Parse(data);
                    account = JsonConvert.DeserializeObject<User>(data);
                    AccountRepository.Load(account);

                    Name = AccountRepository.AccountService.GetUsername();
                    Description = AccountRepository.AccountService.GetDescription();

                    DOB = AccountRepository.AccountService.GetDOB();

                    Github = AccountRepository.AccountService.GetGithub();
                    Facebook = AccountRepository.AccountService.GetFacebook();
                    VK = AccountRepository.AccountService.GetVK();
                    Twitter = AccountRepository.AccountService.GetTwitter();

                    Country = SettingsRepository.Get().User.Country;
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

        private User UpdateUser()
        {
            var list = AccountRepository.GetUser();

            if (Name != AccountRepository.AccountService.GetUsername()) list.username = Name;

            if (Description != AccountRepository.AccountService.GetDescription()) list.description = Description;

            if (DOB != AccountRepository.AccountService.GetDOB()) list.dob = DOB.Value.ToShortDateString();

            if (Github != AccountRepository.AccountService.GetGithub()) list.github = Github;

            if (Facebook != AccountRepository.AccountService.GetFacebook()) list.facebook = Facebook;
            if (VK != AccountRepository.AccountService.GetVK()) list.vk = VK;
            if (Twitter != AccountRepository.AccountService.GetTwitter()) list.twitter = Twitter;

            return list;
        }
    }
}