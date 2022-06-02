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
        public int? Lock { get; set; }

        private static ShopContext context = new ShopContext();
        //public static string getList()
        //{

        //    var listStaff = (from a in context.Accounts
        //                     join p in context.Places on a.IdPlace equals p.Id
        //                     where a.Role == "staff"
        //                     select new
        //                     {
        //                         Id = a.Id,
        //                         Name = a.Name,
        //                         IdentityCard = a.IdentityCard,
        //                         Email = a.Email,
        //                         Avatar = a.Avatar,
        //                         Lock = a.Lock,
        //                         NamePlace = p.Name
        //                     });
        //    string jsonOrders = JsonConvert.SerializeObject(listStaff);

        //    return jsonOrders;
        //}

        public static List<Account> getList()
        {
            return context.Accounts.Where(a => a.Role == "customer").ToList();
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
                                   .Where(a => a.Email == email).SingleOrDefault();

            if (account == null)
                return JsonConvert.SerializeObject(new { Error = "Account not found" });

            if (!Hash.GetInstance().VerifyHash(password, account.Password))
                return JsonConvert.SerializeObject(new { Error = "Wrong password" });

            return JsonConvert.SerializeObject(new { Account = account });
        }
    }
}
