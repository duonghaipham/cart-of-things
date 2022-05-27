using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult SignUp(string name, string email, string password, string confirmPassword, string phone)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View("SignUp");
            }
            else
            {
                if (Account.IsExistedEmail(email) != null)
                {
                    ViewBag.Error = "Email is existed";
                    return View("SignUp");
                }
                else
                {
                    var account = Account.SignUp(name, email, password, phone);
                    return Redirect("/SignIn");
                }
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
        public IActionResult ForgetPassword(string email, string securityQuestion, string securityAnswer, string newPassword)
        {
            return View();
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
    }
}
