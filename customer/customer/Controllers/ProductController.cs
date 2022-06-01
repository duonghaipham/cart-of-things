using Microsoft.AspNetCore.Mvc;
using customer.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace customer.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("Shop")]
        public IActionResult Index()
        {
            int page = int.TryParse(HttpContext.Request.Query["page"], out page) ? page : 1;
            string category = HttpContext.Request.Query["category"];
            string search = HttpContext.Request.Query["search"];
            string sort = HttpContext.Request.Query["sort"];

            string url = HttpContext.Request.GetDisplayUrl();
            url = url.Split("page=")[0];
            
            if (url[url.Length - 1] != '?' && url[url.Length - 1] != '&') {
                if (url.Contains('?'))
                {
                    url += '&';
                } 
                else
                {
                    url += '?';
                }
            }
            
            string jsonCategories = Category.GetAll();
            var jsonProductsAndPagination = Product.RetrieveProducts(page, category, search, sort);

            ViewData["jsonCategories"] = jsonCategories;
            ViewData["jsonProducts"] = jsonProductsAndPagination;
            ViewData["url"] = url;
            ViewData["page"] = page;
            ViewData["category"] = category;
            ViewData["search"] = search;
            ViewData["sort"] = sort;

            return View();
        }

        [HttpGet]
        [Route("Products/{productId?}")]
        public IActionResult View(int productId)
        {
            var jsonProduct = Product.RetrieveProduct(productId);
            var categoryId = Product.RetrieveCategoryId(productId);
            var jsonRelatedProducts = Product.RetrieveProductsRelatedToCategory(categoryId ?? 1, productId);

            ViewData["jsonProduct"] = jsonProduct;
            ViewData["jsonRelatedProducts"] = jsonRelatedProducts;

            return View();
        }
    }
}
