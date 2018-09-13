using Apps.BLL;
using Apps.Model;
using MIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MIS.Web.Controllers
{
    public class EmployeeController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllEntities()
        {
            EmployeeManager manager = new EmployeeManager();
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

            EmployeeManager manager = new EmployeeManager();
            OperateResult or = manager.GetPage(new QueryParam { pager = pager });

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
        public ActionResult CreateEntity(Employee model)
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

            EmployeeManager manager = new EmployeeManager();

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
            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.GetById(id.Value);

            return View(or.data);

        }
        public ActionResult EditEntity(Employee model)
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

            EmployeeManager manager = new EmployeeManager();

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
            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.Remove(id.Value);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowSalary(int? id)
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
            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.GetSalaryInfoById(id.Value);

            if (or.data == null) // 未经设定，则初始化一个
            {
                OperateResult orEmployee = manager.GetById(id.Value);
                if(orEmployee.data != null)
                {
                    or.data = new SalaryInfo
                    {
                        id = 0,
                        employee = orEmployee.data as Employee,
                        employeeId = id.Value
                    };
                }

            }

            return View(or.data);

        }
        public ActionResult EditSalary(SalaryInfo model)
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

            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.UpdateSalary(model);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

    }
}
