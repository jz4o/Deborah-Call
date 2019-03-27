using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace TopController
{
    public class Entry
    {
        public int Id { get; set; }
        public string Tel_No { get; set; }
        public DateTime Entry_Time { get; set; }
        public DateTime End_Time { get; set; }
        public string Login_Id { get; set; }
        public string Company_Name { get; set; }
        public string Tan_Name { get; set; }
    }
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

        [Route("Top/Menu")]
        public IActionResult Menu()
        {
            ViewData["login"] = HttpContext.Session.GetString("login");
            var inqury = (from data in this._context.Tra_Inqury
                        where data.Tel_No != "0不明"
                        where data.Tel_No != ""
                        where data.Tan_Name != "不明"
                        where data.Tan_Name != ""
                        where data.Company_Name != "不明"
                        where data.Company_Name != "0"
                        select new
                        {
                            Tel_No = data.Tel_No,
                            Tan_Name = data.Tan_Name,
                            Company_Name = data.Company_Name
                        }).Distinct();

            var _result = from ent in this._context.Tra_Entry
                                join usr in this._context.Mst_User
                                on ent.Hostname equals usr.Hostname
                                join inq in inqury
                                on ent.Tel_No equals inq.Tel_No
                                where usr.Login_Id == HttpContext.Session.GetString("login")
                                select new Entry
                                {
                                    Id = ent.Id,
                                    Tel_No = ent.Tel_No,
                                    Entry_Time = ent.Entry_Time,
                                    End_Time = ent.End_Time,
                                    Login_Id = usr.Login_Id,
                                    Company_Name = inq.Company_Name,
                                    Tan_Name = inq.Tan_Name
                                };
            ViewBag.entry = _result;
            return View();
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