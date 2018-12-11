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

            if (user.name == "root")
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
                    var role = (from e in db.roleList.Include("rightList")
                        where e.id == user.roleId
                        select e).FirstOrDefault();
                    if (role == null)
                    {
                        filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                        filterContext.RequestContext.HttpContext.Response.End();

                        return;
                    }

                    var right = (from e in role.rightList
                        where e.url == url
                        select e).FirstOrDefault();
                    if (right == null)
                    {
                        filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                        filterContext.RequestContext.HttpContext.Response.End();
                    }
                }
            }

        }
    }
}