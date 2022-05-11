using System;
using System.Collections.Generic;

#nullable disable

namespace customer.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? IdProduct { get; set; }
        public int? IdAccount { get; set; }
    }
}
