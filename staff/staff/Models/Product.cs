using System;
using System.Collections.Generic;
using System.Linq;
using staff.Helpers;
using System.Web;
using System.Security.Cryptography;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace staff.Models
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

        private static ShopContext context = new ShopContext();

        public static string RetrieveProducts() //int page, string category, string search, string sort
        {
            var products = from p in context.Products
                           join c in context.Categories on p.IdCategory equals c.Id
                           where p.IsArchived == 0
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

            return JsonConvert.SerializeObject(products);

            //if (category != null)
            //{
            //    products = products.Where(p => p.CategoryName == category);
            //}

            //if (search != null)
            //{
            //    products = products.Where(p => EF.Functions.Like(p.Name, $"%{search}%"));
            //}

            //if (sort == "name")
            //{
            //    products = products.OrderBy(p => p.Name);
            //}
            //if (sort == "price")
            //{
            //    products = products.OrderBy(p => p.Price);
            //}

            //int numObjects = products.Count();

            //var productsPaged = products
            //    .Skip((page - 1) * Pagination.ITEM_PER_PAGE)
            //    .Take(Pagination.ITEM_PER_PAGE)
            //    .ToList();

            //Pagination pagination = new Pagination(numObjects, page);

            //string jsonProductsAndPagination = JsonConvert.SerializeObject(new
            //{
            //    products = productsPaged,
            //    pagination
            //});

            //return jsonProductsAndPagination;
        }

        public static string getProduct(int id)
        {
            var product = (from p in context.Products
                           join c in context.Categories on p.IdCategory equals c.Id
                           where p.Id == id && p.IsArchived == 0
                           select new
                           {
                               p.Id,
                               p.Name,
                               p.Introduction,
                               p.Description,
                               p.Price,
                               p.Avatar,
                               p.IsArchived,
                               p.IdCategory,
                               CategoryName = c.Name
                           }).SingleOrDefault();

            var jsonProduct = JsonConvert.SerializeObject(product);

            return jsonProduct;
        }

        public static bool deleteProduct(int Id)
        {
            var product = context.Products.Find(Id);
            product.IsArchived = 1;
            var rs = context.SaveChanges();
            if (rs == 0)
                return false;
            return true;
        }

        public static bool updateProduct(int Id, string avatar, int IdCategory, string introduction, double price, string description, string name)
        {
            
            var product = context.Products.Find(Id);
            if (avatar == "" && IdCategory == product.IdCategory && introduction == product.Introduction && 
                price == product.Price && description == product.Description && name == product.Name)
                return true;

            if (avatar != "")
                product.Avatar = avatar;
            product.IdCategory = IdCategory;
            product.Introduction = introduction;
            product.Price = price;
            product.Description = description;
            product.Name = name;
            var rs = context.SaveChanges();
            if (rs == 0)
                return false;
            return true;
        }
        public static int? RetrieveCategoryId(int productId)
        {
            var categoryId = (from p in context.Products
                              where p.Id == productId
                              select p.IdCategory).SingleOrDefault();

            return categoryId;
        }

        public static Product createProduct(string avatar, int IdCategory, string introduction, double price, string description, string name)
        {
            var product = new Product()
            {
                Name = name,
                Introduction = introduction,
                Description = description,
                Price = price,
                Avatar = avatar,
                IsArchived = 0,
                IdCategory = IdCategory
            };
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public static string RetrieveProductsRelatedToCategory(int idCategory, int idProduct)
        {
            var products = (from p in context.Products
                            join c in context.Categories on p.IdCategory equals c.Id
                            where p.IdCategory == idCategory && p.IsArchived == 0 && p.Id != idProduct
                            orderby p.Id descending
                            select new
                            {
                                p.Id,
                                p.Name,
                                p.Price,
                                p.Avatar,
                                CategoryName = c.Name
                            }).ToList();

            var jsonProducts = JsonConvert.SerializeObject(products);

            return jsonProducts;
        }
    }
}
