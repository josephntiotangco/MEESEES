using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.Models
{
    public class DataInput
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; } //source if fund
        public decimal Amount { get; set; }
        public string Type { get; set; } //F= fund E=expense
        public DateTime EntryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UserId { get; set; }

        public DataInput() { }
        public DataInput(string description, decimal amount, string type,DateTime entrydate, DateTime update, int userId)
        {
            this.Description = description;
            this.Amount = amount;
            this.Type = type;
            this.EntryDate = entrydate;
            this.UpdateDate = update;
            this.UserId = userId;
        }
    }
}
