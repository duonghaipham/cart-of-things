using System;
using System.Collections.Generic;

#nullable disable

namespace customer.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public string RecipientName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? IdPayment { get; set; }
        public int? IdAccount { get; set; }
    }
}
