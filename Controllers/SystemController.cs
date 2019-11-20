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

namespace SystemController
{
    public class SystemController : Controller
    {
        private readonly MyContext _context;
        public SystemController(MyContext context)
        {
            this._context = context;
        }

        [AuthorizationFilter]
        [Route("System/Index")]
        public IActionResult Index()
        {
            var _result = this._context.Mst_System.OrderBy(x => x.Id);
            //if (_result != null) this._context.Entry(_result).Reload();
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("System/New")]
        public IActionResult New()
        {
            return View();
        }

        [AuthorizationFilter]
        [Route("System/Edit")]
        public IActionResult Edit(int id)
        {
            var _result = this._context.Mst_System.Single(x => x.Id == id);
            return View(_result);
        }

        [AuthorizationFilter]
        [Route("System/Destory")]
        public IActionResult Destroy(int id)
        {
            var _r = this._context.Tra_Inqury.Where(x => x.System_Id == id).Count();
            if (_r >= 1)
            {
                ViewBag.error = "過去に登録した問合せがあるため、削除できません。";
                var _result = this._context.Mst_System.OrderBy(x => x.Id);
                return View("Index", _result);
            }
            else
            {
                Mst_System result_sys = this._context.Mst_System.Single(x => x.Id == id);
                this._context.Remove(result_sys);
                this._context.SaveChanges();
            }
            return RedirectToAction("Index", "System");
        }

        [AuthorizationFilter]
        [Route("System/Registrate")]
        public IActionResult Registrate(Mst_System _param)
        {
            if (ModelState.IsValid)
            {
                this._context.Mst_System.Add(_param);
                this._context.SaveChanges();
                return RedirectToAction("Index", "System");
            }
            else
            {
                ViewBag.error = "更新に失敗しました。";
                return View("New");
            }
        }
        [AuthorizationFilter]
        [Route("System/Update")]
        public IActionResult Update(Mst_System _params)
        {
            var _r = this._context.Mst_System.SingleOrDefault(x => x.Id == _params.Id);
            if (ModelState.IsValid)
            {
                _r.System_name = _params.System_name;
                _r.OmmitName = _params.OmmitName;
                this._context.Entry(_r).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                this._context.SaveChanges();
            }
            else
            {
                var _result = this._context.Mst_System.Single(x => x.Id == _params.Id);
                return View("Edit", _result);
            }
            return RedirectToAction("Index", "System");
        }
    }
}