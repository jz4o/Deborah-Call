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
    }
}