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
            CompanyRegisterManager manager = new CompanyRegisterManager();
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

            CompanyRegisterManager manager = new CompanyRegisterManager();
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
        /// <summary>
        /// 1、先在default数据库内注册公司
        /// 2、新建数据库（即拷贝一份空数据库，以公司简称为文件名）
        /// 3、在新建数据库内插入公司记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public ActionResult CreateEntity(CompanyRegister model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(
        //            new OperateResult
        //            {
        //                content = Model.Utility.GetModelStateErrors(ModelState),
        //            },
        //            JsonRequestBehavior.AllowGet
        //        );
        //    }

        //    CompanyRegisterManager manager = new CompanyRegisterManager();

        //    OperateResult or = manager.AddRegister(model);
        //    if (or.status == OperateStatus.Success)
        //    {
        //        //从空数据库复制一份
        //        string dbSrc = Server.MapPath("/") + "App_Data/sqliteDefault.db";
        //        string dbDst = Server.MapPath("/") + "App_Data/sqlite" + model.code + ".db";

        //        System.IO.File.Copy(dbSrc, dbDst);

        //        Company c = new Company
        //        {
        //            name = model.name,
        //            code = model.code
        //        };

        //        CompanyManager m = new CompanyManager();
        //        or = m.Add(c);
        //    }

        //    return Json(or, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult CreateEntity(Company model)
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
            CompanyRegisterManager manager = new CompanyRegisterManager();

            OperateResult or = manager.GetRegisterById(id.Value);

            return View(or.data);

        }
        public ActionResult EditEntity(Company model)
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
            CompanyRegisterManager manager = new CompanyRegisterManager();

            OperateResult or = manager.RemoveRegister(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
    }
}
