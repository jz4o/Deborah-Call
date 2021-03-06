using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;
using Login.Filters;
using OfficeOpenXml;
using Deborah_Downloder;
using Pagenations;
using Search;

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
        public IActionResult Index(int now_page=1)
        {
            clear_session(); //検索結果Sessionを削除するぜ。
            DateTime _today = DateTime.Today; //今日の日付
            ViewBag.Check = false;
            var _result = from tr in this._context.Tra_Inqury
                                join usr in this._context.Mst_User
                                on tr.Login_Id equals usr.Id
                                //where tr.Start_day == _today
                                where tr.Check_Flag == false
                                where tr.Del_Flag == false
                                orderby tr.Start_day descending
                                orderby tr.Start_Time descending
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
                                    //Answer = tr.Answer,
                                    //Tan_Name = tr.Tan_Name,
                                    //Staff_Flag = tr.Staff_Flag,
                                    Complate_Flag = tr.Complate_Flag,
                                    Check_Flag = tr.Check_Flag
                                };
            //ページネーション処理
            Pagenation pages = new Pagenation(_result.AsNoTracking(), 20);
            var _result2 = pages.Pager(now_page);
            //_srにはページネーションをどこまで出すかをLIST<int>で渡している。
            var _sr = pages.Separate_now(now_page);
            if (_sr.Count() <= 1)  //１件未満の場合はViewでは非表示にする。
            {
                ViewBag.separate = new List<int>();
            }
            else
            {
                ViewBag.separate = _sr;
            }
            ViewBag.now_page = now_page;
            //ViewBag.date1 = _today;
            //ViewBag.date2 = _today;
            ViewBag.check = true;
            return View(_result2);
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
            ViewBag.staff = this._context.Tra_Inqury.Where(x => x.Tel_No == _result.Tel_No)
                                .OrderByDescending(x => x.Start_day)
                                .OrderByDescending(x => x.Start_Time)
                                .OrderByDescending(x => x.Id)
                                .Select(x => x.Staff_Flag)
                                .FirstOrDefault();
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
            if (ModelState.IsValid)
            {
                _target.Inqury = _params.Inqury;
                _target.Login_Id = _params.Login_Id;
                _target.Relation_Id = _params.Relation_Id;
                _target.Staff_Flag = _params.Staff_Flag;
                _target.Start_Time = _params.Start_Time;
                _target.Start_day = _params.Start_day;
                _target.System_Id = _params.System_Id;
                _target.Tan_Name = _params.Tan_Name;
                _target.Tel_No = _params.Tel_No;
                _target.Spare_Tel_No = _params.Spare_Tel_No;
                _target.Type_Id = _params.Type_Id;
                _target.Answer = _params.Answer;
                _target.Com_Id = _params.Com_Id;
                _target.Company_Name = _params.Company_Name;
                _target.Complate_Flag = _params.Complate_Flag;
                _target.Fin_Time = _params.Fin_Time;
                _target.Check_Flag = _params.Check_Flag;
                _target.Del_Flag = false;
                this._context.SaveChanges();
                EntryUpdate(_params.Tel_No, _params.Company_Name, _params.Tan_Name);
                //EntryUpdate(_params.Tel_No, _params.Company_Name, _params.Tan_Name);
            }
            else
            {
                ViewBag.system = Fetch_System();
                ViewBag.com = Fetch_Communication();
                ViewBag.user = Fetch_User();
                ViewBag.type = Fetch_Type();
                ViewBag.login = HttpContext.Session.GetString("login");
                ViewBag.name = Get_User_Name();
                var _result = this._context.Tra_Inqury.Where(x => x.Id == _params.Id).First();
                return View("Edit", _result);
            }
            return RedirectToAction("Index", "Inqury");
        }

        [AuthorizationFilter]
        [Route("Inqury/Destory")]
        public IActionResult Destory(int id)
        {
            var _result = this._context.Tra_Inqury.FirstOrDefault(x => x.Id == id);
            _result.Del_Flag = true;
            this._context.SaveChanges();
            return Redirect("Index");
        }

        //Tra_Entryにある削除されていないレコードを更新する処理。
        public void EntryUpdate(string tel, string company_name, string tan_name)
        {
            var _result = this._context.Tra_Entry
                            .Where(x => x.Del_Flag == false)
                            .Where(x => x.Tel_No == tel);
            if (_result == null) return; 
            foreach (var val in _result)
            {
                val.Company_Name = company_name;
                val.Tan_Name = tan_name;
            }
            this._context.SaveChanges();
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
                            where inq.Del_Flag == false
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
                                Spare_Tel_No = inq.Spare_Tel_No,
                                User_Name = usr.User_Name,
                                Login_Id = usr.Login_Id,
                                Inqury = inq.Inqury,
                                Answer = inq.Answer,
                                Complate_Flag = inq.Complate_Flag,
                                Check_Flag = inq.Check_Flag,
                                Start_day = inq.Start_day,
                                Start_Time = inq.Start_Time,
                                Fin_Time = inq.Fin_Time
                            }).First();
            //ログインユーザは確認ボタンを押せない。
            if (show_data.Login_Id == HttpContext.Session.GetString("login"))
            {
                ViewBag.self = "true";
            }
            else
            {
                ViewBag.self = "false";
            }
            return View(show_data);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult Complate(int id)
        {
            var _result = this._context.Tra_Inqury
                            .Where(x => x.Id == id)
                            .Where(x => x.Del_Flag == false)
                            .Where(x => x.Check_Flag == false).First();
            if (_result.Id == id)
            {
                _result.Check_Flag = true;
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
            if (ModelState.IsValid)
            {
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
                EntryUpdate(_param.Tel_No, _param.Company_Name, _param.Tan_Name);
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

        //検索用のアクションです。検索条件はsessionに保持します。
        [AuthorizationFilter]
        [Route("Inqury/Search")]
        public IActionResult Search(Search_param _params, int now_page=1)
        {
            clear_session(); //Sessionのクリア
            if (_params.Start_day.ToString("yyyy") != null)
            {
                HttpContext.Session.SetString("date1", _params.Start_day.ToString("yyyy-MM-dd"));
                ViewBag.date1 = _params.Start_day.ToString("yyyy") == "0001" ? null : HttpContext.Session.GetString("date1");
            }
            else
            {
                ViewBag.date1 = "yyyy-MM-dd";
            }
            if (_params.End_day != null)
            {
                HttpContext.Session.SetString("date2", _params.End_day.ToString("yyyy-MM-dd"));
                ViewBag.date2 = _params.End_day.ToString("yyyy") == "0001" ? null : HttpContext.Session.GetString("date2");
            }
            else
            {
                ViewBag.date2 = "yyyy-MM-dd";
            }
            if (_params.Word != null)
            {
                HttpContext.Session.SetString("freeword", _params.Word);
                ViewBag.freeword = HttpContext.Session.GetString("freeword");
            }
            ViewBag.Check = _params.Check ? true : false;
            if (_params.Check)
            {
                HttpContext.Session.SetString("check", "Checked");
            }
            var _list = Search_Target();
            SearchInqury _search_inqury = new SearchInqury(_list, _params.Start_day, _params.End_day, _params.Check, _params.Word);
            var _result = _search_inqury.Search_start();
            if (_result.Count() == 0)
            {
                ViewBag.error = "　該当データはありません";
            }
            else
            {
                ViewBag.error = String.Format("　{0}件ヒット", _result.Count().ToString());
            }
            Pagenation pages = new Pagenation(_result.AsQueryable().AsNoTracking(), 20);
            var _result2 = pages.Pager(now_page);
            //_srにはページネーションをどこまで出すかをLIST<int>で渡している。
            var _sr = pages.Separate_now(now_page);
            if (_sr.Count() <= 1)  //１件未満の場合はViewでは非表示にする。
            {
                ViewBag.separate = new List<int>();
            }
            else
            {
                ViewBag.separate = _sr;
            }
            ViewBag.now_page = now_page;
            return View("Index", _result2);
        }
        [HttpGet]
        //[AuthorizationFilter] Excelファイルをダウンロードさせるため、ログイン必須を解除
        public async Task<IActionResult> Export(Search_param _params, bool excel=false, bool monthly_report = false)
        {
            // Sessionに保持している値を変数に格納する。（後にサブルーチンに渡します）
            var check = _params.Check;
            var date1 = Convert.ToDateTime(_params.Start_day);
            var date2 = Convert.ToDateTime(_params.End_day);
            var word = _params.Word;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //SHIFT_JISを使えるようにする。
            StringBuilder csv = new StringBuilder("");
            Downloader _downloader = new Downloader(this._context);
            //CSVのヘッダー情報を取得する。
            IQueryable<Mst_Download> header = await Task.Run(() => this._context.Mst_Download
                            .OrderBy(x => x.Order_No)
                            .OrderBy(x => x.Id).AsQueryable());
            int _length = header.Select(x => x.Column_Name).Count(); //要素数を取得する。
            foreach(var v in header)
            {
                if(csv.Length >= 1)
                {
                    csv.Append(",");
                }
                csv.Append(v.Column_Name);
            }
            csv.Append("\r\n");
            csv.Append(_downloader.Get_Inqury(header, _length, date1, date2, check, word));
            if (excel)  //Excelのダウンロードの際に使用する。
            {
                //string date = "";
                string file_name = "";
                //Excelファイル名定義
                if (monthly_report) //月報の場合
                {
                    file_name = _params.Start_day.ToString("yyyy年M月月報");
                }
                else if (_params.Start_day.ToString() == "0001/01/01 0:00:00" && _params.End_day.ToString() == "0001/01/01 0:00:00")
                {
                    file_name = String.Format(@"問合せ表（{0}）", DateTime.Today.ToString("M月dd日"));
                }
                else if (_params.Start_day.ToString() == "0001/01/01 0:00:00")
                {
                    file_name = String.Format(@"問合せ表（{0}）", _params.End_day.ToString("M月dd日"));
                }
                else
                {
                    file_name = String.Format(@"問合せ表（{0}）", _params.Start_day.ToString("M月dd日"));
                }

                byte[] excel_file = _downloader.Create_Excel(file_name, csv);
                return File(excel_file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name + ".xlsx");
            }
            var result = Encoding.GetEncoding("Shift_JIS").GetBytes(csv.ToString()); //文字列をバイナリ化
            return File(result, "text/csv", "inquiry.csv");
        }


        //sessionに入っている値をクリアします。
        public void clear_session()
        {
            HttpContext.Session.Remove("date1");
            HttpContext.Session.Remove("date2");
            HttpContext.Session.Remove("freeword");
            HttpContext.Session.Remove("check");
            return;
        }

        public IEnumerable<MyList> Search_Target()
        {
            IEnumerable<MyList> _result = from tr in this._context.Tra_Inqury
                                            join usr in this._context.Mst_User
                                            on  tr.Login_Id equals usr.Id
                                            where tr.Del_Flag == false
                                            orderby tr.Start_day descending
                                            orderby tr.Start_Time descending
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
                                                Answer = tr.Answer,
                                                Tan_Name = tr.Tan_Name,
                                                //Staff_Flag = tr.Staff_Flag,
                                                //Complate_Flag = tr.Complate_Flag,
                                                Check_Flag = tr.Check_Flag
                                            };
            return _result;
        }
    }
}