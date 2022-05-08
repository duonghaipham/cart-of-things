﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.Controllers
{
    public class PlaceController : Controller
    {
        [HttpGet]
        [Route("Places")]
        public IActionResult Retrieve()
        {
            return View();
        }

        [HttpPost]
        [Route("Places/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("Places/{Id?}/update")]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPut]
        [Route("Places/{Id?}/update")]
        public IActionResult Update(string a)
        {
            return View();
        }
    }
}