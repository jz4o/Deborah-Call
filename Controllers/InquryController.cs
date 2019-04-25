using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;
using Login.Filters;



namespace InquryController
{
    public class InquryController : Controller
    {
        
        private readonly MyContext _context;
        
        public InquryController(MyContext context)
        {
            this._context = context;
        }
        
        [AuthorizationFilter]
        [Route("Inqury/Index")]
        public IActionResult Index()
        {
            var _result = from tr in this._context.Tra_Inqury
                                join usr in this._context.Mst_User
                                on tr.Login_Id equals usr.Id
                                orderby tr.Id descending
                                select new MyList
                                {
                                    Id = tr.Id,
                                    Start_day = tr.Start_day,
                                    Start_Time = tr.Start_Time,
                                    Company_Name = tr.Company_Name,
                                    Tel_No = tr.Tel_No,
                                    User_Name = usr.User_Name,
                                    Inqury = tr.Inqury,
                                    Staff_Flag = tr.Staff_Flag,
                                    Complate_Flag = tr.Complate_Flag
                                };
            return View(_result);
        }
        
        [AuthorizationFilter]
        [Route("Inqury/Entry/{id}", Name="Entry") ]
        [HttpGet("{id}")]
        public IActionResult Entry(int id)
        {
            ViewBag.system = Fetch_System();
            ViewBag.com = Fetch_Communication();
            ViewBag.user = Fetch_User();
            ViewBag.type = Fetch_Type();
            ViewBag.login = HttpContext.Session.GetString("login");
            ViewBag.name = Get_User_Name();
            var _result = this._context.Tra_Entry.Where(x => x.Id == id).Where(x => x.Del_Flag == false).First();
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("Inqury/Copy/{id}")]
        [HttpGet("{id}")]
        public IActionResult Copy(int id)
        {
            ViewBag.system = Fetch_System();
            ViewBag.com = Fetch_Communication();
            ViewBag.user = Fetch_User();
            ViewBag.type = Fetch_Type();
            ViewBag.login = HttpContext.Session.GetString("login");
            ViewBag.name = Get_User_Name();
            var _result = this._context.Tra_Inqury.Where(x => x.Id == id).First();
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("Inqury/Edit/{id}")]
        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.system = Fetch_System();
            ViewBag.com = Fetch_Communication();
            ViewBag.user = Fetch_User();
            ViewBag.type = Fetch_Type();
            ViewBag.login = HttpContext.Session.GetString("login");
            ViewBag.name = Get_User_Name();
            var _result = this._context.Tra_Inqury.Where(x => x.Id == id).First();
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("Inqury/Update")]
        [HttpPost]
        public IActionResult Update(Tra_Inqury _params)
        {
            var _target = this._context.Tra_Inqury.Single(x => x.Id == _params.Id);
            _target.Inqury = _params.Inqury;
            _target.Login_Id = _params.Login_Id;
            _target.Relation_Id = _params.Relation_Id;
            _target.Staff_Flag = _params.Staff_Flag;
            _target.Start_Time = _params.Start_Time;
            _target.Start_day = _params.Start_day;
            _target.System_Id = _params.System_Id;
            _target.Tan_Name = _params.Tan_Name;
            _target.Tel_No = _params.Tel_No;
            _target.Type_Id = _params.Type_Id;
            _target.Answer = _params.Answer;
            _target.Com_Id = _params.Com_Id;
            _target.Company_Name = _params.Company_Name;
            _target.Complate_Flag = _params.Complate_Flag;
            _target.Fin_Time = _params.Fin_Time;
            this._context.SaveChanges();
            return RedirectToAction("Index", "Inqury");
        }

        [AuthorizationFilter]
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
                                Id = inq.Id,
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
            return View(show_data);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult Complate(int id)
        {
            var _result = this._context.Tra_Inqury
                            .Where(x => x.Id == id)
                            .Where(x => x.Complate_Flag == false).First();
            if (_result.Id == id)
            {
                _result.Complate_Flag = true;
                this._context.SaveChanges();
            }
            return RedirectToAction("Index", "Inqury");
        }

        [AuthorizationFilter]
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
        [AuthorizationFilter]
        [Route("Inqury/Registrate")]
        public IActionResult Registrate(Tra_Inqury _param)
        {
            //if (ModelState.IsValid)
            //{
                if (Request.Headers["Referer"].ToString().Contains("Entry"))
                {
                    if (Del_Entry(_param.Entry_Id) == false)
                    {
                        ViewBag.error = "更新に失敗しました。";
                        return RedirectToAction("Menu", "Top");
                    }  
                }
                this._context.Tra_Inqury.Add(_param);
                this._context.SaveChanges();
                return RedirectToAction("Index");
            //}
            //else
            //{
            //    ViewBag.system = Fetch_System();
            //   ViewBag.com = Fetch_Communication();
            //    ViewBag.user = Fetch_User();
            //    ViewBag.type = Fetch_Type();
            //    ViewBag.login = HttpContext.Session.GetString("login");
            //    ViewBag.name = Get_User_Name();
            //    return View("New");
            //}
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

        public bool Del_Entry(int id)
        {
            bool kekka = false;
            var _result = this._context.Tra_Entry.Where(x => x.Id == id).First();
            if (_result.Del_Flag == false)
            {
                _result.Del_Flag = true;
                this._context.SaveChanges();
                kekka = true;
            }
            return kekka;
        }

        [AuthorizationFilter]
        [Route("Inqury/Search")]
        public IActionResult Search(Search_param _params)
        {
            IEnumerable<MyList> _result = from tr in this._context.Tra_Inqury
                                    join usr in this._context.Mst_User
                                    on  tr.Login_Id equals usr.Id
                                    orderby tr.Id descending
                                    select new MyList
                                    {
                                        Id = tr.Id,
                                        Start_day = tr.Start_day,
                                        Start_Time = tr.Start_Time,
                                        Company_Name = tr.Company_Name,
                                        Tel_No = tr.Tel_No,
                                        User_Name = usr.User_Name,
                                        Inqury = tr.Inqury,
                                        Staff_Flag = tr.Staff_Flag,
                                        Complate_Flag = tr.Complate_Flag
                                    };
            _result = _result.Where(x => x.Inqury.Contains(_params.Word));
            return View("Index", _result);
        }
    }
}