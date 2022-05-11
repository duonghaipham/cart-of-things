using System;
using System.Collections.Generic;

#nullable disable

namespace customer.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int? IdProduct { get; set; }
        public int? IdAccount { get; set; }
    }
}
