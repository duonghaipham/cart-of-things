using admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using admin.Helpers;
//System.Diagnostics.Debug.WriteLine(account.email);
namespace admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Redirect("/SignIn");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        public class UserOfSignIn
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn([FromBody] UserOfSignIn account)
        {
            var jsonAccount = Account.signin(account.email, account.password);
            var user = JsonConvert.DeserializeObject(jsonAccount) as JObject;

            if (user["Error"] != null)

                return Json(new
                {
                    msg = "failed",
                    error = user["Error"].ToString()
                });

            int lockVal = int.TryParse(user["Account"]["Lock"].ToString(), out lockVal) ? lockVal : 0;
            if (lockVal == 1)
                return Json(new
                {
                    msg = "failed",
                    error = "Your account is locked. Please contact your administrator."
                });

            HttpContext.Session.SetString("profile", JsonConvert.SerializeObject(user["Account"]));

            return Json(new
            {
                msg = "successed"
            });
        }

        [HttpGet]
        [Route("Profile")]
        public IActionResult Profile()
        {
            var profile = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("profile"));
            ViewData["Profile"] = profile;
            return View(profile); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
