using Apps.BLL.Utility;
using Apps.Model;
using Apps.Model.Privilege;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Apps.BLL
{
    public class FunctionRightManager
    {
        public static OperateResult  Add(FunctionRight model)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var match = from m in db.rightList
                                where m.name.Equals(model.name) || m.url.Equals(model.url)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "已经存在",
                        };
                    }

                    db.rightList.Add(model);
                    db.SaveChanges();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                    };
                }

            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

        }
        public static OperateResult  Remove(long id)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var element = db.rightList.Find(id);
                    var elements = db.rightList.ToList();

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在条数据",
                        };
                    }

                    db.rightList.Remove(element);

                    db.Entry(element).State = EntityState.Deleted;
                    db.SaveChanges();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content = "删除成功"
                    };

                }


            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }
        }


        public static OperateResult RemoveAll()
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    db.rightList.RemoveRange(db.rightList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有权限功能"
                    });

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content = "删除成功"
                    };

                }
            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

        }



        public static OperateResult  Update(FunctionRight model)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var elements = (from e in db.rightList
                                    where e.id != model.id && e.name == model.name
                                    select e
                                    ).ToList();
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的部门",
                        };
                    }


                    db.Entry(model).State = EntityState.Modified;

                    db.SaveChanges();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content = "更新成功"
                    };

                }

            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }
        }
        public static OperateResult  GetById(long id)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var element = (from m in db.rightList
                                   where id == m.id
                                   select m
                                ).FirstOrDefault();

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "访问错误",
                        };
                    }

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = element,
                    };

                }


            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }
        }

        public static OperateResult  GetAll(QueryParam param = null)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var elements = from e in db.rightList.Include("module")
                                   orderby e.moduleId
                                   select new
                                   {
                                       e.id,
                                       moduleName = e.module.name,
                                       e.name,
                                       e.url,
                                       e.icon,
                                       e.show,
                                       e.authorize
                                   };

                    #region 查询过滤
                    if (param != null && param.filters != null)
                    {

                        // 过滤显示
                        #region 过滤显示
                        if (param.filters.Keys.Contains("show"))
                        {
                            var p = param.filters["show"];
                            bool v = Convert.ToBoolean(p.value);
                            elements = elements.Where(t => t.show == v);
                        }
                        #endregion

                    }

                    #endregion


                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = elements.ToList(),
                    };

                }

            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

        }

        public static OperateResult  GetByPager(QueryParam param = null)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var elements = from e in db.rightList.Include("module")
                                   orderby e.module.showIndex
                                   select new
                                   {
                                       e.id,
                                       moduleName = e.module.name,
                                       e.name,
                                       e.url,
                                       e.icon,
                                       e.show,
                                       e.authorize
                                   };

                    #region 查询过滤
                    if (param != null && param.filters != null)
                    {

                        // 过滤显示
                        #region 过滤显示
                        if (param.filters.Keys.Contains("show"))
                        {
                            var p = param.filters["show"];
                            bool v = Convert.ToBoolean(p.value);
                            elements = elements.Where(t => t.show == v);
                        }
                        #endregion

                    }

                    #endregion


                    int total = elements.Count();
                    int pages = 0;
                    Pager pager = param.pager;
                    if (pager == null || pager.rows == 0)
                    {
                        pages = total > 0 ? 1 : 0;
                    }
                    else
                    {
                        pages = total / (pager.rows == 0 ? 20 : pager.rows);
                        pages = total % pager.rows == 0 ? pages : pages + 1;
                        if (pager.page <= 1)
                        {
                            elements = elements.Take(pager.rows);
                        }
                        else
                        {
                            elements = elements.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                        }
                    }

                    var data = new
                    {
                        pages,
                        total,
                        rows = elements.ToList()
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = data,
                    };

                }


            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }
        }


    }
}
