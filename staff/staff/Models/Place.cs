using System;
using System.Collections.Generic;

#nullable disable

namespace staff.Models
{
    public partial class Place
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int NumberStaff { get; set; }
    }
}
