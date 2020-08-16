using MEESEES.Commons;
using MEESEES.Data;
using MEESEES.ViewModels;
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
    public partial class DataInputPage : ContentPage
    {
        SQLiteDataInput dataInterface = new SQLiteDataInput(DependencyService.Get<ISQLiteDb>(), "normal");
        PageService pageService = new PageService();
        public DataInputPage(DataInputViewModel data, string type, string mode)
        {
            Globals.myAdID = "ca-app-pub-6838059012127071/4824098100"; //create ad control
            InputModel = new DashboardViewModel(dataInterface, pageService, type, data, mode);
            if (data.Id == 0)
            {
                switch (type)
                {
                    case "E":
                        Title = "New Expense";
                        break;
                    case "F":
                        Title = "New Fund";
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case "E":
                        Title = "Update Expense";
                        break;
                    case "F":
                        Title = "Update Fund";
                        break;
                }
            }

            InitializeComponent();
            DateTime oldDate = Convert.ToDateTime("1/1/2020");
            dpEntryDate.SetValue(DatePicker.DateProperty, DateTime.Now);
            dpEntryDate.SetValue(DatePicker.MinimumDateProperty, oldDate);
            dpEntryDate.SetValue(DatePicker.MaximumDateProperty, DateTime.Now);
        }
        public DashboardViewModel InputModel
        {
            get { return BindingContext as DashboardViewModel; }
            set { BindingContext = value; }
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
                else
                {
                    var menuPage = new MenuPage();
                    (Application.Current).MainPage = menuPage;
                    menuPage.Detail = new NavigationPage(new DashboardPage());
                    menuPage.IsPresented = false;
                }
            });
            return true;
        }
        private void OnLogOutClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }
    }
}