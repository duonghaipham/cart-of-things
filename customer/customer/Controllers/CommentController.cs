using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using customer.Models;
using Microsoft.AspNetCore.Http;

namespace customer.Controllers
{
    public class CommentController : Controller
    {
        [HttpGet]
        [Route("Comments")]
        public IActionResult RetrieveComments()
        {
            int productId = Convert.ToInt32(HttpContext.Request.Query["product"].ToString());
            int page = HttpContext.Request.Query["page"].ToString() == ""
                ? 1 : Convert.ToInt32(HttpContext.Request.Query["page"].ToString());
            
            var objectComments = Comment.RetrieveComments(productId, page);

            return Json(objectComments);
        }

        public class SimpleComment
        {
            public string content { get; set; }
        }
        
        [HttpPost]
        [Route("Comments")]
        public IActionResult CreateComment([FromBody] SimpleComment simpleComment)
        {
            int accountId = HttpContext.Session.GetInt32("id") ?? -1;
            int productId = Convert.ToInt32(HttpContext.Request.Query["product"].ToString());
            int page = HttpContext.Request.Query["page"].ToString() == ""
                ? 1 : Convert.ToInt32(HttpContext.Request.Query["page"].ToString());

            Comment comment = Comment.LeaveComment(accountId, productId, simpleComment.content);
            var objectComments = Comment.RetrieveComments(productId, page);

            return Json(objectComments);
        }
    }
}
