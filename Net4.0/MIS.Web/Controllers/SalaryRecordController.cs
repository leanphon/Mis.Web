using Apps.BLL;
using Apps.Model;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Linq;
using MIS.Web.Models;
using System;
using System.Data;

namespace MIS.Web.Controllers
{

    public class SalaryRecordController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HistoryIndex()
        {
            return View();
        }
        public ActionResult GetAllEntities()
        {
            SalaryRecordManager manager = new SalaryRecordManager();
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
            QueryParam queryParam = new QueryParam { pager = pager };

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            SalaryRecordManager manager = new SalaryRecordManager();
            OperateResult or = manager.GetByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAssessmentByPager(Pager pager)
        {

            QueryParam queryParam = new QueryParam { pager = pager };

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }


            SalaryRecordManager manager = new SalaryRecordManager();
            OperateResult or = manager.GetAssessmentByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
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
            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.GetById(id.Value);

            return View(or.data);

        }

        public ActionResult SaveBatch()
        {
            string data = Request.Params["salaryRecord"];
            if (data == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<SalaryRecord> lstData = js.Deserialize<List<SalaryRecord>>(data);

            if (lstData.Count == 0)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            SalaryRecordManager manager = new SalaryRecordManager();
            OperateResult or = manager.AddBatch(lstData);

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSingle()
        {
            string month = Request.Params["month"];
            if (month == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "请输入考核月",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
            string data = Request.Params["salaryRecord"];
            if (data == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            SalaryRecord model = js.Deserialize<SalaryRecord>(data);

            if (model == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            SalaryRecordManager manager = new SalaryRecordManager();
            OperateResult or = manager.Add(model);

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

        public ActionResult ExportAll()
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


            SalaryRecordManager manager = new SalaryRecordManager();
            OperateResult or = manager.ExportAll(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                string exportFileName = string.Concat("导出", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "工资单记录",
                    FileName = exportFileName,
                    ExportData = (DataTable)or.data
                };

            }

            return Json(or, JsonRequestBehavior.AllowGet);

        }

    }
}
