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

namespace admin.Controllers
{
    public class PlaceController : Controller
    {
        [HttpGet]
        [Route("Places")]
        public IActionResult Retrieve(string search, string sort)
        {
            ViewBag.Active = "Places";
            //System.Diagnostics.Debug.WriteLine(sort);
            if (search != null)
            {
                ViewData["searchOfPlace"] = Place.search(search, sort);
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
        [Route("GetPlaces")]
        public IActionResult getPlaces(int page)
        {
            var jsonList = Place.getListOfPag(page);
            return Json(new
            { success = jsonList });
        }

        [HttpGet]
        [Route("GetTotalPlace")]
        public IActionResult getTotalPlace()
        {
            return Json(new
            {
                total = Place.totalPlace(),
                limit = Pagination.ITEM_PER_PAGE
            });
        }

        public class NewPlace
        {
            public string placeName { get; set; }
            public string address { get; set; }
        }

        [HttpPost]
        [Route("Places/create")]
        public IActionResult Create([FromBody] NewPlace place)
        {
            var result = Place.createPlace(place.placeName, place.address);

            if (result)
                return Json(new
                { msg = "successed", url = "/Places" });
            return Json(new{ msg = "failed" });
        }

        [HttpGet]
        [Route("Places/{Id?}/update")]
        public IActionResult Update(int Id)
        {
            ViewBag.Active = "Places";

            ViewData["Place"] = Place.getPlace(Id);
            return View();
        }

        [HttpPut]
        [Route("Places/{Id?}/update")]
        public IActionResult Update([FromBody] NewPlace place, int Id)
        {
            var result = Place.updatePlace(Id, place.placeName, place.address);
            if (result)
                return Json(new
                { msg = "successed", url = "/Places" });
            return Json(new { msg = "failed" });
        }
    }
}
