using Apps.BLL;
using Apps.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MIS.Web.Controllers
{
    public class PredictController : BaseController
    {
        public ActionResult LeaveWarning()
        {
            return View();
        }

        public ActionResult GetLeaveWarningByPager(Pager pager)
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

            OperateResult or = LeaveManager.LeaveWarningByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowManualAnalyze()
        {
            return View();
        }

        [NoAsyncTimeout]
        public async Task<ActionResult> ManualAnalyze()
        {
            LeaveManager manager = new LeaveManager();

            var task = await manager.AnalyzeOneTimeAsync();

            return Json(task, JsonRequestBehavior.AllowGet);

        }


        public ActionResult EmployeeContract()
        {
            return View();
        }
        public ActionResult GetEmployeeContractByPager(Apps.Model.Pager pager)
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

            OperateResult or = EmployeeManager.GetEmployeeContractByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeBirthday()
        {
            return View();
        }
        public ActionResult GetEmployeeBirthdayByPager(Apps.Model.Pager pager)
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

            OperateResult or = EmployeeManager.GetEmployeeBirthdayByPager(queryParam);

            if (or.status == OperateStatus.Success
                && or.data != null)
            {
                return Json(or.data, JsonRequestBehavior.AllowGet);
            }

            return Json(or, JsonRequestBehavior.AllowGet);
        }

    }
}
