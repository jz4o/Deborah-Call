using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace TopController
{
    public class TopController : Controller
    {
        private readonly MyContext _context;
        public TopController(MyContext context)
        {
            this._context = context;
        }

        [Route("Top/Login")]
        public IActionResult Login()
        {
            ViewBag.nav = true;
            return View();
        }

        [HttpPost]
        public IActionResult Create(string username, string password)
        {
            if (Certification(username, password))
            {
                return Redirect("Menu");
            }
            else
            {
                ViewBag.error= "ログインに失敗しました";
                return View("Login");
            }
        }

        [Route("Top/Menu")]
        public IActionResult Menu()
        {
            ViewData["login"] = HttpContext.Session.GetString("login");
            return View();
        }

        public bool Certification(string username, string password)
        {
            var result = this._context.Mst_User.Where(r => r.User_Name == username).Where(r => r.Password == password).ToList();
            if (result.Any())
            {
                HttpContext.Session.SetString("login", username);
                return true;
            }
            return false;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("login", "");
            ViewBag.error = "ログアウトしました";
            return View("Login");
        }
    }
}