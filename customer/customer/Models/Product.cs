#nullable disable

using System.Linq;
using Newtonsoft.Json;

namespace customer.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Avatar { get; set; }
        public int? IsArchived { get; set; }
        public int? IdCategory { get; set; }

        private static ShopContext _context = new ShopContext();

        public static string RetrieveProducts()
        {
            var products = from p in _context.Products
                join c in _context.Categories on p.IdCategory equals c.Id
                select new
                {
                    p.Id,
                    p.Name,
                    p.Introduction,
                    p.Description,
                    p.Price,
                    p.Avatar,
                    p.IsArchived,
                    CategoryName = c.Name
                };
            
            var jsonProducts = JsonConvert.SerializeObject(products);

            return jsonProducts;
        }
        
        public static string RetrieveProduct(int id)
        {
            var product = (from p in _context.Products
                join c in _context.Categories on p.IdCategory equals c.Id
                where p.Id == id
                select new
                {
                    p.Id,
                    p.Name,
                    p.Introduction,
                    p.Description,
                    p.Price,
                    p.Avatar,
                    p.IsArchived,
                    CategoryName = c.Name
                }).SingleOrDefault();
            
            var jsonProduct = JsonConvert.SerializeObject(product);

            return jsonProduct;
        }
    }
}
