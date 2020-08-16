using MEESEES.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MEESEES
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzAzNzMzQDMxMzgyZTMyMmUzMGNXSytxRlNmcStTYm9XUGg4dDVNK0pMRzlxbTZYaUlwakszN09WNCtMaUU9");
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            MessagingCenter.Send<string>("1", "myService");
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send<string>("1", "myService");
        }

        protected override void OnResume()
        {
            MessagingCenter.Send<string>("1", "myService");
        }
    }
}
