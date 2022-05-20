using Microsoft.AspNetCore.Mvc;
using System;
using customer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
            var account = Account.SignIn(email, password);
            
            if (account != null)
            {
                HttpContext.Session.SetInt32("id", account.Id);
                HttpContext.Session.SetString("name", account.Name);
                HttpContext.Session.SetString("ava", account.Avatar);
            
                return Redirect("/Home");
            }

            return Redirect("/SignIn");
        }

        [HttpGet]
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(string name, string phone, string email, string password, string securityQuestion, string securityAnswer)
        {
            return View();
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

        [HttpGet]
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            return null;
        }

        [HttpGet]
        [Route("Users/{UserId?}")]
        public IActionResult ViewInformation()
        {
            // var account = 
        }

        [HttpGet]
        [Route("Users/{UserId?}/Update")]
        public IActionResult UpdateInformation()
        {
            return View();
        }

        [HttpPost]
        [Route("Users/{UserId?}/Update")]
        public IActionResult UpdateInformation(IFormFile avatar, string name, string phone)
        {
            var uploadResult = ImageManager.getInstance().Upload(avatar).SecureUrl;
            Console.WriteLine(uploadResult);
            
            return View();
        }

        [HttpGet]
        [Route("Users/{UserId?}/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPut]
        [Route("Users/{id?}/ChangePassword")]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            return View();
        }

        [HttpPost]
        [Route("Users/{id?}/CheckPassword")]
        public IActionResult CheckPassword()
        {
            return null;
        }
    }
}
