using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace admin.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdentityCard { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public int? IdState { get; set; }
        public int? IdPlace { get; set; }
        public List<Account> getList(){
            ShopContext context = new ShopContext();
            var listStaff = context.Accounts
                                   .Where(s => s.Role == "staff").ToList();

            return listStaff;
        }

    }
}
