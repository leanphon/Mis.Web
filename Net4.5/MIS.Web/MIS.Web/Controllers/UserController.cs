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
            OperateResult or = UserManager.GetAll();

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }


            return Json(or, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEntities(Pager pager)
        {
            OperateResult or = UserManager.GetByPager(new QueryParam { pager = pager });

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


            OperateResult or = UserManager.Add(model);

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

            OperateResult or = UserManager.GetById(id.Value);

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

            OperateResult or = UserManager.Update(model);

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

            OperateResult or = UserManager.Remove(id.Value);

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

            var user = Session["currentUser"] as User;
            if (user == null || user.role.type != RoleType.Admin && user.name != "root")
            {
                return Json(
                    new OperateResult
                    {
                        content = "无权限访问",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
            ViewBag.id = id.Value;
            return View();

        }

        public ActionResult ResetPasswd(long id, string passwd)
        {
            if (passwd == null || string.IsNullOrEmpty(passwd))
            {
                return Json(
                    new OperateResult
                    {
                        content = "密码不能为空",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            var user = Session["currentUser"] as User;
            if (user == null || user.role.type != RoleType.Admin && user.name != "root")
            {
                return Json(
                    new OperateResult
                    {
                        content = "无权限访问",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            OperateResult or = UserManager.ResetPasswd(id, passwd);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowModifyPasswd(long? id)
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

        public ActionResult ModifyPasswd(long id, string passwdOld, string passwd)
        {
            if (passwd == null || string.IsNullOrEmpty(passwdOld) || string.IsNullOrEmpty(passwd))
            {
                return Json(
                    new OperateResult
                    {
                        content = "密码不能为空",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            OperateResult or = UserManager.ModifyPasswd(id, passwdOld, passwd);

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

            OperateResult or = UserManager.Lock(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
    }
}