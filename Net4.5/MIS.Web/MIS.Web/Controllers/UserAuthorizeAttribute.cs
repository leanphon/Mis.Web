using System.Web.Mvc;
using System.Linq;
using System.Web;
using Apps.Model.Privilege;
using Apps.BLL;

namespace MIS.Web.Controllers
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext .Session["currentUser"] as User;
            if (user == null)
            {
                httpContext.Response.Write("无权访问");
                httpContext.Response.End();

                return false;
            }

            if (user.name == "root")
            {
                return true;
            }
            else
            {
                var url = httpContext.Request.Path;
                if (url == null)
                {
                    return false;
                }

                using (SystemDB db = new SystemDB())
                {
                    var role = (from e in db.roleList.Include("rightList")
                        where e.id == user.roleId
                        select e).FirstOrDefault();
                    if (role == null)
                    {
                        httpContext.Response.Write("无权访问");
                        httpContext.Response.End();

                        return false;
                    }

                    var right = (from e in role.rightList
                        where e.url.Contains(url)
                        select e).FirstOrDefault();
                    if (right == null || right.authorize == "否")
                    {
                        httpContext.Response.Write("无权访问");
                        httpContext.Response.End();

                        return false;
                    }
                }
            }

            return true;
        }
    }

    //public class UserAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    //{
    //    public void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        var user = filterContext.HttpContext.Session["currentUser"] as User;


    //        if (user == null)
    //        {
    //            filterContext.RequestContext.HttpContext.Response.Write("无权访问");
    //            filterContext.RequestContext.HttpContext.Response.End();

    //            return;
    //        }

    //        if (user.name == "root")
    //        {
    //            return base.o;
    //        }
    //        else
    //        {
    //            var controller = filterContext.RouteData.Values["controller"];
    //            var action = filterContext.RouteData.Values["action"];
    //            string url = "/" + controller.ToString() + "/" + action.ToString();

    //            using (SystemDB db = new SystemDB())
    //            {
    //                var role = (from e in db.roleList.Include("rightList")
    //                    where e.id == user.roleId
    //                    select e).FirstOrDefault();
    //                if (role == null)
    //                {
    //                    filterContext.RequestContext.HttpContext.Response.Write("无权访问");
    //                    filterContext.RequestContext.HttpContext.Response.End();

    //                    return;
    //                }

    //                var right = (from e in role.rightList
    //                    where e.url.Contains(url) 
    //                    select e).FirstOrDefault();
    //                if (right == null || right.authorize == "否")
    //                {
    //                    filterContext.RequestContext.HttpContext.Response.Write("无权访问");
    //                    filterContext.RequestContext.HttpContext.Response.End();

    //                    return;
    //                }
    //            }
    //        }

    //    }
    //}
}