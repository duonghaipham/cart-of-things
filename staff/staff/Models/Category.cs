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
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        private static ShopContext context = new ShopContext();
        public static List<Category> getList()
        {
            return context.Categories.ToList();
        }
    }
}
