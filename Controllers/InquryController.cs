using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;

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

        [Route("Inqury/Show/{id}")]
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var show_data = (from inq in this._context.Tra_Inqury
                                join usr in this._context.Mst_User
                                on inq.Login_Id equals usr.Id
                                join sys in this._context.Mst_System
                                on inq.System_Id equals sys.Id
                                join type in this._context.Mst_Type
                                on inq.Type_Id equals type.Id
                                join com in this._context.Mst_Communication
                                on inq.Com_Id equals com.Id
                            where inq.Id == id
                            select new Show_List
                            {
                                id = inq.Id,
                                System_Name = sys.System_name,
                                Com_Name = com.Com_Name,
                                Type_Name = type.Type_Name,
                                Relation_Id = inq.Relation_Id,
                                Staff_Flag = inq.Staff_Flag,
                                Company_Name = inq.Company_Name,
                                Tan_Name = inq.Tan_Name,
                                Tel_No = inq.Tel_No,
                                User_Name = usr.User_Name,
                                Inqury = inq.Inqury,
                                Answer = inq.Answer,
                                Complate_Flag = inq.Complate_Flag,
                                Start_day = inq.Start_day,
                                Start_Time = inq.Start_Time,
                                Fin_Time = inq.Fin_Time
                            }).First();
            Console.WriteLine(show_data.Inqury);
            return View(show_data);
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