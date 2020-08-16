using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MEESEES.Models
{
    public class User
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Username { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        public decimal Savings { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool isNotify { get; set; }
        public User()
        {

        }
        public User(string userName, string passWord, DateTime update, decimal savings = 0, bool isNotify = false)
        {
            this.Username = userName;
            this.Password = passWord;
            this.UpdateDate = update;
            this.Savings = savings;
        }
    }
}
