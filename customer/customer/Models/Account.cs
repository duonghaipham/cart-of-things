


#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace customer.Models
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

        private static ShopContext _context = new ShopContext();

        public static Account SignIn(string email, string password)
        {
            var account = (from a in _context.Accounts
                          where a.Email == email && a.Password == password
                          select a).SingleOrDefault();
            
            return account;
        }
        
        public static Account ViewInformation(int id)
        {
            var account = (from a in _context.Accounts
                           where a.Id == id
                           select a).SingleOrDefault();
            
            return account;
        }

        public static Account UpdateInformation(int id, string avatar, string name)
        {
            var account = (from a in _context.Accounts
                where a.Id == id
                select a).SingleOrDefault();

            if (avatar != null)
                account.Avatar = avatar;
            account.Name = name;

            _context.SaveChanges();

            return account;
        }
    }
}
