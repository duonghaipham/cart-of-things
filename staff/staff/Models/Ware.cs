using System;
using System.Collections.Generic;

#nullable disable

namespace staff.Models
{
    public partial class Ware
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int? IdProduct { get; set; }
        public int? IdOrder { get; set; }
    }
}
