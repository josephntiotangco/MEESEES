using MEESEES.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.ViewModels
{
    public class ChartDataViewModel : BaseViewModel
    {
        public ChartDataViewModel(ChartData data)
        {
            Type = data.Type;
            Amount = data.Amount;
        }
        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                SetValue(ref _type, value);
                OnPropertyChanged(nameof(Type));
            }
        }
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                SetValue(ref _amount, value);
                OnPropertyChanged(nameof(Amount));
            }
        }
    }
}
