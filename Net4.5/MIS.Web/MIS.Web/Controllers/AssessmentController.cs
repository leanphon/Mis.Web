using Apps.BLL;
using Apps.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Linq;
using MIS.Web.Models;
using System.Web;
using System.IO;

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
        public ActionResult EditEntity()
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
            OperateResult or = manager.ExportAll(queryParam);

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

        public ActionResult UpdateStatus(long id, string status)
        {

            AssessmentManager manager = new AssessmentManager();

            OperateResult or = manager.UpdateStatus(id, status);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdateStatusBatch()
        {
            string data = Request.Params["assessmentData"];
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
                        content = "无考核数据",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.UpdateStatusBatch(lstData, status);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowImport()
        {
            return View();
        }

        public ActionResult ImportExcel()
        {
            if (Request.Files.Count == 0)
            {
                return Json(
                    new OperateResult
                    {
                        content = "请上传数据文件",
                    },
                    JsonRequestBehavior.AllowGet
                );

            }
            HttpPostedFileBase file = Request.Files["fileName"];
            if (file == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "请上传数据文件",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            switch (Path.GetExtension(file.FileName))
            {
                case ".xlsx":
                case ".xls":
                    break;
                default:
                    return Json(
                        new OperateResult
                        {
                            content = "上传的文件不是Excel文件",
                        },
                        JsonRequestBehavior.AllowGet
                    );
            }

            string target = Server.MapPath("/") + ("/Upload/");//取得目标文件夹的路径
            int pos = file.FileName.LastIndexOf('\\');
            string filename;
            if (pos >= 0)
            {
                filename = file.FileName.Substring(pos + 1);
            }
            else
            {
                filename = file.FileName;
            }

            string path = target + filename;//获取存储的目标地址
            file.SaveAs(path);

            AssessmentManager manager = new AssessmentManager();
            OperateResult or = manager.ImportExcel(path);


            return Json(or, JsonRequestBehavior.AllowGet);
        }


    }
}

