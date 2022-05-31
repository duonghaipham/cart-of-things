using System;
using System.Linq;

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
        public string PaymentState{ get; set; }
        public int? IdAccount { get; set; }
        public float? Total { get; set; }
        public string PaymentId { get; set; }

        private static ShopContext _context = new ShopContext();
        
        public static Order Create(string name, string country, string street, string number, string city, string note, string phone, int idAccount, float total)
        {
            var order = new Order
            {
                RecipientName = name,
                Address = country + " " + street + " " + number + " " + city,
                Phone = phone,
                Note = note,
                CreatedAt = DateTime.Now,
                IdAccount = idAccount,
                Total = total
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }
        
        public static void UpdatePaymentId(int id, string paymentId)
        {
            var order = _context.Orders.Find(id);
            
            order.PaymentId = paymentId;
            _context.SaveChanges();
        }
        
        public static void UpdatePaymentState(string paymentId, string paymentState)
        {
            var order = (from o in _context.Orders
                where o.PaymentId == paymentId
                select o).SingleOrDefault();
            
            order.PaymentState = paymentState;
            _context.SaveChanges();
        }
    }
}
