﻿using System;
using System.Collections.Generic;
using System.Linq;
using admin.Helpers;
using System.Web;
using System.Security.Cryptography;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public int Lock { get; set; }
        public int? VerifiedEmail { get; set; }

        private static ShopContext context = new ShopContext();
        public static string getList(int page)
        {
            var listStaffs = (from a in context.Accounts
                            join p in context.Places on a.IdPlace equals p.Id
                            where a.Role == "staff"
                            select new
                            {
                                Id = a.Id,
                                Name = a.Name,
                                IdentityCard = a.IdentityCard,
                                Email = a.Email,
                                Avatar = a.Avatar,
                                Lock = a.Lock, 
                                NamePlace = p.Name
                            });

            var listStaffsPaged = listStaffs
                .Skip((page - 1) * Pagination.ITEM_PER_PAGE) 
                .Take(Pagination.ITEM_PER_PAGE) 
                .ToList();

            return JsonConvert.SerializeObject(listStaffsPaged);

        }

        public static string search(string search, string sort)
        {
            var staff = (from a in context.Accounts
                              join p in context.Places on a.IdPlace equals p.Id
                              where (EF.Functions.Like(a.Name, $"%{search}%") || EF.Functions.Like(a.IdentityCard, $"%{search}%")) && a.Role == "staff"
                              select new
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  IdentityCard = a.IdentityCard,
                                  Email = a.Email,
                                  Avatar = a.Avatar,
                                  Lock = a.Lock,
                                  NamePlace = p.Name
                              });
            if (sort == "name")
                staff = staff.OrderBy(a => a.Name);

            if (sort == "email")
                staff = staff.OrderBy(a => a.Email);

            return JsonConvert.SerializeObject(staff);
        }

        public static int totalStaff()
        {
            var listStaffs = context.Accounts.Where(a => a.Role == "staff");

            return listStaffs.Count();
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
            if (avatar == "" && account.Name == name && account.Email == email && account.IdentityCard == identityCard)
                if (account.Role == "admin")
                    return true;
                else if (idPlace == account.IdPlace && account.Role == "staff")
                    return true;

            if (avatar != "")
                account.Avatar = avatar;
            account.Name = name;
            account.Email = email;
            account.IdentityCard = identityCard;
            if (idPlace != 0)
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

        public static string changePass(int id, string newPass, string curPass)
        {
            var account = context.Accounts.Find(id);

            if (Hash.GetInstance().VerifyHash(newPass, account.Password))
                return JsonConvert.SerializeObject(new { msg = "New password same old password" });

            if (!Hash.GetInstance().VerifyHash(curPass, account.Password))
                return JsonConvert.SerializeObject(new { msg = "Wrong password" });

            string hashedPassword = Hash.GetInstance().GetHash(newPass);
            account.Password = hashedPassword;
            var rs = context.SaveChanges();
            if (rs == 0)
                return JsonConvert.SerializeObject(new { msg = "failed update password" });

            return JsonConvert.SerializeObject(new { msg = "successed" });
        }

        public static string signin(string email, string password)
        {
            Account account = context.Accounts
                                   .Where(a => a.Email == email && a.Role == "admin").SingleOrDefault();

            if (account == null)
                return JsonConvert.SerializeObject(new { Error = "Account not found" });

            if (!Hash.GetInstance().VerifyHash(password, account.Password))
                return JsonConvert.SerializeObject(new { Error = "Wrong password" });

            return JsonConvert.SerializeObject(new { Account = account });
        }
    }
}
