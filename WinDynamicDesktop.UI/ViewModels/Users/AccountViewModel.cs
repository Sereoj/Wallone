using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.UI.Services;
using Prism.Commands;
using Microsoft.Win32;
using WinDynamicDesktop.Core.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using WinDynamicDesktop.Core.Services;
using System.Windows.Media.Imaging;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class AccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private static BitmapHelper bitmapHelper = new BitmapHelper();
        private string avatar_path;
        private User account;

        private string header = "Аккаунт";
        public string Header { get => header; set => SetProperty(ref header, value); }

        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private System.Windows.Media.ImageSource avatar;
        public System.Windows.Media.ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }

        private System.Windows.Media.ImageSource cover;
        public System.Windows.Media.ImageSource Cover { get => cover; set => SetProperty(ref cover, value); }

        private System.DateTime? dOB;
        public System.DateTime? DOB { get => dOB; set => SetProperty(ref dOB, value); }

        private string github;
        public string Github { get => github; set => SetProperty(ref github, value); }

        private string facebook;
        public string Facebook { get => facebook; set => SetProperty(ref facebook, value); }

        private string vk;
        public string VK { get => vk; set => SetProperty(ref vk, value); }

        private string twitter;
        public string Twitter { get => twitter; set => SetProperty(ref twitter, value); }

        private string textInformation;
        public string TextInformation { get => textInformation; set => SetProperty(ref textInformation, value); }
        public DelegateCommand PersonPictureCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public AccountViewModel()
        {
        }

        public AccountViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            PersonPictureCommand = new DelegateCommand(OnPersonPicture);
            SaveCommand = new DelegateCommand(OnSave);
        }

        private async void OnSave()
        {
            var param = new List<Parameter>();
            if(avatar_path != null)
            {
                param.Add(new Parameter() { Name = "avatar", Type = "file", Value = avatar_path });
            }
            if(AccountService.GetCover() != null )
            {
                param.Add(new Parameter() { Name = "cover", Type = "file", Value = AccountService.GetCover() });
            }
            var data = await AccountService.EditUserPageAsync(update(), param);
        }

        private void OnPersonPicture()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                avatar_path = fileDialog.FileName;
                Avatar = bitmapHelper[UriHelper.Get(fileDialog.FileName)];
            }
        }

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
        public async void LoadText()
        {
            try
            {
                var info = await AccountService.GetPageGuidsAsync();
                if (!string.IsNullOrEmpty(info))
                {
                    var text = JsonConvert.DeserializeObject<Text>(info);

                    TextInformation = text.text ?? "Информация не доступна";
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
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
        private User update()
        {
            var list = AccountService.getUser();

            if (Name != AccountService.GetUsername())
            {
                list.name = Name;
            }

            if (Description != AccountService.GetDescription())
            {
                list.description = Description;
            }

            if (DOB != AccountService.GetDOB())
            {
                list.dob = DOB.Value.ToShortDateString();
            }

            if (Github != AccountService.GetGithub())
            {
                list.github = Github;
            }

            if (Facebook != AccountService.GetFacebook())
            {
                list.facebook = Facebook;
            }
            if (VK != AccountService.GetVK())
            {
                list.vk = VK;
            }
            if (Twitter != AccountService.GetTwitter())
            {
                list.twitter = Twitter;
            }

            return list;
        }
    }
}
