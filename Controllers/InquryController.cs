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
            ViewBag.system = Fetch_System();
            ViewBag.com = Fetch_Communication();
            ViewBag.user = Fetch_User();
            ViewBag.type = Fetch_Type();
            ViewBag.login = HttpContext.Session.GetString("login");
            ViewBag.name = Get_User_Name();
            return View();
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