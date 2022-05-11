using System;
using System.Collections.Generic;

#nullable disable

namespace staff.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public double Cost { get; set; }
        public string Details { get; set; }
        public DateTime Time { get; set; }
        public int State { get; set; }
    }
}
