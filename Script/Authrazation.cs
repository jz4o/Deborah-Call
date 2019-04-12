using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Deborah.Models;

namespace Login.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session.GetString("login");
            //context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            if (session == "")
            {
                context.Result = new RedirectToActionResult("Login", "Top", null);
            }
        }
    }
}