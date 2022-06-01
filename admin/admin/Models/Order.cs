using System;
using System.Collections.Generic;
using System.Linq;
using admin.Helpers;
using System.Web;
using System.Security.Cryptography;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace admin.Models
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