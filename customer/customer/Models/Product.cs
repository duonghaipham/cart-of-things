using System;
using System.Collections.Generic;

#nullable disable

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
    }
}
