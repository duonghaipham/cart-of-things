using admin.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return View();
        }

        [HttpPost]
        [Route("Users/create")]
        public IActionResult Create(string a)
        {
            return View();
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
            if(account != null)
                return Json(new { msg = "successed", newState = account.Lock});
            return Json(new { msg = "failed" });
        }   
    }
}
