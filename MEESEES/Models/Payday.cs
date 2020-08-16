using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.Models
{
    public class Payday
    {
        public string Description { get; set; }
        public int Month { get; set; }
        public int CutOff { get; set; }
        public int Index { get; set; }
    }
}
