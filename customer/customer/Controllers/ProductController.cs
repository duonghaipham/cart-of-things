using System;
using Microsoft.AspNetCore.Mvc;
using customer.Models;

namespace customer.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("Shop")]
        public IActionResult Index()
        {
            string jsonCategories = Category.GetAll();
            string jsonProducts = Product.RetrieveProducts();

            ViewData["jsonCategories"] = jsonCategories;
            ViewData["jsonProducts"] = jsonProducts;

            return View();
        }

        [HttpGet]
        [Route("Products/{productId?}")]
        public IActionResult View(int productId)
        {
            string jsonProduct = Product.RetrieveProduct(productId);

            ViewData["jsonProduct"] = jsonProduct;

            return View();
        }
    }
}
