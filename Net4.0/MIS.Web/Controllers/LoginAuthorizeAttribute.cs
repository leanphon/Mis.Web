using Apps.Model.Privilege;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS.Web.Controllers
{
    public class LoginAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var account = filterContext.HttpContext.Session["currentUser"] as User;
            if (account == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}