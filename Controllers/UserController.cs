using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;
using Login.Filters;
using Deborah_User;

namespace UserController
{
    public class UserController : Controller
    {
        private readonly MyContext _context;
        public UserController(MyContext context)
        {
            this._context = context;
        }

        [AuthorizationFilter]
        [Route("User/Index")]
        public IActionResult Index()
        {
            var _result = from usr in this._context.Mst_User
                                orderby usr.Id
                                select new User
                                {
                                    Id = usr.Id,
                                    User_Name = usr.User_Name,
                                    DisconnectableFlag = usr.DisconnectableFlag,
                                    InquryCount = this._context.Tra_Inqury
                                                      .Where(x => x.Login_Id == usr.Id)
                                                      .Select(x => x.Id)
                                                      .Count()
                                };
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("User/New")]
        public IActionResult New()
        {
            return View();
        }

        [AuthorizationFilter]
        [Route("User/Edit")]
        public IActionResult Edit(int id)
        {
            var _result = this._context.Mst_User.Single(x => x.Id == id);
            //ログインユーザ以外のパスワード等を変更できないようにする。
            if (_result.Login_Id != HttpContext.Session.GetString("login")) 
            {
                ViewBag.yourself = true;
            }
            else
            {
                ViewBag.yourself = false;
            }
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("User/Destory")]
        public IActionResult Destroy(int id)
        {
            var _r = this._context.Tra_Inqury.Where(x => x.Login_Id == id).Count();
            if (_r >= 1)
            {
                ViewBag.error = "過去に登録した問合せがあるため、削除できません。";
            }
            else
            {
                Mst_User result_user = this._context.Mst_User.Single(x => x.Id == id);
                this._context.Remove(result_user);
                this._context.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }

        [AuthorizationFilter]
        [Route("User/Registrate")]
        public IActionResult Registrate(Mst_User _param)
        {
            if (ModelState.IsValid)
            {
                this._context.Mst_User.Add(_param);
                this._context.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.error = "更新に失敗しました。";
                return View("New");
            }
        }
    }
}