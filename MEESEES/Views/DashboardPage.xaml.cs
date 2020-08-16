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
    public partial class DashboardPage : ContentPage
    {
        SQLiteDataInput dataInterface = new SQLiteDataInput(DependencyService.Get<ISQLiteDb>(), "normal");
        PageService pageService = new PageService();
        public DashboardPage()
        {
            Globals.myAdID = "ca-app-pub-6838059012127071/5073888855";
            DashboardModel = new DashboardViewModel(dataInterface, pageService);
            DashboardModel.SelectedPayday = null;
            Globals.isDataLoaded = false;
            InitializeComponent();

            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int paydayCutoff = (day > 15) ? 2 : 1;
            int index = DashboardModel.PaydayIndex(paydayCutoff, month);
            if (DashboardModel.SelectedPayday == null) DashboardModel.SelectedPayday = DashboardModel.Paydays.Single(p => p.Index == 0);


        }
        public DashboardViewModel DashboardModel
        {
            get { return BindingContext as DashboardViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnAppearing()
        {
            if (DashboardModel.SelectedPayday != null) return;
            Globals.isDataLoaded = false;
            DashboardModel.LoadDataInputCommand.Execute(null);

            base.OnAppearing();
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

        private void OnLogOutClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void OnResetClicked(object sender, EventArgs e)
        {
            if (await pageService.DisplayAlert("WARNING", "Are you sure you want to reset transactions?", "YES", "NO"))
            {
                dataInterface = new SQLiteDataInput(DependencyService.Get<ISQLiteDb>(), "reset");
                DashboardModel.ListViewData.Clear();
                DashboardModel.DataInputs.Clear();
                Globals.currentUser = null;
                (Application.Current).MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                return;
            }
        }

        private void OnFundStackTapped(object sender, EventArgs e)
        {
            var mpage = new MenuPage();
            (Application.Current).MainPage = mpage;
            mpage.OnFundListClicked(sender, e);
        }
        private void OnExpenseStackTapped(object sender, EventArgs e)
        {
            var mpage = new MenuPage();
            (Application.Current).MainPage = mpage;
            mpage.OnExpenseListClicked(sender, e);
        }
        private void OnSavingsStackTapped(object sender, EventArgs e)
        {
            var mpage = new MenuPage();
            (Application.Current).MainPage = mpage;
            mpage.OnSettingsClicked(sender, e);
        }
        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (DashboardModel.SelectedPayday == null) return;
            int currentIndex = DashboardModel.SelectedPayday.Index;
            if (currentIndex == 0) return;
            int newIndex = currentIndex - 1;
            if (newIndex == 0) return;
            DashboardModel.SelectedPayday = DashboardModel.Paydays.Single(p => p.Index == newIndex);
            Globals.isDataLoaded = false;
            DashboardModel.LoadPayDayDataCommand.Execute(null);


        }
        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            if (DashboardModel.SelectedPayday == null) return;
            int currentIndex = DashboardModel.SelectedPayday.Index;
            if (currentIndex == 24) return;
            int newIndex = currentIndex + 1;
            if (newIndex == 25) return;
            DashboardModel.SelectedPayday = DashboardModel.Paydays.Single(p => p.Index == newIndex);
            Globals.isDataLoaded = false;
            DashboardModel.LoadPayDayDataCommand.Execute(null);

        }

        private void OnSelectionChange(object sender, EventArgs e)
        {
            if (DashboardModel.SelectedPayday == null) return;
            Globals.isDataLoaded = false;
            if (DashboardModel.SelectedPayday.Index == 0)
            {
                DashboardModel.LoadDataInputCommand.Execute(null);
            }
            else
            {
                DashboardModel.LoadPayDayDataCommand.Execute(null);
            }

        }
        

    }
}