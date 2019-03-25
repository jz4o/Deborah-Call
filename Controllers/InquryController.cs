using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace InquryController
{
    public class MyList
    {
        public int Id { get; set; }
        public DateTime Start_Time { get; set; }
        public string Company_Name { get; set; }
        public string Tel_No { get; set; }
        public string User_Name { get; set; }
        public string Inqury { get; set; }
        public bool Staff_Flag { get; set; }
        public bool Complate_Flag { get; set; }
    }
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
            var inqury_list = from tr in this._context.Tra_Inqury
                                    join usr in this._context.Mst_User
                                    on  tr.Login_Id equals usr.Id
                                    orderby(tr.Start_Time)
                                    select new MyList
                                    {
                                        Id = tr.Id,
                                        Start_Time = tr.Start_Time,
                                        Company_Name = tr.Company_Name,
                                        Tel_No = tr.Tel_No,
                                        User_Name = usr.User_Name,
                                        Inqury = tr.Inqury,
                                        Staff_Flag = tr.Staff_Flag,
                                        Complate_Flag = tr.Complate_Flag
                                    };
            ViewBag.list = inqury_list;
            return View(this._context.Tra_Inqury);
        }

        [Route("Inqury/New")]
        public IActionResult New()
        {
            ViewBag.system = Fetch_System();
            ViewBag.com = Fetch_Communication();
            ViewBag.user = Fetch_User();
            ViewBag.type = Fetch_Type();
            ViewBag.login = HttpContext.Session.GetString("login");
            ViewBag.name = Get_User_Name();
            return View();
        }
        //データ登録を行います（insert）
        [Route("Inqury/Registrate")]
        public IActionResult Registrate(Tra_Inqury _param)
        {
            if (ModelState.IsValid)
            {
                this._context.Tra_Inqury.Add(_param);
                this._context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.system = Fetch_System();
                ViewBag.com = Fetch_Communication();
                ViewBag.user = Fetch_User();
                ViewBag.type = Fetch_Type();
                ViewBag.login = HttpContext.Session.GetString("login");
                ViewBag.name = Get_User_Name();
                return View("New");
            }
        }

        //システム名をディクショナリ型で生成します。
        public Dictionary<int, string> Fetch_System()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var _result = this._context.Mst_System.OrderBy(r => r.Id);
            foreach (var item in _result)
            {
                dic.Add(item.Id, item.System_name);
            }
            return dic;
        }

        //連絡方法をディクショナリ型で返します。
        public Dictionary<int, string> Fetch_Communication()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var _result = this._context.Mst_Communication.OrderBy(r => r.Id);
            foreach (var item in _result)
            {
                dic.Add(item.Id, item.Com_Name);
            }
            return dic;
        }

        //ユーザ情報をディクショナリ型で返します。
        public Dictionary<int, string> Fetch_User()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var _result = this._context.Mst_User.OrderBy(r => r.Id);
            foreach (var item in _result)
            {
                dic.Add(item.Id, item.User_Name);
            }
            return dic;
        }
        
        //問合せ分類をディクショナリ型で返します。
        public Dictionary<int, string> Fetch_Type()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var _result = this._context.Mst_Type.OrderBy(r => r.Id);
            foreach (var item in _result)
            {
                dic.Add(item.Id, item.Type_Name);
            }
            return dic;
        }
        //User_Nemeを取得します
        public string Get_User_Name()
        {
            var _result = this._context.Mst_User.Where(r => r.Login_Id == HttpContext.Session.GetString("login"));
            string _username = "";
            foreach (var n in _result)
            {
                _username = n.User_Name;
            }
            return _username;
        }
    }
}