using MEESEES.Commons;
using MEESEES.Data;
using MEESEES.Models;
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
    public partial class InputListPage : ContentPage
    {
        SQLiteDataInput dataInterface = new SQLiteDataInput(DependencyService.Get<ISQLiteDb>(), "normal");
        PageService pageService = new PageService();
        private static string _type;
        public InputListPage(string type)
        {
            Globals.myAdID = "ca-app-pub-6838059012127071/6137179779";
            _type = type;
            InputModel = new DashboardViewModel(dataInterface, pageService, type);

            switch (type)
            {
                case "E":
                    Title = "Expenses";
                    break;
                case "F":
                    Title = "Fund Sources";
                    break;
            }
            InitializeComponent();

            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int paydayCutoff = (day > 15) ? 2 : 1;
            int index = InputModel.PaydayIndex(paydayCutoff, month);
            if (InputModel.SelectedPayday == null) InputModel.SelectedPayday = InputModel.Paydays.Single(p => p.Index == index);
        }
        
        public DashboardViewModel InputModel
        {
            get { return BindingContext as DashboardViewModel; }
            set { BindingContext = value; }
        }
        protected override void OnAppearing()
        {
            Globals.isDataLoaded = false;
            InputModel.LoadPayDayDataCommand.Execute(null);
            base.OnAppearing();
        }
        private void OnDataSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InputModel.SelectedDataInput != null)
            {
                InputModel.SelectedDate = InputModel.SelectedDataInput.EntryDate;
                InputModel.Description = InputModel.SelectedDataInput.Description;
                InputModel.Amount = InputModel.SelectedDataInput.Amount;

                InputModel.DataInput = new DataInput
                {
                    Id = InputModel.SelectedDataInput.Id,
                    Description = InputModel.SelectedDataInput.Description,
                    Amount = InputModel.SelectedDataInput.Amount,
                    Type = InputModel.SelectedDataInput.Type,
                    EntryDate = InputModel.SelectedDataInput.EntryDate,
                    UpdateDate = InputModel.SelectedDataInput.UpdateDate,
                    UserId = InputModel.SelectedDataInput.UserId
                };
            }


            var mPage = new MenuPage();
            (Application.Current).MainPage = mPage;
            mPage.Detail = new NavigationPage(new DataInputPage(InputModel.SelectedDataInput, _type, "update"));
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("MEESEES", $"Return to Dashboard?", "YES", "NO"))
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

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (InputModel.SelectedPayday == null) return;
            int currentIndex = InputModel.SelectedPayday.Index;
            if (currentIndex == 0) return;
            int newIndex = currentIndex - 1;
            if (newIndex == 0) return;
            InputModel.SelectedPayday = InputModel.Paydays.Single(p => p.Index == newIndex);
            Globals.isDataLoaded = false;
            InputModel.LoadPayDayDataCommand.Execute(null);

        }
        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            if (InputModel.SelectedPayday == null) return;
            int currentIndex = InputModel.SelectedPayday.Index;
            if (currentIndex == 24) return;
            int newIndex = currentIndex + 1;
            if (newIndex == 25) return;
            InputModel.SelectedPayday = InputModel.Paydays.Single(p => p.Index == newIndex);
            Globals.isDataLoaded = false;
            InputModel.LoadPayDayDataCommand.Execute(null);
        }

        private void OnSelectionChange(object sender, EventArgs e)
        {
            if (InputModel.SelectedPayday == null) return;
            Globals.isDataLoaded = false;
            if (InputModel.SelectedPayday.Index == 0)
            {
                InputModel.LoadDataInputCommand.Execute(null);
            }
            else
            {
                InputModel.LoadPayDayDataCommand.Execute(null);
            }
        }
    
    }
}