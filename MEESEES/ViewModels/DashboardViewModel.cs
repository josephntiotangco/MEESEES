using MEESEES.Commons;
using MEESEES.Data;
using MEESEES.Models;
using MEESEES.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MEESEES.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private IDataInputInterface _dataInputInterface;
        private IPageService _pageService;
        private static string _type;
        private static string _mode;
        public DataInput DataInput { get; set; }
        public ObservableCollection<DataInputViewModel> DataInputs { get; private set; } = new ObservableCollection<DataInputViewModel>();
        public ObservableCollection<DataInputViewModel> ListViewData { get; private set; } = new ObservableCollection<DataInputViewModel>();
        public ObservableCollection<Payday> Paydays { get; private set; } = new ObservableCollection<Payday>();
        public ObservableCollection<ChartDataViewModel> PieChart { get; private set; } = new ObservableCollection<ChartDataViewModel>();
        //public ObservableCollection<ChartDataViewModel> BarChart { get; private set; } = new ObservableCollection<ChartDataViewModel>();
        public ChartData ChartData { get; set; }
        public List<string> CutOffs { get; private set; } = new List<string>();
        
        #region Properties
        private decimal _totalFund;
        public decimal TotalFund
        {
            get { return _totalFund; }
            set { SetValue(ref _totalFund, value); OnPropertyChanged(nameof(TotalFund)); }
        }
        private decimal _totalExpense;
        public decimal TotalExpense
        {
            get { return _totalExpense; }
            set { SetValue(ref _totalExpense, value); OnPropertyChanged(nameof(TotalExpense)); }
        }
        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get { return _totalBalance; }
            set { SetValue(ref _totalBalance, value); OnPropertyChanged(nameof(TotalBalance)); }
        }
        private decimal _savings;
        public decimal Savings
        {
            get { return _savings; }
            set { SetValue(ref _savings, value); OnPropertyChanged(nameof(Savings)); }
        }
        private DataInputViewModel _selectedDataInput;
        public DataInputViewModel SelectedDataInput
        {
            get { return _selectedDataInput; }
            set { SetValue(ref _selectedDataInput, value); OnPropertyChanged(nameof(SelectedDataInput)); }
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetValue(ref _isRefreshing, value); OnPropertyChanged(nameof(IsRefreshing)); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetValue(ref _description, value); OnPropertyChanged(nameof(Description)); }
        }
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set { SetValue(ref _amount, value); OnPropertyChanged(nameof(Amount)); }
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetValue(ref _selectedDate, value); OnPropertyChanged(nameof(SelectedDate)); }
        }
        private string _labelTitle;
        public string LabelTitle
        {
            get { return _labelTitle; }
            set { SetValue(ref _labelTitle, value); OnPropertyChanged(nameof(LabelTitle)); }
        }
        private string _placeholder;
        public string Placeholder
        {
            get { return _placeholder; }
            set { SetValue(ref _placeholder, value); OnPropertyChanged(nameof(Placeholder)); }
        }
        private bool _isLoggingOut;
        public bool IsLoggingOut
        {
            get { return _isLoggingOut; }
            set { SetValue(ref _isLoggingOut, value); OnPropertyChanged(nameof(IsLoggingOut)); }
        }
        private Payday _selectedPayday;
        public Payday SelectedPayday
        {
            get { return _selectedPayday; }
            set { SetValue(ref _selectedPayday, value); OnPropertyChanged(nameof(SelectedPayday)); }
        }
        #endregion

        #region Commands
        public ICommand DeleteDataInputCommand { get; private set; }
        public ICommand SaveDataInputCommand { get; private set; }
        public ICommand LoadDataInputCommand { get; private set; }
        public ICommand RefreshDataCommand { get; private set; }
        public ICommand SelectInputCommand { get; private set; }
        public ICommand LoadPayDayDataCommand { get; private set; }

        #endregion
        public DashboardViewModel(IDataInputInterface dataInputInterface, IPageService pageService, string type ="", DataInputViewModel data = null, string mode ="")
        {
            _dataInputInterface = dataInputInterface;
            _pageService = pageService;
            _mode = mode;
            
            Savings = Globals.currentUser.Savings;

            if (type != "") InitializeControls(type);

            DeleteDataInputCommand = new Command<DataInputViewModel>(async c => await DeleteInput(c));
            LoadDataInputCommand = new Command(async () => await LoadData());
            SaveDataInputCommand = new Command(async () => await SaveData(type));
            RefreshDataCommand = new Command(async () => await RefreshData());
            LoadPayDayDataCommand = new Command(async () => await LoadDataByEntryDate(SelectedPayday));
            if(mode != "new")
            {
                if (data != null)
                {
                    Amount = data.Amount;
                    Description = data.Description;
                    SelectedDate = data.EntryDate;

                    DataInput = new DataInput
                    {
                        Id = data.Id,
                        Description = data.Description,
                        EntryDate = data.EntryDate,
                        UpdateDate = data.UpdateDate,
                        Amount = data.Amount,
                        Type = data.Type,
                        UserId = data.UserId
                    };
                }
                else
                {
                    SelectedDate = DateTime.Now;
                }
            }

            InitializePaydays();
        }
        private void InitializeControls(string type)
        {
            _type = type;
            switch (type)
            {
                case "E":
                    Placeholder = "DESCRIPTION";
                    break;
                case "F":
                    Placeholder = "SOURCE";
                    break;
            }
        }
        public async Task RefreshData()
        {
            IsRefreshing = true;
            Globals.isDataLoaded = false;
            if(SelectedPayday == null)
            {
                await LoadData();
            }
            else
            {
                if (SelectedPayday.Index == 0)
                {
                    await LoadData();
                }
                else
                {
                    await LoadDataByEntryDate(SelectedPayday);
                }
            }
            IsRefreshing = false;
        }
        private async Task LoadData()
        {
            if (Globals.isDataLoaded == true) return;
            Globals.isDataLoaded = true;
            DataInputs.Clear();
            var dataInputs = await _dataInputInterface.GetDataInputByUserId(Globals.currentUser.Id);
            foreach (DataInput data in dataInputs)
                DataInputs.Add(new DataInputViewModel(data));

            ListViewData.Clear(); //2020/08/15
            if (DataInputs.Count != 0)
            {
                ListViewData.Clear();
                foreach (DataInputViewModel dmodel in DataInputs)
                {
                    if (dmodel.Type == _type)
                    {
                        ListViewData.Add(dmodel);
                    }
                }
            }
            
            TotalExpense = DataInputs.Where(x => x.Type == "E").Sum(x => x.Amount);
            TotalFund = DataInputs.Where(x => x.Type == "F").Sum(x => x.Amount);
            TotalBalance = TotalFund - TotalExpense;
            Savings = Globals.currentUser.Savings;

            Globals.gvTotalExpense = DataInputs.Where(x => x.Type == "E").Sum(x => x.Amount);
            Globals.gvTotalFund = DataInputs.Where(x => x.Type == "F").Sum(x => x.Amount);
            Globals.gvTotalBalance = Globals.gvTotalFund - Globals.gvTotalExpense;

            PieChart.Clear();
            ChartData = new ChartData
            {
                Amount = Globals.gvTotalBalance,
                Type = "Remaining Balance"
            };
            PieChart.Add(new ChartDataViewModel(ChartData));
            ChartData = new ChartData
            {
                Amount = Globals.gvTotalExpense,
                Type = "Expenses"
            };
            PieChart.Add(new ChartDataViewModel(ChartData));
            if(Globals.currentUser.isNotify == true)
            {
                if (Savings > Globals.gvTotalBalance)
                {
                    if (await _pageService.DisplayAlert("MEESEES", "Your remaining balance is less than your Savings, have you already set aside the savings from this amount?", "YES", "NO"))
                    {
                        return;
                    }
                    else
                    {
                        await _pageService.DisplayAlert("MEESEES", "Please set aside the savings amount now!", "OK");
                        return;
                    }
                }
            }

        }
        private async Task LoadDataByEntryDate(Payday payday)
        {
            if (payday == null) return;
            if (Globals.isDataLoaded == true) return;
            Globals.isDataLoaded = true;
            DataInputs.Clear();
            var dataInputs = await _dataInputInterface.GetDataInputByUserId(Globals.currentUser.Id);
            foreach (DataInput data in dataInputs)
            {
                int cutOff = data.EntryDate.Day > 15 ? 2 : 1;
                if(payday.Month == data.EntryDate.Month && payday.CutOff == cutOff)
                {
                    DataInputs.Add(new DataInputViewModel(data));
                }
            }

            ListViewData.Clear();
            if (DataInputs.Count != 0)
            {
                ListViewData.Clear();
                foreach (DataInputViewModel dmodel in DataInputs)
                {
                    if (dmodel.Type == _type)
                    {
                        ListViewData.Add(dmodel);
                    }
                }
            }
            TotalExpense = DataInputs.Where(x => x.Type == "E").Sum(x => x.Amount);
            TotalFund = DataInputs.Where(x => x.Type == "F").Sum(x => x.Amount);
            TotalBalance = TotalFund - TotalExpense;
            Savings = Globals.currentUser.Savings;

            Globals.gvTotalExpense = DataInputs.Where(x => x.Type == "E").Sum(x => x.Amount);
            Globals.gvTotalFund = DataInputs.Where(x => x.Type == "F").Sum(x => x.Amount);
            Globals.gvTotalBalance = Globals.gvTotalFund - Globals.gvTotalExpense;

            PieChart.Clear();
            ChartData = new ChartData
            {
                Amount = Globals.gvTotalExpense,
                Type = "Remaining Balance"
            };
            PieChart.Add(new ChartDataViewModel(ChartData));
            ChartData = new ChartData
            {
                Amount = Globals.gvTotalFund,
                Type = "Expenses"
            };
            PieChart.Add(new ChartDataViewModel(ChartData));

            if (Globals.currentUser.isNotify == true)
            {
                if (Savings > Globals.gvTotalBalance)
                {
                    if (await _pageService.DisplayAlert("MEESEES", "Your remaining balance is less than your Savings, have you already set aside the savings from this amount?", "YES", "NO"))
                    {
                        return;
                    }
                    else
                    {
                        await _pageService.DisplayAlert("MEESEES", "Please set aside the savings amount now!", "OK");
                        return;
                    }
                }
            }


        }

        private async Task SaveData(string type)
        {
            if (DataInput == null)
            {
                DataInput = new DataInput
                {
                    Amount = Amount,
                    Description = Description,
                    UserId = Globals.currentUser.Id,
                    Type = type,
                    EntryDate = SelectedDate,
                    UpdateDate = Convert.ToDateTime(Globals.phoneDate)
                };
            }
            if (DataInput != null)
            {
                if (DataInput.Description != Description || DataInput.Amount != Amount || DataInput.EntryDate != SelectedDate)
                {
                    DataInput.Amount = Amount;
                    DataInput.EntryDate = SelectedDate;
                    DataInput.Description = Description;
                    DataInput.UpdateDate = Convert.ToDateTime(Globals.phoneDate);
                }
            }
            if (string.IsNullOrWhiteSpace(Description) && Amount == 0)
            {
                await _pageService.DisplayAlert("ERROR", "Input Failed : Please complete the description/source and amount", "OK");
                return;
            }
            if (DataInput.Id == 0)
            {
                await _dataInputInterface.AddDataInput(DataInput);
                await _pageService.DisplayAlert("MEESEES", "Input Successfully Saved: " + $"Description: {DataInput.Description}, Amount = {DataInput.Amount}.", "OK");

                ListViewData.Add(new DataInputViewModel(DataInput));
            }
            else
            {
                await _dataInputInterface.UpdateData(DataInput);
                await _pageService.DisplayAlert("MEESEES", "Input Successfully Updated: " + $"Description: {DataInput.Description}, Amount = {DataInput.Amount}.", "OK");

                var DataInList = ListViewData.Single(c => c.Id == DataInput.Id);

                DataInList.Id = DataInput.Id;
                DataInList.Description = DataInput.Description;
                DataInList.Amount = DataInput.Amount;
                DataInList.Type = DataInput.Type;
                DataInList.UserId = DataInput.UserId;
                DataInList.EntryDate = DataInput.EntryDate;
                DataInList.UpdateDate = DataInput.UpdateDate;

            }
            SelectedDataInput = null;
            DataInput = null;
            Amount = 0;
            Description = null;
        }
        private async Task DeleteInput(DataInputViewModel viewModel)
        {
            string type = viewModel.Type == "F" ? "Fund :" : "Expense :";
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {type} {viewModel.Description} : {viewModel.Amount}?", "YES", "NO"))
            {
                var data = await _dataInputInterface.GetDataInput(viewModel.Id);
                await _dataInputInterface.DeleteData(data);
                ListViewData.Remove(viewModel);
            }
            SelectedDataInput = null;
            DataInput = null;
            Amount = 0;
            Description = null;
        }
        

        #region Initialize
        public int PaydayIndex(int index, int month)
        {
            switch (index)
            {
                case 1:
                    switch (month)
                    {
                        case 1:
                            return 1;
                        case 2:
                            return 3;
                        case 3:
                            return 5;
                        case 4:
                            return 7;
                        case 5:
                            return 9;
                        case 6:
                            return 11;
                        case 7:
                            return 13;
                        case 8:
                            return 15;
                        case 9:
                            return 17;
                        case 10:
                            return 19;
                        case 11:
                            return 21;
                        case 12:
                            return 23;
                    }
                    break;
                case 2:
                    switch (month)
                    {
                        case 1:
                            return 2;
                        case 2:
                            return 4;
                        case 3:
                            return 6;
                        case 4:
                            return 8;
                        case 5:
                            return 10;
                        case 6:
                            return 12;
                        case 7:
                            return 14;
                        case 8:
                            return 16;
                        case 9:
                            return 18;
                        case 10:
                            return 20;
                        case 11:
                            return 22;
                        case 12:
                            return 24;
                    }
                    break;
            }
            return 0;
        }
        private void InitializePaydays()
        {
            Paydays.Clear();
            for (int i = 0; i < 25; i++)
            {
                string caption = "";
                int index = 0;
                int month = 0;
                int cutoff = 0;
                switch (i)
                {
                    case 0:
                        caption = "ALL";
                        month = 0;
                        index = i;
                        cutoff = 0;
                        break;
                    case 1:
                        caption = "JAN 15";
                        month = 1;
                        index = i;
                        cutoff = 1;
                        break;
                    case 2:
                        caption = "JAN 31";
                        month = 1;
                        index = i;
                        cutoff = 2;
                        break;
                    case 3:
                        caption = "FEB 15";
                        month = 2;
                        index = i;
                        cutoff = 1;
                        break;
                    case 4:
                        caption = "FEB 28";
                        month = 2;
                        index = i;
                        cutoff = 2;
                        break;
                    case 5:
                        caption = "MAR 15";
                        month = 3;
                        index = i;
                        cutoff = 1;
                        break;
                    case 6:
                        caption = "MAR 31";
                        month = 3;
                        index = i;
                        cutoff = 2;
                        break;
                    case 7:
                        caption = "APR 15";
                        month = 4;
                        index = i;
                        cutoff = 1;
                        break;
                    case 8:
                        caption = "APR 30";
                        month = 4;
                        index = i;
                        cutoff = 2;
                        break;
                    case 9:
                        caption = "MAY 15";
                        month = 5;
                        index = i;
                        cutoff = 1;
                        break;
                    case 10:
                        caption = "MAY 31";
                        month = 5;
                        index = i;
                        cutoff = 2;
                        break;
                    case 11:
                        caption = "JUN 15";
                        month = 6;
                        index = i;
                        cutoff = 1;
                        break;
                    case 12:
                        caption = "JUN 30";
                        month = 6;
                        index = i;
                        cutoff = 2;
                        break;
                    case 13:
                        caption = "JUL 15";
                        month = 7;
                        index = i;
                        cutoff = 1;
                        break;
                    case 14:
                        caption = "JUL 31";
                        month = 7;
                        index = i;
                        cutoff = 2;
                        break;
                    case 15:
                        caption = "AUG 15";
                        month = 8;
                        index = i;
                        cutoff = 1;
                        break;
                    case 16:
                        caption = "AUG 31";
                        month = 8;
                        index = i;
                        cutoff = 2;
                        break;
                    case 17:
                        caption = "SEP 15";
                        month = 9;
                        index = i;
                        cutoff = 1;
                        break;
                    case 18:
                        caption = "SEP 30";
                        month = 9;
                        index = i;
                        cutoff = 2;
                        break;
                    case 19:
                        caption = "OCT 15";
                        month = 10;
                        index = i;
                        cutoff = 1;
                        break;
                    case 20:
                        caption = "OCT 31";
                        month = 10;
                        index = i;
                        cutoff = 2;
                        break;
                    case 21:
                        caption = "NOV 15";
                        month = 11;
                        index = i;
                        cutoff = 1;
                        break;
                    case 22:
                        caption = "NOV 30";
                        month = 11;
                        index = i;
                        cutoff = 2;
                        break;
                    case 23:
                        caption = "DEC 15";
                        month = 12;
                        index = i;
                        cutoff = 1;
                        break;
                    case 24:
                        caption = "DEC 31";
                        month = 12;
                        index = i;
                        cutoff = 2;
                        break;
                }
                Paydays.Add(new Payday
                {
                    Description = caption,
                    Month = month,
                    Index = index,
                    CutOff = cutoff
                }) ;
            }
        }
        #endregion
    }
}
