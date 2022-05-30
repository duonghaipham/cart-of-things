﻿using System;
using System.Collections.Generic;
using System.Linq;
using admin.Helpers;
using System.Web;

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

        public static Account getStaff(int Id)
        {
            var account = context.Accounts.Find(Id);
            return account;
        }

        public static bool updateStaff(string avatar, string name, string email, string identityCard, int idPlace, int Id)
        {
            var account = context.Accounts.Find(Id);
            account.Avatar = avatar;
            account.Name = name;
            account.Email = email;
            account.IdentityCard = identityCard;
            account.IdPlace = idPlace;
            var rs = context.SaveChanges();
            if (rs == 0)
                return false;
            return true;
        }
        public static int createStaff(string avatar, string name, string email, string identityCard, string password, int id)
        {
            string hashedPassword = Hash.GetInstance().GetHash(password);
            var staff = new Account()
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                Avatar = avatar,
                Role = "staff",
                IdentityCard = identityCard,
                IdPlace = id,
                Lock = 0 //Ko khóa
            };
            context.Accounts.Add(staff);
            var rs = context.SaveChanges();

            return rs;
        }
    }
}
