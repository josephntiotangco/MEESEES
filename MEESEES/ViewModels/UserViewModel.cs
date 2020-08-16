using MEESEES.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public UserViewModel() { }
        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.Username;
            Password = user.Password;
            Savings = user.Savings;
            UpdateDate = user.UpdateDate;
            IsNotify = user.isNotify;
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetValue(ref _userName, value);
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                SetValue(ref _password, value);
                OnPropertyChanged(nameof(Password));
            }
        }
        private decimal _savings;
        public decimal Savings
        {
            get { return _savings; }
            set
            {
                SetValue(ref _savings, value);
                OnPropertyChanged(nameof(Savings));
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
        private bool _isNotify;
        public bool IsNotify
        {
            get { return _isNotify; }
            set
            {
                SetValue(ref _isNotify, value);
                OnPropertyChanged(nameof(IsNotify));
            }
        }
    }
}
