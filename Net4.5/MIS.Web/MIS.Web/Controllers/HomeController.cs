using Apps.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Apps.Model;
using Apps.Model.Privilege;
using Apps.Model.Utility;

namespace MIS.Web.Controllers
{
    public class HomeController : Controller
    {
        [LoginAuthorize]
        public ActionResult Index()
        {

            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }
            Company c = Session["company"] as Company;
            User user = Session["currentUser"] as User;

            if (c != null)
            {
                ViewBag.companyName = c.name;
            }
            if (user != null)
            {
                ViewBag.username = user.name;
                ViewBag.userId = user.id;
            }

            FilterModel m = new FilterModel
            {
                action = "==",
                dataType = "bool",
                key = "roleId",
                value = Convert.ToString(user.roleId)
            };
            QueryParam queryParam = new QueryParam { filters = new Dictionary<string, FilterModel>() };
            queryParam.filters.Add(m.key, m);

            ModuleManager manager = new ModuleManager();
            OperateResult or = manager.GetModuleTree(queryParam);
            if (or.status == OperateStatus.Success)
            {
                return View(or.data);
            }

            return Content("访问错误");
        }

        /// <summary>
        /// 从请求中得到公司名称，然后查询公司信息，加载到登录页面
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public ActionResult Login(string company)
        {
            //if (company == "Root")
            //{
            //    //Session["CompanyName"] = "Default";
            //    Session["CompanyName"] = "Default";
            //}
            //else
            //{
            //    CompanyManager manager = new CompanyManager();
            //    OperateResult or = manager.GetByCode(company);
            //    if (or.status != OperateStatus.Success)
            //    {
            //        return Content("不存在的公司");
            //    }

            //    Company m = or.data as Company;

            //    ViewBag.companyName = m.name;

            //    Session["company"] = m;
            //}

            return View();
        }

        public ActionResult CheckLogin(User model)
        {
            if (!ModelState.IsValid)
            {
                return Json(
                    new OperateResult {
                        content = "请输入用户名和密码"
                    }, 
                JsonRequestBehavior.AllowGet);
            }

            UserManager manager = new UserManager();

            if (model.name == "root")
            {
                var or = manager.RootLogin(model);
                if (or.status == OperateStatus.Success
                    && or.data != null)
                {
                    Session["currentUser"] = or.data;
                    or.data = null;
                }
                return Json( or, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var or =  manager.Login(model);

                if (or.status == OperateStatus.Success
                    && or.data != null)
                {
                    Session["currentUser"] = or.data;

                    CompanyManager cm = new CompanyManager();
                    or = cm.GetFirst();
                    if (or.status == OperateStatus.Success
                        && or.data != null)
                    {
                        Session["company"] = or.data;
                    }

                    or.data = null;
                }

                return Json(or, "text/html", Encoding.UTF8, JsonRequestBehavior.AllowGet);

            }



        }


        private bool IsLogin()
        {
            var user = Session["currentUser"] as User;

            return user != null;
        }

        public ActionResult Logout()
        {
            Session["currentUser"] = null;
            Session.Remove("currentUser");

            Session["company"] = null;
            Session.Remove("company");



            OperateResult or = new OperateResult
            {
                status = OperateStatus.Success,
            };

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Quit()
        {
            Session["currentUser"] = null;
            Session.Remove("currentUser");

            Session["company"] = null;
            Session.Remove("company");



            OperateResult or = new OperateResult
            {
                status = OperateStatus.Success,
            };

            return Json(or, JsonRequestBehavior.AllowGet);
        }
    }
}