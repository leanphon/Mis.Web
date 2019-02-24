
using Apps.Model;
using Apps.Model.Privilege;
using Apps.Model.Utility;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Apps.BLL
{
    public class LogManager
    {
        public static OperateResult Add(LogRecord model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    db.logRecordList.Add(model);
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
        public static OperateResult Remove(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

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
                    db.logRecordList.RemoveRange(db.logRecordList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有日志"
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

        public static OperateResult Update(LogRecord model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {


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
        public static OperateResult GetById(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.logRecordList
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

        public static OperateResult GetAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.logRecordList.Include("user")
                                    select new
                                    {
                                        e.id,
                                        e.time,
                                        e.content,
                                        e.type,
                                        e.user.name
                                    }
                                  ).ToList();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = elements,
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


        public static OperateResult GetByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.logRecordList.Include("user")
                                   orderby e.time descending 
                                   select new
                                   {
                                       e.id,
                                       e.time,
                                       e.content,
                                       e.type,
                                       e.user.name
                                   };

                    // 过滤时间
                    #region
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("timeBegin"))
                        {
                            var p = param.filters["timeBegin"];
                            var t = Convert.ToDateTime(p.value);
                            elements = elements.Where(m => m.time >= t);
                        }

                        if (param.filters.Keys.Contains("timeEnd"))
                        {
                            var p = param.filters["timeEnd"];
                            var t = Convert.ToDateTime(p.value);
                            elements = elements.Where(m => m.time <= t);
                        }
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

        /// <summary>
        /// 返回值中的data是DataTable
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public static OperateResult ExportAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.logRecordList.Include("user")
                                   select new
                                   {
                                       e.id,
                                       e.time,
                                       e.content,
                                       e.type,
                                       e.user.name
                                   };


                    // 过滤时间
                    #region
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("timeBegin"))
                        {
                            var p = param.filters["timeBegin"];
                            var t = Convert.ToDateTime(p.value);
                            elements = elements.Where(m => m.time >= t);
                        }
                        if (param.filters.Keys.Contains("timeEnd"))
                        {
                            var p = param.filters["timeEnd"];
                            var t = Convert.ToDateTime(p.value);
                            elements = elements.Where(m => m.time <= t);
                        }
                    }
                    #endregion

                    long rowIndex = 1;
                    var results = from e in elements.AsEnumerable()
                                  let index = rowIndex++
                                  select new LogRecordExport
                                  {
                                      index = index,
                                      time = e.time,
                                      content = e.content,
                                      type = e.type,
                                      username = e.name,
                                  };

                    DataTable dt = DataTableHelper.ToDataTable<LogRecordExport>(results.ToList());

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content = "导出成功",
                        data = dt,
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
