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
        public int? Lock { get; set; }

        private static ShopContext context = new ShopContext();
        public static List<Account> getList(){
      
            var listStaff = context.Accounts
                                   .Where(s => s.Role == "staff").ToList();

            return listStaff;
        }

        public static Account updateLock(int Id)
        {
            var account = context.Accounts.Find(Id);
            account.Lock = account.Lock == 1 ? 0 : 1;
            context.SaveChanges();
            return account;            
        }
    }
}
