using MEESEES.Commons;
using MEESEES.Data;
using MEESEES.Models;
using MEESEES.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MEESEES.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private IUserInterface _sqlUser;
        private IPageService _pageService;

        private string _inputUserName;
        public string InputUserName
        {
            get { return _inputUserName; }
            set { SetValue(ref _inputUserName, value); OnPropertyChanged(nameof(InputUserName)); }
        }
        private string _inputPassword;
        public string InputPassword
        {
            get { return _inputPassword; }
            set { SetValue(ref _inputPassword, value); OnPropertyChanged(nameof(InputPassword)); }
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetValue(ref _isRefreshing, value); OnPropertyChanged(nameof(IsRefreshing)); }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetValue(ref _errorMessage, value); OnPropertyChanged(nameof(ErrorMessage)); }
        }
        private string _aboutMessage;
        public string AboutMessage
        {
            get { return _aboutMessage; }
            set { SetValue(ref _aboutMessage, value); OnPropertyChanged(nameof(AboutMessage)); }
        }
        public ObservableCollection<UserViewModel> Users { get; private set; } = new ObservableCollection<UserViewModel>();
        public ICommand LoginCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public LoginViewModel( IPageService pageService, IUserInterface sqluser=null, string about="")
        {
            _sqlUser = sqluser;
            _pageService = pageService;
            AboutMessage = about;
            LoginCommand = new Command(async () => await LoadUser());
            AboutCommand = new Command(async () => await DisplayAbout(about));
            MessagingCenter.Unsubscribe<string>(this, "counterValue");
            MessagingCenter.Subscribe<string>(this, "counterValue", (value) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Globals.serviceCounter = value;
                });
            });

        }
        private async Task DisplayAbout(string about)
        {
            await _pageService.PushAsync(new AboutPage());
            //await _pageService.DisplayAlert("About", about, "OK");
        }
        private async Task LoadUser()
        {
            var users = await _sqlUser.GetUserByUsername(InputUserName);
            if (users.Count() != 0)
            {
                foreach (var user in users)
                {
                    if (user.Password == InputPassword)
                    {
                        Users.Add(new UserViewModel(user));
                        Globals.currentUser = user;
                        Globals.isForNotification = Globals.currentUser.isNotify;
                        break;
                    }
                    else
                    {
                        await _pageService.DisplayAlert("MEESEES", "Login Failed: " + $"Incorrect Password for user: {InputUserName}", "OK");
                        break;
                    }
                }
                if (Globals.currentUser != null)
                {
                    //testing only
                    DependencyService.Get<ILocalNotification>().CreateNotification("MEESEES", $"Welcome Back {Globals.currentUser.Username}",0);
                    Application.Current.MainPage = new MenuPage();
                }
            }
            else
            {
                await _pageService.DisplayAlert("MEESEES", "Login Failed: " + $"User not registered...", "OK");
                return;
            }
        }
    }
}
