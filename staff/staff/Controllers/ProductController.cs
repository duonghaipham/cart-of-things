using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace staff.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("Products")]
        public IActionResult Retrieve()
        {
            return View();
        }

        [HttpGet]
        [Route("Products/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Products/create")]
        public IActionResult Create(string a)
        {
            return View();
        }

        [HttpGet]
        [Route("Products/{Id?}/update")]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPut]
        [Route("Products/{Id?}/update")]
        public IActionResult Update(string a)
        {
            return View();
        }

        [HttpDelete("{id}")]
        [Route("Products/{Id?}/delete")]
        public IActionResult Delete(int Id)
        {
            return View();
        }

    }
}
