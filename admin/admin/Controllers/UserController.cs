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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//System.Diagnostics.Debug.WriteLine();
namespace admin.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [Route("Users")]
        public IActionResult Retrieve()
        {
            var jsonList = Account.getList();
            ViewData["listStaff"] = jsonList;
            ViewBag.Active = "Users";
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
            ViewBag.Active = "Users";
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
                var updateNumberStaff = Place.updateNumberStaff(account.idPlace, 1);
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
        public IActionResult Update(int Id)
        {
            ViewBag.Active = "Users";
            Account staff = Account.getStaff(Id);
            Account profile = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("profile"));
            if (profile.Id == staff.Id)
                ViewBag.Active = "No";
            Place placeWork = Place.getPlace(staff.IdPlace ?? 0);
            List<Place> listPlace = Place.getList(staff.IdPlace ?? 0);
            ViewData["listPlace"] = listPlace;
            ViewData["staff"] = staff;
            ViewData["workPlace"] = placeWork;
            return View();
        }

        [HttpPut]
        [Route("Users/{Id?}/update")]
        public IActionResult Update([FromBody] Staff account, int Id)
        {
            Account staff = Account.getStaff(Id);
            Account profile = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("profile"));
            var result = Account.updateStaff(account.avatar, account.name, account.email, account.identityCard, account.idPlace, Id);
            if (profile.Id == staff.Id)
            {
                if (!result)
                    return Json(new { msg = "failed" });
                staff = Account.getStaff(Id);
                if(staff != null)
                    HttpContext.Session.SetString("profile", JsonConvert.SerializeObject(staff));
                return Json(new { msg = "successed", path = "/Profile" });
            }

            var updateNumberStaffCur = Place.updateNumberStaff(staff.IdPlace ?? 0, -1);
            var updateNumberStaffNew = Place.updateNumberStaff(account.idPlace, 1);
            if (!result || !updateNumberStaffCur || !updateNumberStaffNew)
                return Json(new{ msg = "failed" });
            return Json(new{ msg = "successed", path = "/Users" });
        }

        [HttpPut]
        [Route("Users/{Id?}/lock")]
        public IActionResult Lock(int Id)
        {
            ViewBag.Active = "Users";
            var account = Account.updateLock(Id);
            if (account != null)
                return Json(new { msg = "successed", newState = account.Lock });
            return Json(new { msg = "failed" });
        }
    }
}
