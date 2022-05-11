using System;
using System.Collections.Generic;

#nullable disable

namespace customer.Models
{
    public partial class State
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int MinuteLimit { get; set; }
    }
}
