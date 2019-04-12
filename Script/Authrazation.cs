using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Deborah.Models;
using Microsoft.AspNetCore.HttpsPolicy;

namespace Login.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session.GetString("login");
            Console.WriteLine(session);
            Console.WriteLine("こここここここここここ");
            if (session == "")
            {
                context.Result = new RedirectResult();
            }
            return;
        }
    }
}