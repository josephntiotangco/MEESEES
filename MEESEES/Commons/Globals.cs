using MEESEES.ViewModels;
using MEESEES.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;


namespace MEESEES.Commons
{
    public class Globals
    {
        public static string phoneDate = DateTime.Now.ToString("MM/dd/yyyy");
        public static string myAdID;
        public static User currentUser = new User();
        public static bool isDataLoaded;
        //public static ObservableCollection<DataInputViewModel> ExpenseList = new ObservableCollection<DataInputViewModel>();
        //public static ObservableCollection<DataInputViewModel> FundList = new ObservableCollection<DataInputViewModel>();
        public static bool isForNotification;
        public static string serviceCounter;
        public static decimal gvTotalBalance;
        public static decimal gvTotalExpense;
        public static decimal gvTotalFund;

        //public static List<Entry> pieEntries = new List<Entry>();
        //public static List<Entry> barEntries = new List<Entry>();
    }
}
