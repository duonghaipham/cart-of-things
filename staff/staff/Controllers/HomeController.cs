using staff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace staff.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Redirect("/signin");
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
            HttpContext.Session.SetString("userName", user["Account"]["Name"].ToString());
            HttpContext.Session.SetString("avatar", user["Account"]["Avatar"].ToString());
            return Json(new
            {
                msg = "successed"
            });
        }

        [Route("SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
