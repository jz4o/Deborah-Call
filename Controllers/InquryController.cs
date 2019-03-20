using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace InquryController
{
    public class InquryController : Controller
    {
        private readonly MyContext _context;
        public InquryController(MyContext context)
        {
            this._context = context;
        }

        [Route("Inqury/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Inqury/New")]
        public IActionResult New()
        {
            ViewBag.system = Date_Fetch("Mst_System");
            return View();
        }

        public Dictionary<int, string> Date_Fetch(string table_name)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var _result = this._context.Mst_System.OrderBy(r => r.Id);
            foreach (var item in _result)
            {
                dic.Add(item.Id, item.System_name);
            }
            return dic;
        }
    }
}