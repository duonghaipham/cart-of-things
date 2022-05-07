using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string name, string phone, string email, string password, string securityQuestion, string securityAnswer)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgetAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetAccount(string email, string securityQuestion, string securityAnswer, string newPassword)
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RetrieveInformation()
        {
            return View();
        }


    }
}
