using MEESEES.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.ViewModels
{
    public class DataInputViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public DataInputViewModel() { }

        public DataInputViewModel(DataInput data)
        {
            Id = data.Id;
            Description = data.Description;
            Amount = data.Amount;
            Type = data.Type;
            EntryDate = data.EntryDate;
            UpdateDate = data.UpdateDate;
            UserId = data.UserId;
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                SetValue(ref _description, value);
                OnPropertyChanged(nameof(Description));
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
        private DateTime _entryDate;
        public DateTime EntryDate
        {
            get { return _entryDate; }
            set
            {
                SetValue(ref _entryDate, value);
                OnPropertyChanged(nameof(EntryDate));
            }
        }
        private DateTime _updateDate;
        public DateTime UpdateDate
        {
            get { return _updateDate; }
            set
            {
                SetValue(ref _updateDate, value);
                OnPropertyChanged(nameof(UpdateDate));
            }
        }
        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set
            {
                SetValue(ref _userId, value);
                OnPropertyChanged(nameof(UserId));
            }
        }
    }
}
