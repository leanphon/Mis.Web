using Apps.BLL;
using Apps.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS.Web.Controllers
{
    public class DepartmentController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllEntities()
        {
            OperateResult or = DepartmentManager.GetAll();

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }


            return Json(or, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEntities(Pager pager)
        {

            OperateResult or = DepartmentManager.GetByPager(new QueryParam {pager=pager });
            
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
        public ActionResult CreateEntity(Department model)
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

            OperateResult or = DepartmentManager.Add(model);

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
            OperateResult or = DepartmentManager.GetById(id.Value);

            return View(or.data);

        }
        public ActionResult EditEntity(Department model)
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

            OperateResult or = DepartmentManager.Update(model);

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

            OperateResult or = DepartmentManager.Remove(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 编制
        /// </summary>
        /// <returns></returns>
        public ActionResult Strength()
        {
            return View();

        }


    }
}

