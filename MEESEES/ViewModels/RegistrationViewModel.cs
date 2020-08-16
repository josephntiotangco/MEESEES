using MEESEES.Commons;
using MEESEES.Data;
using MEESEES.Models;
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
    public class RegistrationViewModel : BaseViewModel
    {
        private IUserInterface _sqlUser;
        private IPageService _pageService;
        private bool _isDataLoaded;
        public User User { get; set; }
        private bool _isNotify;
        public bool IsNotify
        {
            get { return _isNotify; }
            set { SetValue(ref _isNotify, value); OnPropertyChanged(nameof(IsNotify)); }
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
        private UserViewModel _selectedUser;
        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetValue(ref _selectedUser, value); OnPropertyChanged(nameof(SelectedUser)); }
        }
        private string _newUsername;
        public string NewUsername
        {
            get { return _newUsername; }
            set { SetValue(ref _newUsername, value); OnPropertyChanged(nameof(NewUsername)); }
        }
        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetValue(ref _newPassword, value); OnPropertyChanged(nameof(NewPassword)); }
        }
        private decimal _newSavings;
        public decimal NewSavings
        {
            get { return _newSavings; }
            set { SetValue(ref _newSavings, value); OnPropertyChanged(nameof(NewSavings)); }
        }
        public ObservableCollection<UserViewModel> Users { get; private set; } = new ObservableCollection<UserViewModel>();
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadDataCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand RefreshDataCommand { get; private set; }
        public RegistrationViewModel(UserViewModel userView, IUserInterface sqlUser, IPageService pageService, string mode ="")
        {
            _sqlUser = sqlUser;
            _pageService = pageService;

            if (userView == null) throw new ArgumentException(nameof(userView));

            SaveCommand = new Command(async () => await Save(mode));
            LoadDataCommand = new Command(async () => await LoadData(mode));
            RefreshDataCommand = new Command(async () => await RefreshData(mode));
            DeleteUserCommand = new Command<UserViewModel>(async c => await DeleteUser(c,mode));

            //MessagingCenter.Subscribe<RegistrationViewModel, User>(this, Events.UserAdded, OnUserAdded);
            //MessagingCenter.Subscribe<RegistrationViewModel, User>(this, Events.UserUpdated, OnUserUpdated);
        }
        //private void OnUserAdded(RegistrationViewModel source, User User)
        //{
        //    Users.Add(new UserViewModel(User));
        //}
        //private void OnUserUpdated(RegistrationViewModel source, User User)
        //{
        //    var UserInList = Users.Single(c => c.Id == User.Id);

        //    UserInList.Id = User.Id;
        //    UserInList.UserName = User.Username;
        //    UserInList.Password = User.Password;
        //    UserInList.Savings = User.Savings;
        //    UserInList.UpdateDate = User.UpdateDate;
        //    UserInList.IsNotify = User.isNotify;

        //    if(Globals.currentUser.Id == User.Id)
        //    {
        //        Globals.currentUser.Id = User.Id;
        //        Globals.currentUser.Savings = User.Savings;
        //        Globals.currentUser.Username = User.Username;
        //        Globals.currentUser.isNotify = User.isNotify;
        //    }
        //    if(User.isNotify == false)
        //    {
        //        DeactivateNotification();
        //    }
        //}
        public async Task RefreshData(string mode)
        {
            IsRefreshing = true;
            _isDataLoaded = false;
            await LoadData(mode);
            IsRefreshing = false;
        }
        private async Task LoadData(string mode)
        {
            if (_isDataLoaded) return;
            _isDataLoaded = true;
            Users.Clear();
            if(mode == "")
            {
                var users = await _sqlUser.GetUsersAsync();
                foreach (var user in users)
                    Users.Add(new UserViewModel(user));
            }
            else
            {
                var users = await _sqlUser.GetUserById(Globals.currentUser.Id);
                foreach (var user in users)
                    Users.Add(new UserViewModel(user));
            }

        }
        private void DeactivateNotification()
        {
            MessagingCenter.Send<string>("0", "myService");
        }
        private async Task Save(string mode)
        {
            if (User == null)
            {
                User = new User
                {
                    Username = NewUsername,
                    Password = NewPassword,
                    Savings = NewSavings,
                    UpdateDate = Convert.ToDateTime(Globals.phoneDate),
                    isNotify = IsNotify
                };
            }
            if (SelectedUser != null)
            {
                if (SelectedUser.Password != NewPassword || SelectedUser.UserName != NewUsername || SelectedUser.Savings != NewSavings || SelectedUser.IsNotify != IsNotify)
                {
                    User.Savings = NewSavings;
                    User.Password = NewPassword;
                    User.Username = NewUsername;
                    User.UpdateDate = Convert.ToDateTime(Globals.phoneDate);
                    User.isNotify = IsNotify;
                }
            }
            if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewPassword) || NewSavings == 0)
            {
                await _pageService.DisplayAlert("ERROR", "Please complete user details", "OK");
                //ErrorMessage = "Registration Failed : Please complete the username, password and savings"; 
                return;
            }
            if(IsNotify == false)
            {
                if(await _pageService.DisplayAlert("Warning","You are about to save a user without turning on the notification setting, are you sure you want to continue?", "YES", "NO"))
                {
                    goto PostDetails;
                }
                else
                {
                    return;
                }
            }
PostDetails:
            if (User.Id == 0)
            {
                if (mode == "setting")
                {
                    ErrorMessage = "Registration is not allowed in settings mode.";
                    goto ResetDetails;
                }
                else
                {
                    await _sqlUser.AddUser(User);
                    Users.Add(new UserViewModel(User));
                    ErrorMessage = "Registration Successful: " + $"User {User.Username} is added.";
                    MessagingCenter.Send(this, Events.UserAdded, User);
                }

            }
            else
            {
                await _sqlUser.UpdateUser(User);
                var UserInList = Users.Single(c => c.Id == User.Id);

                UserInList.Id = User.Id;
                UserInList.UserName = User.Username;
                UserInList.Password = User.Password;
                UserInList.Savings = User.Savings;
                UserInList.UpdateDate = User.UpdateDate;
                UserInList.IsNotify = User.isNotify;

                if (Globals.currentUser.Id == User.Id)
                {
                    Globals.currentUser.Id = User.Id;
                    Globals.currentUser.Savings = User.Savings;
                    Globals.currentUser.Username = User.Username;
                    Globals.currentUser.isNotify = User.isNotify;
                }
                if (User.isNotify == false)
                {
                    DeactivateNotification();
                }
                ErrorMessage = "Update Successful: " + $"User {User.Username} is updated.";
                MessagingCenter.Send(this, Events.UserUpdated, User);
            }


  ResetDetails:          
            SelectedUser = null;
            User = null;
            NewUsername = null;
            NewPassword = null;
            NewSavings = 0;
            IsNotify = false;
            //await _pageService.PopAsync();
        }
        private async Task DeleteUser(UserViewModel userViewModel,string mode)
        {
            if(mode == "setting")
            {
                await _pageService.DisplayAlert("Warning", "You are not allowed to delete your own user in settings mode.", "OK");
                return;
            }
            else
            {
                if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {userViewModel.UserName}?", "Yes", "No"))
                {
                    Users.Remove(userViewModel);

                    var user = await _sqlUser.GetUser(userViewModel.Id);
                    await _sqlUser.DeleteUser(user);
                }
            }

        }
    }
}
