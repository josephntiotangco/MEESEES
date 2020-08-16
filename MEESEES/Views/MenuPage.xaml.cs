using MEESEES.Commons;
using MEESEES.ViewModels;
using MEESEES.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MEESEES.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class MenuPage : MasterDetailPage
    {
        UserViewModel userModel = new UserViewModel();
        DataInputViewModel dataInput = new DataInputViewModel();
        public MenuPage()
        {
            InitializeComponent();

            Detail = new NavigationPage(new DashboardPage());

            IsPresented = false;

        }
        public void OnDashboardClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new DashboardPage());
            IsPresented = false;
        }

        public void OnNewFundClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new DataInputPage(dataInput, "F", "new"));
            IsPresented = false;
        }

        public void OnNewExpenseClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new DataInputPage(dataInput, "E", "new"));
            IsPresented = false;
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Warning", $"Are you sure you want to logout user: {Globals.currentUser.Username} ?", "YES", "NO"))
                {
                    (Application.Current).MainPage = new NavigationPage(new LoginPage());
                }
            });
            return true;
        }

        public void OnSettingsClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new RegistrationPage(userModel, "setting"));
            IsPresented = false;
        }

        public void OnFundListClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new InputListPage("F"));
            IsPresented = false;
        }

        public void OnExpenseListClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new InputListPage("E"));
            IsPresented = false;
        }
    }
}