using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("Shop")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Products/{ProductId?}")]
        public IActionResult View(int id)
        {

            // truy vấn lấy ra Product có id
            ViewData["Id"] = id;

            return View();
        }
    }
}
