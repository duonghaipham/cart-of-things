using System;
using System.Collections.Generic;

#nullable disable

namespace customer.Models
{
    public partial class Ware
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int? IdProduct { get; set; }
        public int? IdOrder { get; set; }
        
        private static ShopContext _context = new ShopContext();
        
        public static void AddWareToOrder(int amount, int idProduct, int idOrder)
        {
            var ware = new Ware()
            {
                Amount = amount,
                IdProduct = idProduct,
                IdOrder = idOrder
            };
            
            _context.Wares.Add(ware);
            _context.SaveChanges();
        }
    }
}
