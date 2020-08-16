using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.Models
{
    public class ChartData
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }

        public ChartData() { }
        public ChartData(string type, decimal amount)
        {
            this.Type = type;
            this.Amount = amount;
        }
    }
}
