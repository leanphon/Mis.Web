﻿using Apps.BLL;
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

    public class AssessmentController : BaseController
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
            AssessmentManager manager = new AssessmentManager();
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

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.GetByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeesByPager(Pager pager)
        {
            QueryParam queryParam = new QueryParam { pager = pager};

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize < List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }


            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.GetEmployeesByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddBatch()
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
            string data = Request.Params["assessmentData"];
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
            List<AssessmentInfo> lstData = js.Deserialize<List<AssessmentInfo>>(data);

            if (lstData.Count == 0)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无考核数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.AddBatch(lstData, month);

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSingle()
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
            string data = Request.Params["assessmentData"];
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
            AssessmentInfo model = js.Deserialize<AssessmentInfo>(data);

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

            model.month = month;

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.Add(model);

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBatch()
        {
            string data = Request.Params["assessmentData"];
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
            List<AssessmentInfo> lstData = js.Deserialize<List<AssessmentInfo>>(data);

            if (lstData.Count == 0)
            {
                return Json(
                    new OperateResult
                    {
                        content = "无考核数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.SaveBatch(lstData);

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSingle()
        {
            string data = Request.Params["assessmentData"];
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
            AssessmentInfo model = js.Deserialize<AssessmentInfo>(data);

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

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.Update(model);

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


            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.ExportAll (queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                string exportFileName = string.Concat("导出", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "考核记录",
                    FileName = exportFileName,
                    ExportData = (DataTable)or.data
                };

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


    }
}

