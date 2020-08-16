using MEESEES.Commons;
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
    public partial class AboutPage : ContentPage
    {
        PageService pageService = new PageService();
        string about = "";
        public AboutPage()
        {
            Globals.myAdID = "ca-app-pub-6838059012127071/5429112077";
            about += "Monitor - Expenses - Easy - Simple - Effective - Efficient - System" + Environment.NewLine;
            about += "aka M.E.E.S.E.E.S" + Environment.NewLine;
            about += "Is an app designed to help individuals to monitor their budget and daily expenses." + Environment.NewLine;
            about += "This app aims to make people more concern on their current financial status.The app has the following features: " + Environment.NewLine;
            about += " 1.Dashboard - shows the summary of your total funds, total expenses, remaining balance, should be savings and charts to for your analysis." + Environment.NewLine;
            about += " 2.Add Fund - this is the module to where you will add your fund sources stating the specific date you have received them. " + Environment.NewLine;
            about += " 3.Fund List - this will show all of your fund entries." + Environment.NewLine;
            about += " 4.Add Expense - this is the module to where you will add your expenses per day or on a specific date." + Environment.NewLine;
            about += " 5.Expense List - this will show all of your expense entries." + Environment.NewLine;
            about += " 6.My Settings - this is where you can edit your savings amount, or password." + Environment.NewLine;
            about += "" + Environment.NewLine;
            about += "Disclaimer:" + Environment.NewLine;
            about += "This app does not collect any personal data from the user." + Environment.NewLine;
            about += "Your data will remain on your mobile device storage and database unless otherwise uninstalled." + Environment.NewLine;
            about += "" + Environment.NewLine;
            about += "MEESEES Privacy Statement: " + Environment.NewLine;
            about += "Link: https://jntdevz.blogspot.com/2020/08/privacy-policy-of-meesees.html" + Environment.NewLine;

            LoginModel = new LoginViewModel(pageService, null, about);
            InitializeComponent();
        }
        public LoginViewModel LoginModel
        {
            get { return BindingContext as LoginViewModel; }
            set { BindingContext = value; }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}