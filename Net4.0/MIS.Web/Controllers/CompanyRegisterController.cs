using Apps.BLL;
using Apps.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS.Web.Controllers
{
    public class CompanyRegisterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllEntities()
        {
            CompanyManager manager = new CompanyManager();
            OperateResult or = manager.GetRegisterAll();

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }


            return Json(or, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEntities(Pager pager)
        {

            CompanyManager manager = new CompanyManager();
            OperateResult or = manager.GetRegisterByPager(new QueryParam { pager = pager });

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
        public ActionResult CreateEntity(CompanyRegister model)
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

            CompanyManager manager = new CompanyManager();

            OperateResult or = manager.AddRegister(model);
            if (or.status == OperateStatus.Success)
            {
                //从空数据库复制一份
                string dbSrc = Server.MapPath("/") + "App_Data/sqliteEmpty.db";
                string dbDst = Server.MapPath("/") + "App_Data/sqlite" + model.code + ".db";

                System.IO.File.Copy(dbSrc, dbDst);
            }

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
            CompanyManager manager = new CompanyManager();

            OperateResult or = manager.GetRegisterById(id.Value);

            return View(or.data);

        }
        public ActionResult EditEntity(CompanyRegister model)
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

            CompanyManager manager = new CompanyManager();

            OperateResult or = manager.UpdateRegister(model);

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
            CompanyManager manager = new CompanyManager();

            OperateResult or = manager.RemoveRegister(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
    }
}
