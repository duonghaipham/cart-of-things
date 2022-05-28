


#nullable disable

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using customer.Helpers;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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
        public int? Lock { get; set; }
        
        public int? VerifiedEmail { get; set; }

        private static ShopContext _context = new ShopContext();

        private static string DEFAULT_AVATAR =
            "https://res.cloudinary.com/ddt3wxtce/image/upload/v1653036596/user_w6bc5z.png";
        
        public static string SignIn(string email, string password)
        {
            var account = (from a in _context.Accounts
                          where a.Email == email
                          select a).SingleOrDefault();

            if (account == null)
            {
                return JsonConvert.SerializeObject(new { Error = "Account not found" });
            }

            if (!Hash.GetInstance().VerifyHash(password, account.Password))
            {
                return JsonConvert.SerializeObject(new { Error = "Wrong password" });
            }

            return JsonConvert.SerializeObject(new { Account = account });
        }
        
        public static Account ViewInformation(int id)
        {
            var account = (from a in _context.Accounts
                           where a.Id == id
                           select a).SingleOrDefault();
            
            return account;
        }

        public static Account ViewInformation(string email)
        {
            var account = (from a in _context.Accounts
                           where a.Email == email
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
        
        public static Account IsExistedEmail(string email)
        {
            var account = (from a in _context.Accounts
                           where a.Email == email
                           select a).SingleOrDefault();
            
            return account;
        }

        public static Account SignUp(string name, string email, string password, string phone)
        {
            string hashedPassword = Hash.GetInstance().GetHash(password);

            var account = new Account()
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                Avatar = DEFAULT_AVATAR,
                Role = "customer",
                IdentityCard = phone
            };
            
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        public static bool CheckPassword(int id, string password)
        {
            var account = (from a in _context.Accounts
                where a.Id == id
                select a).SingleOrDefault();
            
            return Hash.GetInstance().VerifyHash(password, account.Password);
        }

        public static Account ChangePassword(int id, string newPassword)
        {
            string hashedNewPassword = Hash.GetInstance().GetHash(newPassword);
            
            var account = (from a in _context.Accounts
                where a.Id == id
                select a).SingleOrDefault();

            account.Password = hashedNewPassword;
            _context.SaveChanges();

            return account;
        }

        public static string GenerateToken(string email)
        {
            string token = Guid.NewGuid().ToString();

            var state = new State()
            {
                MinuteLimit = 15,
                Token = token
            };
            
            _context.States.Add(state);
            _context.SaveChanges();
            
            var account = (from a in _context.Accounts
                where a.Email == email
                select a).SingleOrDefault();
            
            account.IdState = state.Id;
            _context.SaveChanges();

            return token;
        }

        public static bool ValidateToken(string token, string email)
        {
            var account = (from a in _context.Accounts
                join s in _context.States on a.IdState equals s.Id 
                where a.Email == email && s.Token == token
                select a).SingleOrDefault();

            if (account != null)
                return true;
            
            return false;
        }

        public static bool VerifyEmail(string email)
        {
            var account = (from a in _context.Accounts
                where a.Email == email
                select a).SingleOrDefault();
            
            account.VerifiedEmail = 1;
            _context.SaveChanges();
            
            if (account.VerifiedEmail == 1)
                return true;

            return false;
        }

        public static bool ResetPassword(string email, string password)
        {
            try
            {
                var account = (from a in _context.Accounts
                    where a.Email == email
                    select a).SingleOrDefault();
                
                string hashedPassword = Hash.GetInstance().GetHash(password);
                account.Password = hashedPassword;
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
