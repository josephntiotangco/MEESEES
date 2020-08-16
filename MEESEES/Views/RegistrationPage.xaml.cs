using MEESEES.Commons;
using MEESEES.Data;
using MEESEES.Models;
using MEESEES.ViewModels;
using MEESEES.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MEESEES.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public static string _mode;
        public RegistrationPage(UserViewModel viewModel, string mode = "")
        {
            _mode = mode;
            Globals.myAdID = "ca-app-pub-6838059012127071/4882317165";

            var sqliteUser = new SQLiteUser(DependencyService.Get<ISQLiteDb>());
            var pageService = new PageService();
            Title = mode == "setting" ? "User Setting" : "New User";
            RegModel = new RegistrationViewModel(viewModel ?? new UserViewModel(), sqliteUser, pageService, mode);
            InitializeComponent();
        }
        public RegistrationViewModel RegModel
        {
            get { return BindingContext as RegistrationViewModel; }
            set { BindingContext = value; }
        }
        protected override void OnAppearing()
        {
            RegModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        private void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (RegModel.SelectedUser != null)
            {
                RegModel.NewUsername = RegModel.SelectedUser.UserName;
                RegModel.NewPassword = RegModel.SelectedUser.Password;
                RegModel.NewSavings = RegModel.SelectedUser.Savings;
                RegModel.IsNotify = RegModel.SelectedUser.IsNotify;

                RegModel.User = new User
                {
                    Id = RegModel.SelectedUser.Id,
                    Username = RegModel.SelectedUser.UserName,
                    Password = RegModel.SelectedUser.Password,
                    Savings = RegModel.SelectedUser.Savings,
                    isNotify = RegModel.SelectedUser.IsNotify,
                    UpdateDate = RegModel.SelectedUser.UpdateDate
                };
            }
            Title = "Edit User";
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            if (_mode == "setting")
            {
                var menuPage = new MenuPage();
                (Application.Current).MainPage = menuPage;
                menuPage.Detail = new NavigationPage(new DashboardPage());
                menuPage.IsPresented = false;
            }
            return true;
        }
        private void OnSavingsCompleted(object sender, EventArgs e)
        {
            RegModel.SaveCommand.Execute(null);
        }
    }
}