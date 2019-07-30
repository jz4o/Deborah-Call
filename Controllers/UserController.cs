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
    }
}