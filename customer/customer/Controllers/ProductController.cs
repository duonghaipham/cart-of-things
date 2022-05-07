using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult View(int id)
        {

            // truy vấn lấy ra Product có id
            ViewData["Id"] = id;

            return View();
        }
    }
}
