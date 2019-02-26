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

namespace MIS.Web.Controllers
{

    public class AnalyseController : BaseController
    {

        public ActionResult Index(string reportType)
        {
            ViewBag.reportType = reportType;

            return View();
        }

        public ActionResult EmployeeAge()
        {
            return View();
        }

        public ActionResult LoadEmployeeAge()
        {
            QueryParam queryParam = new QueryParam();

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            OperateResult or = EmployeeManager.AnalyseByAge(queryParam);

            if (or.status != OperateStatus.Success
                && or.content != null)
            {
                return Json(or, JsonRequestBehavior.AllowGet);
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EmployeeGender()
        {
            return View();
        }
        public ActionResult LoadEmployeeGender()
        {
            QueryParam queryParam = new QueryParam();

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            OperateResult or = EmployeeManager.AnalyseByGender(queryParam);

            if (or.status != OperateStatus.Success
                && or.content != null)
            {
                return Json(or, JsonRequestBehavior.AllowGet);
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //ViewBag.data = js.Serialize(or.data);

            return Json(or, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EmployeeWorkAge()
        {
            return View();
        }
        public ActionResult LoadEmployeeWorkAge()
        {
            QueryParam queryParam = new QueryParam();

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            OperateResult or = EmployeeManager.AnalyseByWorkAge(queryParam);

            if (or.status != OperateStatus.Success
                && or.content != null)
            {
                return Json(or, JsonRequestBehavior.AllowGet);
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //ViewBag.data = js.Serialize(or.data);

            return Json(or, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EmployeeSalary()
        {
            return View();
        }
        public ActionResult LoadEmployeeSalary()
        {
            QueryParam queryParam = new QueryParam();

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            OperateResult or = EmployeeManager.AnalyseBySalary(queryParam);

            if (or.status != OperateStatus.Success
                && or.content != null)
            {
                return Json(or, JsonRequestBehavior.AllowGet);
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //ViewBag.data = js.Serialize(or.data);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EmployeePost()
        {
            return View();
        }
        public ActionResult LoadEmployeePost()
        {
            QueryParam queryParam = new QueryParam();

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            OperateResult or = EmployeeManager.AnalyseByPost(queryParam);

            if (or.status != OperateStatus.Success
                && or.content != null)
            {
                return Json(or, JsonRequestBehavior.AllowGet);
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //ViewBag.data = js.Serialize(or.data);

            return Json(or, JsonRequestBehavior.AllowGet);

        }



        public ActionResult LeaveWarning()
        {
            return View();
        }
        public ActionResult LoadLeaveWarning()
        {
            QueryParam queryParam = new QueryParam();

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            OperateResult or = LeaveManager.LeaveWarningByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);

        }

    }
}

