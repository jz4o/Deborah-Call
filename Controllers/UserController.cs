using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
            ViewBag.yourself = YourSelf_Check(_result.Login_Id);
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("User/Destory")]
        public IActionResult Destroy(int id)
        {
            var _login = this._context.Mst_User.FirstOrDefault(x => x.Id == id);
            var _r = this._context.Tra_Inqury.Where(x => x.Login_Id == id).Count();
            if (_r >= 1 && !YourSelf_Check(_login.Login_Id))
            {
                ViewBag.error = "過去に登録した問合せがあるため、削除できません。";
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
                return View("Index", _result);
            }
            else
            {
                if (YourSelf_Check(_login.Login_Id))
                {
                    this._context.Remove(_login);
                    this._context.SaveChanges();
                }
                else
                {
                    ViewBag.error = "ログインユーザ自身を削除することはできません。";
                }
            }
            return RedirectToAction("Index", "User");
        }

        [AuthorizationFilter]
        [Route("User/Registrate")]
        public IActionResult Registrate(Mst_User _param)
        {
            if (ModelState.IsValid)
            {
                byte[] _salt = Generate_Salt();
                string password = Generate_Password(_param.Password, _salt);
                try
                {
                    this._context.Mst_User.Add(new Mst_User{
                        Login_Id = _param.Login_Id,
                        User_Name = _param.User_Name,
                        Hostname = _param.Hostname,
                        Password = password,
                        Password_Salt = _salt,
                        DisconnectableFlag = true
                    });
                    this._context.SaveChanges();
                }
                catch 
                {
                    ViewBag.error = "一意制約違反の可能性があります。";
                    return View("New");
                }
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.error = "更新に失敗しました。";
                return View("New");
            }
        }

        [AuthorizationFilter]
        [Route("User/Update")]
        public IActionResult Update(Mst_User _params)
        {
            var _r = this._context.Mst_User.SingleOrDefault(x => x.Id == _params.Id);
            if (ModelState.IsValid)
            {
                byte[] _salt = Generate_Salt();
                string password = Generate_Password(_params.Password, _salt);
                try
                {
                    _r.Password = password;
                    _r.User_Name = _params.User_Name;
                    _r.Hostname = _params.Hostname;
                    _r.Password_Salt = _salt;
                    this._context.SaveChanges();
                }
                catch
                {
                    ViewBag.error = "一意制約違反の可能性があります。";
                    var _result = this._context.Mst_User.Single(x => x.Id == _params.Id);
                    //ログインユーザ以外のパスワード等を変更できないようにする。
                    ViewBag.yourself = YourSelf_Check(_result.Login_Id);
                    return View("EDIT", _result);
                }
            }
            else
            {
                var _result = this._context.Mst_User.Single(x => x.Id == _params.Id);
                //ログインユーザ以外のパスワード等を変更できないようにする。
                ViewBag.yourself = YourSelf_Check(_result.Login_Id);
                return View("Edit", _result);
            }
            return RedirectToAction("Index", "User");
        }


        //暗号化されたパスワードを生成する。
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


        private bool YourSelf_Check(string _login_id)
        {
            if (_login_id != HttpContext.Session.GetString("login")) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}