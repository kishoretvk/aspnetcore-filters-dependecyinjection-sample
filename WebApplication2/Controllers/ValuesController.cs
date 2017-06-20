﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Filters;
using WebApplication2.service;

namespace WebApplication2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values

        [HttpGet]
       [ServiceFilter(typeof(ResponseFilter))]
        public IEnumerable<string> Get()
        {
            var abc = ViewData["token"];
            var d = HttpContext.Items["tokens"];

            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        public async Task<JsonResult> JsonTest()
        {
            var obj = new 
            {
                a = "welcome",
                b= "hello"
            };
            return Json(obj);
        }
        // GET api/values/5
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ResponseFilter))] 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}