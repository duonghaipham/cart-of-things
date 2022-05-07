using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customer.Controllers
{
    public class CommentController : Controller
    {
        [HttpGet]
        [Route("Comments")]
        public IActionResult RetrieveComments()
        {
            return View();
        }

        [HttpPost]
        [Route("Comments")]
        public IActionResult CreateComments()
        {
            return View();
        }
    }
}
