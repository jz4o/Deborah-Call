using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Inqury.Models;
using Login.Filters;
using Sammarys.Models;
//using UserController;

namespace TopController
{
    public class TopController : Controller
    {
        private readonly MyContext _context;
        public TopController(MyContext context)
        {
            this._context = context;
        }

        public IActionResult FirstUser()
        {
            byte[] _salt = Generate_Salt();
            string password = Generate_Password("deborah_" + DateTime.Now.ToString("yyyy"), _salt);
            try
            {
                this._context.Mst_User.Add(new Mst_User{
                    Login_Id = "admin",
                    User_Name = "管理者",
                    Hostname = "test_hostname",
                    Password = password,
                    Password_Salt = _salt,
                    DisconnectableFlag = true
                });
                this._context.SaveChanges();
            }
            catch
            {
                ViewBag.error = "初回ユーザ登録に失敗しました";
            }
            return Redirect("Login");
        }

        
        [Route("Top/Login")]
        public IActionResult Login(int code=0)
        {
            if (code == 1)
            {
                ViewBag.error = "この先はログインが必要です";
            }
            int cnt = this._context.Mst_User.Count();
            if (cnt < 1) { ViewBag.first = true; }
            return View();
        }

        [HttpPost]
        public IActionResult Create(string login, string password)
        {
            if (Certification(login, password))
            {
                return Redirect("Sammary");
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

        //迷惑電話登録アクション
        [AuthorizationFilter]
        [Route("Top/MissTel")]
        public IActionResult MissTel(int id)
        {
            var _result = this._context.Tra_Entry.Where(x => x.Id == id).Where(x => x.Del_Flag == false).SingleOrDefault();
            if (_result == null)
            {
                ViewBag.error = "既に削除されています。";
            }
            else
            {  
                _result.Del_Flag = true;
                this._context.SaveChanges();
            }
            return RedirectToAction("Menu", "Top");
        }

        private bool Certification(string login, string password)
        {
            var result = this._context.Mst_User.Where(r => r.Login_Id == login).SingleOrDefault();
            if (result == null) return false;
            if (result.Password_Salt == null) return false;
            byte[] salt = result.Password_Salt;
            string hash_pass = Generate_Password(password, salt);
            
            if (result.Password == hash_pass)
            {
                HttpContext.Session.SetString("login", login);
                return true;
            }
            return false;
        }
        

        private byte[] Generate_Salt()
        {
            byte[] salt = new byte[128 / 8];
            //暗号乱数ジェネレーターのインスタンス
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                return salt;
            }
        }
        private string Generate_Password(string password, byte[] salt)
        {
            string hash_pass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256/8
                )
            );
            return hash_pass;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("login", "");
            ViewBag.error = "ログアウトしました";
            return View("Login");
        }

        [AuthorizationFilter]
        public IActionResult Sammary()
        {
            Sammary smy = new Sammary(this._context);
            ViewBag.total = smy.get_sammary_total();
            var system = smy.get_sammary_system();
            if (system != null) ViewBag.max = system.Max(x => x.Id);
            ViewBag.staff = smy.get_sammary_Staff();
            ViewBag.juchusha = ViewBag.total - ViewBag.staff;
            return View("Sammary", system);
        }
    }
}