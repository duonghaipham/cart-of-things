using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using customer.Helpers;
using customer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace customer.Controllers
{
    public class AuthController : Controller
    {
        private IConfiguration _config;
        
        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(string email, string password)
        {
            var jsonAccount = Account.SignIn(email, password);
            var user = JsonConvert.DeserializeObject(jsonAccount) as JObject;
            
            if (user["Error"] != null)
            {
                ViewBag.Error = user["Error"].ToString();
                return View();
            }
            
            int lockVal = int.TryParse(user["Account"]["Lock"].ToString(), out lockVal) ? lockVal : 0;
            if (lockVal == 1)
            {
                ViewBag.Error = "Your account is locked. Please contact your administrator.";
                return View();
            }

            int verifiedEmail = int.TryParse(user["Account"]["VerifiedEmail"].ToString(), out verifiedEmail) ? verifiedEmail : 0;
            if (verifiedEmail != 1)
            {
                ViewBag.Error = "Your email is not verified. Please verify your email.";
                return View();
            }

            HttpContext.Session.SetInt32("id", (int) user["Account"]["Id"]);
            HttpContext.Session.SetString("name", user["Account"]["Name"].ToString());
            HttpContext.Session.SetString("avatar", user["Account"]["Avatar"].ToString());
            
            return Redirect("/Home");
        }
        
        [HttpGet]
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(string name, string email, string password, string confirmPassword, string phone)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View("SignUp");
            }

            if (Account.IsExistedEmail(email) != null)
            {
                ViewBag.Error = "Email is existed";
                return View("SignUp");
            }

            var account = Account.SignUp(name, email, password, phone);
            
            string token = Account.GenerateToken(email);

            string verifiedUrl = $"https://localhost:5001/VerifyEmail?token={token}&email={email}";
            string verifiedBody = $"Please confirm your account by clicking this link: <a href='{verifiedUrl}'>link</a>";

            try
            {
                await MailUtil.GetInstance().SendGmail(email, "Confirm your account", verifiedBody);
                ViewBag.Message = "Please check your email to confirm your account";

                return View("SignIn");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.Error = e.Message;
                return View("SignUp");
            }
        }

        [HttpGet]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            string token = Account.GenerateToken(email);
            
            string verifiedUrl = $"https://localhost:5001/ResetPassword?token={token}&email={email}";
            string verifiedBody = $"<h1>Reset Password</h1><p>Click <a href='{verifiedUrl}'>here</a> to reset your password</p>";

            try
            {
                await MailUtil.GetInstance().SendGmail(email, "Forgot Password", verifiedBody);
                ViewBag.Message = "Please check your email to reset your password";

                return View("ForgetPassword");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            
            return Redirect("/");
        }

        [HttpGet]
        [Route("Users/{UserId?}")]
        public IActionResult ViewInformation()
        {
            int id = HttpContext.Session.GetInt32("id").GetValueOrDefault();
            var account = Account.ViewInformation(id);
            
            ViewData["Account"] = account;
            
            return View();
        }

        [HttpGet]
        [Route("Users/{UserId?}/Update")]
        public IActionResult UpdateInformation()
        {
            int id = HttpContext.Session.GetInt32("id").GetValueOrDefault();
            var account = Account.ViewInformation(id);
            
            ViewData["Account"] = account;
            
            return View();
        }

        [HttpPost]
        [Route("Users/{UserId?}/Update")]
        public IActionResult UpdateInformation(IFormFile avatar, string name, string phone)
        {
            string newAvatar = null;
            
            if (avatar != null)
                newAvatar = ImageManager.GetInstance().Upload(avatar).SecureUrl.AbsoluteUri;
            
            int id = HttpContext.Session.GetInt32("id").GetValueOrDefault();
            var account = Account.UpdateInformation(id, newAvatar, name);
            
            HttpContext.Session.SetInt32("id", account.Id);
            HttpContext.Session.SetString("name", account.Name);
            HttpContext.Session.SetString("avatar", account.Avatar);

            return Redirect($"/Users/{id}/Update");
        }

        [HttpGet]
        [Route("Users/{UserId?}/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        
        public class ChangingPassword
        {
            public string NewPassword { get; set; }
        }

        [HttpPut]
        [Route("Users/ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangingPassword password)
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            
            Console.WriteLine();
            
            var account = Account.ChangePassword(id, password.NewPassword);
            
            if (account != null)
            {
                return Json(new { msg = "success" });
            }

            return Json(new { msg = "failed" });
        }
        
        public class CheckingPassword
        {
            public string CurrentPassword { get; set; }
        }

        [HttpPost]
        [Route("Users/CheckPassword")]
        public IActionResult CheckPassword([FromBody] CheckingPassword password)
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            
            if (Account.CheckPassword(id, password.CurrentPassword))
                return Json(new {msg = "success"});
            
            return Json(new {msg= "failed"});
        }
        
        [HttpGet]
        [Route("ResetPassword")]
        public IActionResult ResetPassword()
        {
            string token = HttpContext.Request.Query["token"].ToString();
            string email = HttpContext.Request.Query["email"].ToString();

            if (Account.ValidateToken(token, email))
            {
                ViewData["email"] = email;
                return View();
            }
            
            ViewBag.Message = "Invalid token";
            return View("ForgetPassword");
        }
        
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string password)
        {
            string email = HttpContext.Request.Query["email"].ToString();
            
            if (Account.ResetPassword(email, password))
            {
                ViewBag.Message = "Password has been reset";
                return View("ResetPassword");
            }

            return View();
        }

        [HttpGet]
        [Route("VerifyEmail")]
        public IActionResult VerifyEmail()
        {
            string token = HttpContext.Request.Query["token"].ToString();
            string email = HttpContext.Request.Query["email"].ToString();

            if (Account.ValidateToken(token, email))
            {
                if (Account.VerifyEmail(email))
                {
                    ViewBag.Message = "Your email has been verified";
                
                    var account = Account.ViewInformation(email);
                
                    HttpContext.Session.SetInt32("id", account.Id);
                    HttpContext.Session.SetString("name", account.Name);
                    HttpContext.Session.SetString("avatar", account.Avatar);
                
                    return View("VerifiedEmail");
                }

                ViewBag.Message = "There was an error verifying your email, please try again";
                return View("SignUp");
            }

            ViewBag.Message = "Invalid token";
            return View("SignUp");
        }
    }
}
