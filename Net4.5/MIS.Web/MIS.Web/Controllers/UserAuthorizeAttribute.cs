using System.Web.Mvc;
using System.Linq;
using Apps.Model.Privilege;
using Apps.BLL;

namespace MIS.Web.Controllers
{
    public class UserAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.Session["currentUser"] as User;


            if (user == null)
            {
                filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                filterContext.RequestContext.HttpContext.Response.End();

                return;
            }
            else if (user.name == "root")
            {
                return;
            }
            else
            {
                var controller = filterContext.RouteData.Values["controller"];
                var action = filterContext.RouteData.Values["action"];
                string url = "/" + controller.ToString() + "/" + action.ToString();

                using (SystemDB db = new SystemDB())
                {
                    var elements = from e in db.roleRightsList
                                   join r in db.rightList on e.rightId equals r.id
                                   where e.roleId == user.roleId
                                   select e;
                    if (elements.Count() <= 0)
                    {
                        filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                        filterContext.RequestContext.HttpContext.Response.End();
                    }
                }
            }

        }
    }
}