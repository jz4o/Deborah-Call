using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Inqury.Models;
using Login.Filters;

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
        public IActionResult Login(int code=0)
        {
            if (code == 1)
            {
                ViewBag.error = "この先はログインが必要です";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(string login, string password)
        {
            if (Certification(login, password))
            {
                return Redirect("Menu");
            }
            else
            {
                ViewBag.error= "ログインに失敗しました";
                return View("Login");
            }
        }

        [AuthorizationFilter]
        [Route("Top/Menu")]
        public IActionResult Menu()
        {
            ViewData["login"] = HttpContext.Session.GetString("login");
            var _result = from ent in this._context.Tra_Entry
                                join usr in this._context.Mst_User
                                on ent.Hostname equals usr.Hostname into entrys
                                from usr in entrys.DefaultIfEmpty()
                                where (usr.Login_Id == HttpContext.Session.GetString("login") || ent.Hostname == "" || ent.Hostname == null)
                                where ent.Del_Flag == false
                                orderby (ent.Id)
                                orderby (ent.Entry_Time)
                                select new Entry
                                {
                                    Id = ent.Id,
                                    Tel_No = ent.Tel_No,
                                    Entry_Time = ent.Entry_Time,
                                    End_Time = ent.End_Time,
                                    Login_Id = usr.Login_Id,
                                    Company_Name = ent.Company_Name,
                                    Tan_Name = ent.Tan_Name
                                };
            ViewBag.cnt = _result.Count();
            return View(_result);
        }

        public bool Certification(string login, string password)
        {
            var result = this._context.Mst_User.Where(r => r.Login_Id == login).Where(r => r.Password == password).ToList();
            if (result.Any())
            {
                HttpContext.Session.SetString("login", login);
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