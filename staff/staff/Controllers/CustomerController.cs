using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace staff.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet]
        [Route("Customers")]
        public IActionResult Retrieve()
        {
            return View();
        }

        [HttpPut]
        [Route("Customers/{Id?}/lock")]
        public IActionResult Lock(string a)
        {
            return View();
        }
    }
}
