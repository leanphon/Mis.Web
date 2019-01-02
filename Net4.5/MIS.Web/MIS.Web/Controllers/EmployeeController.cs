using Apps.BLL;
using Apps.Model;
using MIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            QueryParam queryParam = new QueryParam { pager = pager };

            var extendParams = Request.Params["extendParams"];
            if (extendParams != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<FilterModel> filters = js.Deserialize<List<FilterModel>>(extendParams);
                Dictionary<string, FilterModel> filterSet = filters.ToDictionary(key => key.key, model => model);

                queryParam.filters = filterSet;
            }

            EmployeeManager manager = new EmployeeManager();
            OperateResult or = manager.GetByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Entry()
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

            SalaryInfo salary = new SalaryInfo();
            try
            {
                salary.postId = Convert.ToInt64(Request.Params["postId"]);
                salary.levelId = Convert.ToInt64(Request.Params["levelId"]);
                salary.performanceId = Convert.ToInt64(Request.Params["performanceId"]);
                salary.benefitId = Convert.ToInt64(Request.Params["benefitId"]);
            }
            catch (Exception)
            {
                return Json(new OperateResult
                {
                    content = "岗位、层级、绩效、效益,选择有误"
                }, JsonRequestBehavior.AllowGet);
            }

            model.salaryInfo = salary;

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
        public ActionResult ShowFormal()
        {
            return View();
        }
        public ActionResult ShowLeave()
        {
            return View();
        }

        public ActionResult Formal(int? id)
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
        public ActionResult Leave(int? id)
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



        public ActionResult UpdateToFormal(int? id, string state, DateTime formalDate)
        {
            if (id == null || state == null || formalDate == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "参数错误",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.Formal(id.Value, state, formalDate);

            return Json(or, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdateToLeave(int? id, string state, DateTime leaveDate)
        {
            if (id == null || state == null || leaveDate == null)
            {
                return Json(
                    new OperateResult
                    {
                        content = "参数错误",
                    },
                    JsonRequestBehavior.AllowGet
                );
            }

            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.Leave(id.Value, state, leaveDate);

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

            OperateResult or = manager.GetById(id.Value);

            if (or.data != null) // 未经设定，则初始化一个
            {
                Employee e = or.data as Employee;
                if (e.salaryInfo == null)
                {
                    e.salaryInfo = new SalaryInfo()
                    {
                        id = 0
                    };
                }

            }

            return View(or.data);

        }
        public ActionResult EditSalary(SalaryInfo model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(
            //        new OperateResult
            //        {
            //            content = Model.Utility.GetModelStateErrors(ModelState),
            //        },
            //        JsonRequestBehavior.AllowGet
            //    );
            //}
            try
            {
                long employeeId = Convert.ToInt64(Request.Params["employeeId"]);

                EmployeeManager manager = new EmployeeManager();
                OperateResult or = manager.UpdateSalary(employeeId, model);

                return Json(or, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new OperateResult()
                {
                    content = "访问错误"
                }, JsonRequestBehavior.AllowGet);
            }

            

        }

        public ActionResult ShowDepartment(int? id)
        {
            return View();

        }
        public ActionResult SelectDepartment(long id ,long departmentId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(
            //        new OperateResult
            //        {
            //            content = Model.Utility.GetModelStateErrors(ModelState),
            //        },
            //        JsonRequestBehavior.AllowGet
            //    );
            //}

            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.SelectDepartment(id, departmentId);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowCareer(long? id)
        {
            EmployeeManager manager = new EmployeeManager();

            OperateResult or = manager.GetById(id.Value);
            if (or.status == OperateStatus.Error || or.data == null)
            {
                return Content("访问错误");
            }

            return View(or.data);
        }
        public ActionResult GetCareer(long? id)
        {
            EmployeeManager manager = new EmployeeManager();
            OperateResult or = manager.GetCareerRecordsById(id.Value);
            if (or.status == OperateStatus.Success)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddCareerBatch(long id)
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
            List<EmployeeCareerRecord> lstData = js.Deserialize<List<EmployeeCareerRecord>>(data);

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

            EmployeeManager manager = new EmployeeManager();
            OperateResult or = manager.AddCareerRecordBatch(id, lstData);

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


            EmployeeManager manager = new EmployeeManager();
            OperateResult or = manager.ExportAll(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                string exportFileName = string.Concat("导出", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "员工列表",
                    FileName = exportFileName,
                    ExportData = (DataTable)or.data
                };

            }

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

            EmployeeManager manager = new EmployeeManager();
            OperateResult or = manager.ImportExcel(path);


            return Json(or, JsonRequestBehavior.AllowGet);
        }

    }
}
