using Apps.BLL;
using Apps.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace MIS.Web.Controllers
{
    public class CompanyController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllEntities()
        {
            CompanyManager manager = new CompanyManager();
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

            CompanyManager manager = new CompanyManager();
            OperateResult or = manager.GetByPager(new QueryParam { pager = pager });

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
            CompanyManager manager = new CompanyManager();

            OperateResult or = manager.GetById(id.Value);

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

            if (Request.Files.Count > 0)
            {
                if (model.loginImg != null)
                {
                    HttpPostedFileBase loginFile = Request.Files["loginFile"];
                    if (loginFile != null && loginFile.FileName != "")
                    {

                        if (Path.GetExtension(loginFile.FileName) != ".jpg")
                        {
                            return Json(
                                new OperateResult
                                {
                                    content = "上传的文件不是jpg文件",
                                },
                                JsonRequestBehavior.AllowGet
                            );
                        }

                        model.loginImg = model.code + "-loginImg.jpg";
                        string file = Server.MapPath("/") + ("/Upload/") + model.loginImg;

                        loginFile.SaveAs(file);
                    }
                }
                if (model.logo != null)
                {
                    HttpPostedFileBase logoFile = Request.Files["logoFile"];
                    if (logoFile != null && logoFile.FileName!="")
                    {

                        if (Path.GetExtension(logoFile.FileName) != ".jpg")
                        {
                            return Json(
                                new OperateResult
                                {
                                    content = "上传的文件不是jpg文件",
                                },
                                JsonRequestBehavior.AllowGet
                            );
                        }

                        model.logo = model.code + "-logo.jpg";
                        string file = Server.MapPath("/") + ("/Upload/") + model.logo;

                        logoFile.SaveAs(file);
                    }
                }
            }

            
            CompanyManager manager = new CompanyManager();

            OperateResult or = manager.Update(model);

            return Json(or, JsonRequestBehavior.AllowGet);

        }

    }
}
