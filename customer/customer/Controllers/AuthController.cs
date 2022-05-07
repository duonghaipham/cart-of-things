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
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(string email, string password)
        {
            return View();
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
            return View();
        }

        [HttpGet]
        [Route("Users/{UserId?}")]
        public IActionResult ViewInformation()
        {
            return View();
        }

        [HttpGet]
        [Route("Users/{UserId?}/Update")]
        public IActionResult UpdateInformation()
        {
            return View();
        }

        [HttpPost]
        [Route("Users/{UserId?}/Update")]
        public IActionResult UpdateInformation(string atLeastOneInfor)
        {
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
            return View();
        }
    }
}
