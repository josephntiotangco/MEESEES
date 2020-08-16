using MEESEES.Commons;
using MEESEES.Data;
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
    public partial class LoginPage : ContentPage
    {
        SQLiteUser sqliteUser = new SQLiteUser(DependencyService.Get<ISQLiteDb>());
        PageService pageService = new PageService();
        UserViewModel userView = new UserViewModel();
        public LoginPage()
        {
            Globals.myAdID = "ca-app-pub-6838059012127071/1517787225";
            Globals.currentUser = null;
            LoginModel = new LoginViewModel(pageService, sqliteUser);
            InitializeComponent();

        }
        public LoginViewModel LoginModel
        {
            get { return BindingContext as LoginViewModel; }
            set { BindingContext = value; }
        }
        private void OnEntryComplete(object sender, EventArgs e)
        {
            LoginModel.LoginCommand.Execute(null);
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage(userView));
        }


    }
}