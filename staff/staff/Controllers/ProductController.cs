﻿using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using staff.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using staff.Models;


namespace staff.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("Products")]
        public IActionResult Retrieve(string search, string sort)
        {
            ViewBag.Active = "Products";
            //System.Diagnostics.Debug.WriteLine(sort);
            if (search != null)
            {
                ViewData["jsonSearch"] = Product.search(search, sort);
                ViewData["keySearch"] = (string)search;
                ViewData["keySort"] = (string)sort;
                ViewBag.search = true;
            }
            else
            {
                ViewData["keySearch"] = "";
            }
            return View();
        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult getProducts(int page)
        {
            var jsonList = Product.getList(page);
            return Json(new
            { success = jsonList });
        }

        [HttpGet]
        [Route("GetTotalProduct")]
        public IActionResult getTotalProduct()
        {
            return Json(new
            {
                total = Product.totalProduct(),
                limit = Pagination.ITEM_PER_PAGE
            });
        }

        [HttpGet]
        [Route("Products/create")]
        public IActionResult Create()
        {
            ViewBag.Active = "Products";
            ViewData["Categories"] = Category.getList();
            return View();
        }

        [HttpPost]
        [Route("Products/create")]
        public IActionResult Create(IFormFile avatar, int category, string introduction, double price, string description, string name)
        {
            string pathAvatar = "";
            if (avatar?.Length > 0)
                pathAvatar = ImageManager.GetInstance().Upload(avatar).SecureUrl.AbsoluteUri.ToString();

            try
            {
                var rs = Product.createProduct(pathAvatar, category, introduction, price, description, name);
                //System.Diagnostics.Debug.WriteLine(rs.Id);
                if (rs != null)
                    return Redirect("/products");
                else
                {
                    ViewBag.Message = "Create failed";
                    return View();
                }                   
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                Console.WriteLine(e.Message);
                return View();
            }
        }

        [HttpGet]
        [Route("Products/{Id?}/update")]
        public IActionResult Update(int Id)
        {
            ViewBag.Active = "Products";
            ViewData["Product"] = Product.getProduct(Id);
            ViewData["Categories"] = Category.getList();
            return View();
        }

        [HttpPost]
        [Route("Products/{Id?}/update")]
        public IActionResult Update(int Id, IFormFile avatar, int category, string introduction, double price, string description, string name)
        {

            string pathAvatar = "";
            if (avatar?.Length > 0)
                pathAvatar = ImageManager.GetInstance().Upload(avatar).SecureUrl.AbsoluteUri.ToString();

            try
            {
                var rs = Product.updateProduct(Id, pathAvatar, category, introduction, price, description, name);
                //System.Diagnostics.Debug.WriteLine(rs.Id);
                if (rs)
                    return Redirect("/products");
                else
                {
                    ViewBag.Message = "Update failed";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                Console.WriteLine(e.Message);
                return View();
            }
        }

        [HttpGet]
        [Route("Products/{Id?}/delete")]
        public IActionResult Delete(int Id)
        {
            try
            {
                ViewBag.Active = "Products";
                var rs = Product.deleteProduct(Id);
                
                if (rs)
                    return Redirect("/products");
                else
                {
                    ViewBag.Message = "Deleted failed";
                    return View();
                }    
                    
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                Console.WriteLine(e.Message);
                return View();
            }
        }

    }
}
