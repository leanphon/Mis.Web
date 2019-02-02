using Apps.BLL;
using Apps.Model;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Linq;
using MIS.Web.Models;
using System;
using System.Data;
using System.Web;

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
            OperateResult or = SalaryRecordManager.GetAll();

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

            OperateResult or = SalaryRecordManager.GetByPager(queryParam);

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

            OperateResult or = SalaryRecordManager.GetAssessmentByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditEntity()
        {
            string data = Request.Params["requestData"];
            if (data == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无考核数据",
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
                        content = "无考核数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

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

            OperateResult or = SalaryRecordManager.Update(model);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveBatch()
        {
            string data = Request.Params["requestData"];
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

            OperateResult or = SalaryRecordManager.AddBatch(lstData);

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
            string data = Request.Params["requestData"];
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

            OperateResult or = SalaryRecordManager.Add(model);

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshSalary()
        {
            
            string data = Request.Params["requestData"];
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

            OperateResult or = SalaryRecordManager.RefreshSalary(model);

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


            OperateResult or = SalaryRecordManager.ExportAll(queryParam);

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

        public ActionResult LockStatus(long id, string status)
        {

            OperateResult or = SalaryRecordManager.UpdateStatus(id, status);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LockStatusBatch()
        {
            string data = Request.Params["salaryRecord"];
            string status = Request.Params["status"];
            if (data == null || status == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "数据异常",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<long> lstData = js.Deserialize<List<long>>(data);

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

            OperateResult or = SalaryRecordManager.UpdateStatusBatch(lstData, status);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UnlockStatus(long id, string status)
        {

            OperateResult or = SalaryRecordManager.UpdateStatus(id, status);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UnlockStatusBatch()
        {
            string data = Request.Params["salaryRecord"];
            string status = Request.Params["status"];
            if (data == null || status == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "数据异常",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<long> lstData = js.Deserialize<List<long>>(data);

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

            OperateResult or = SalaryRecordManager.UpdateStatusBatch(lstData, status);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExportBill()
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

            OperateResult or = SalaryRecordManager.ExportBill(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                string exportFileName = string.Concat("工资条", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx");

                var excelHelper = new ExportExcelHelper()
                {
                    SheetName = "工资条",
                    FileName = exportFileName,
                    ExportData = (DataTable)or.data
                };

                excelHelper.GenerateExcelEx(this.HttpContext);

                return new EmptyResult();
            }

            return Json(or, JsonRequestBehavior.AllowGet);

        }

    }
}
