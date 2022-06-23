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
        public static string search(string search, string sort)
        {
            var staff = (from a in context.Accounts
                         where EF.Functions.Like(a.Name, $"%{search}%") && a.Role == "customer"
                         select a);
            if (sort == "name")
                staff = staff.OrderBy(a => a.Name);

            if (sort == "email")
                staff = staff.OrderBy(a => a.Email);

            return JsonConvert.SerializeObject(staff);
        }

        public static List<Account> getList()
        {
            return context.Accounts.Where(a => a.Role == "customer").ToList();
        }

        public static string getList(int page)
        {
            var listCustomers = (from a in context.Accounts
                              where a.Role == "customer"
                              select a);

            var listCustomersPaged = listCustomers
                .Skip((page - 1) * Pagination.ITEM_PER_PAGE)
                .Take(Pagination.ITEM_PER_PAGE)
                .ToList();

            return JsonConvert.SerializeObject(listCustomersPaged);

        }

        public static int totalCustomer()
        {
            var list = context.Accounts.Where(a => a.Role == "customer");

            return list.Count();
        }

        public static Account updateLock(int Id)
        {
            var account = context.Accounts.Find(Id);
            account.Lock = account.Lock == 1 ? 0 : 1;
            context.SaveChanges();
            return account;
        }

        public static Account get(int Id)
        {
            var account = context.Accounts.Find(Id);
            return account;
        }

        public static bool update(string avatar, string name, string email, string identityCard, int Id)
        {
            var account = context.Accounts.Find(Id);
            if (avatar == "" && account.Name == name && account.Email == email && account.IdentityCard == identityCard)
                    return true;
            if (avatar != "")
                account.Avatar = avatar;
            account.Name = name;
            account.Email = email;
            account.IdentityCard = identityCard;
            var rs = context.SaveChanges();
            if (rs == 0)
                return false;
            return true;
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
                                   .Where(a => a.Email == email && a.Role == "staff").SingleOrDefault();

            if (account == null)
                return JsonConvert.SerializeObject(new { Error = "Account not found" });

            if (!Hash.GetInstance().VerifyHash(password, account.Password))
                return JsonConvert.SerializeObject(new { Error = "Wrong password" });

            return JsonConvert.SerializeObject(new { Account = account });
        }
    }
}
