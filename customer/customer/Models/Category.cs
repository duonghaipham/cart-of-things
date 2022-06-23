

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace customer.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private static ShopContext _context = new ShopContext();
        
        public static string GetAll()
        {
            var categories = _context.Categories.ToList();
            string jsonProducts = JsonConvert.SerializeObject(categories);

            return jsonProducts;
        }
    }
}
