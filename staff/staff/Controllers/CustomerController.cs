using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using staff.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using staff.Models;

namespace staff.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet]
        [Route("Customers")]
        public IActionResult Retrieve()
        {
            ViewData["listCustomers"] = Account.getList(); ;
            ViewBag.Active = "Customers";
            return View();
        }

        [HttpPut]
        [Route("Customers/{Id?}/lock")]
        public IActionResult Lock(int Id)
        {
            ViewBag.Active = "Customers";
            var account = Account.updateLock(Id);
            if (account != null)
                return Json(new { msg = "successed", newState = account.Lock });
            return Json(new { msg = "failed" });
        }

        [HttpPost]
        [Route("Customers/uploadFile")]
        public IActionResult UploadFile(IFormFile file)
        {
            string result = "";
            if (file?.Length > 0)
                result = ImageManager.GetInstance().Upload(file).SecureUrl.AbsoluteUri.ToString();

            return Json(new
            {
                pathFile = result
            });
        }

        [HttpGet]
        [Route("Customers/{Id?}/update")]
        public IActionResult Update(int Id)
        {
            ViewBag.Active = "Customers";
            Account account = Account.get(Id);
            Account profile = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("profile"));
            if (profile != null && profile.Id == account.Id)
            {
                ViewBag.Active = "No";
                Place placeWork = Place.getPlace(account.IdPlace ?? 0);
                ViewData["workPlace"] = placeWork;
            }

            ViewData["staff"] = account;
            return View();
        }

        public class Profile
        {
            public string avatar { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string identityCard { get; set; }

        }

        [HttpPut]
        [Route("Customers/{Id?}/update")]
        public IActionResult Update([FromBody] Profile account, int Id)
        {
            Account staff = Account.get(Id);
            Account profile = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("profile"));
            var result = Account.update(account.avatar, account.name, account.email, account.identityCard, Id);
            if (!result)
                return Json(new { msg = "failed" });
            if (profile.Id == staff.Id)
            {
                staff = Account.get(Id);
                if (staff != null)
                    HttpContext.Session.SetString("profile", JsonConvert.SerializeObject(staff));
                return Json(new { msg = "successed", path = "/Profile" });
            }

            return Json(new{ msg = "successed", path = "/Customers" });
        }
    }
}
