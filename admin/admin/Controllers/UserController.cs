using admin.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using admin.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System;
//System.Diagnostics.Debug.WriteLine();
namespace admin.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [Route("Users")]
        public IActionResult Retrieve()
        {
            List<Account> list = Account.getList();
            ViewData["listStaff"] = list;
            return View();
        }
        //public IEnumerable<Place> Retrieve()
        //{
        //    ShopContext context = new ShopContext();

        //    return context.Places.ToList();
        //}

        [HttpGet]
        [Route("Users/create")]
        public IActionResult Create()
        {
            List<Place> list = Place.getList();
            ViewData["listPlace"] = list;
            return View();
        }

        public class Staff
        {
            public string avatar { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string identityCard { get; set; }
            public string password { get; set; }
            public int idPlace { get; set; }
        }

        [HttpPost]
        [Route("Users/create")]
        public IActionResult Create([FromBody] Staff account)
        {
            var resultSttaff = Account.createStaff(account.avatar, account.name, account.email, account.identityCard, account.password, account.idPlace);
            if (resultSttaff == 1)
            {
                var updateNumberStaff = Place.updateNumberStaff(account.idPlace);
                if (updateNumberStaff)
                    return Json(new
                    { msg = "successed" });
                return Json(new
                { msg = "failed" });
            }
            return Json(new
            { msg = "failed" });
        }


        [HttpPost]
        [Route("Users/uploadFile")]
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
        [Route("Users/{Id?}/update")]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPut]
        [Route("Users/{Id?}/update")]
        public IActionResult Update(string a)
        {
            return View();
        }

        [HttpPut]
        [Route("Users/{Id?}/lock")]
        public IActionResult Lock(int Id)
        {
            var account = Account.updateLock(Id);
            if (account != null)
                return Json(new { msg = "successed", newState = account.Lock });
            return Json(new { msg = "failed" });
        }
    }
}
