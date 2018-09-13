using Apps.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apps.Model;
using Apps.Model.Privilege;

namespace MIS.Web.Controllers
{
    public class HomeController : Controller
    {
        [LoginAuthorize]
        public ActionResult Index()
        {
            ViewBag.companyName = "广西南宁穆图装饰工程有限公司";

            if (!IsLogin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult Login()
        {
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

            OperateResult or = manager.Login(model);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                Session["currentUser"] = or.data;

            }

            return Json(or, JsonRequestBehavior.AllowGet);

        }


        private bool IsLogin()
        {
            var user = Session["currentUser"] as User;

            return user != null;
        }

    }
}