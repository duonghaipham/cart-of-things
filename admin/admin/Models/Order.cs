using System;
using System.Collections.Generic;

#nullable disable

namespace admin.Models
{
    public partial class Order
    {
        //public Order()
        //{
        //    Wares = new HashSet<Ware>();
        //}

        public int Id { get; set; }
        public string RecipientName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PaymentState { get; set; }
        public int? IdAccount { get; set; }
        public double? Total { get; set; }
        public string PaymentId { get; set; }

    }
}