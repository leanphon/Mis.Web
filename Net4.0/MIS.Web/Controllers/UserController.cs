using Apps.BLL;
using Apps.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Linq;
using MIS.Web.Models;
using Apps.Model.Privilege;

namespace MIS.Web.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllEntities()
        {
            UserManager manager = new UserManager();
            OperateResult or = manager.GetAll();

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }


            return Json(or, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEntities(Pager pager)
        {
            UserManager manager = new UserManager();
            OperateResult or = manager.GetByPager(new QueryParam { pager = pager });

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateEntity(User model)
        {
            if (!ModelState.IsValid)
            {
                return Json(
                    new OperateResult
                    {
                        content = Model.Utility.GetModelStateErrors(ModelState),
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            UserManager manager = new UserManager();

            OperateResult or = manager.Add(model);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "访问错误",
                    },
                    JsonRequestBehavior.AllowGet
                );

            }
            UserManager manager = new UserManager();

            OperateResult or = manager.GetById(id.Value);

            return View(or.data);

        }
        public ActionResult EditEntity(User model)
        {
            if (!ModelState.IsValid)
            {
                return Json(
                    new OperateResult
                    {
                        content = Model.Utility.GetModelStateErrors(ModelState),
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            UserManager manager = new UserManager();

            OperateResult or = manager.Update(model);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "访问错误",
                    },
                    JsonRequestBehavior.AllowGet
                );

            }
            UserManager manager = new UserManager();

            OperateResult or = manager.Remove(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowResetPasswd(long? id)
        {
            if (id == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "访问错误",
                    },
                    JsonRequestBehavior.AllowGet
                );

            }

            ViewBag.id = id.Value;
            return View();

        }

        public ActionResult ResetPasswd(long id, string pwd)
        {
            if (pwd == null || string.IsNullOrEmpty(pwd))
            {
                return Json(
                    new OperateResult
                    {
                        content = "密码不能为空",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            UserManager manager = new UserManager();

            OperateResult or = manager.ResetPasswd(id, pwd);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Lock(long? id)
        {
            if (id == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "访问错误",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            UserManager manager = new UserManager();

            OperateResult or = manager.Lock(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
    }
}