using customer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("About")]
        public IActionResult ViewAbout()
        {
            return View();
        }

        [HttpGet]
        [Route("Contact")]
        public IActionResult ViewContact()
        {
            return View();
        }

        [HttpGet]
        [Route("CheckOut")]
        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpGet]
        [Route("Cart")]
        public IActionResult ViewCart()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
