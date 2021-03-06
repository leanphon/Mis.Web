﻿using Apps.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.BLL.Utility;

namespace Apps.BLL
{
    public class LevelManager
    {
        public static OperateResult  Add(LevelInfo model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var match = from m in db.levelInfoList
                                where m.name.Equals(model.name)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该岗位名称已经存在",
                        };
                    }

                    db.levelInfoList.Add(model);
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "添加层级：" + model.name
                    });


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
                using (SystemDB db = new SystemDB())
                {

                    var element = db.levelInfoList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该岗位",
                        };
                    }

                    db.levelInfoList.Remove(element);

                    db.Entry(element).State = EntityState.Deleted;
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除层级：" + element.name
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

        public static OperateResult RemoveAll()
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    db.levelInfoList.RemoveRange(db.levelInfoList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有层级信息"
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

        public static OperateResult  Update(LevelInfo model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.levelInfoList
                                   where e.id != model.id && e.name == model.name
                                   select e;
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的岗位",
                        };
                    }


                    db.Entry(model).State = EntityState.Modified;

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "修改层级：" + model.name
                    });

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
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.levelInfoList
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
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.levelInfoList
                                    select e
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


        public static OperateResult  GetByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.levelInfoList
                                   orderby e.id

                                   select e;

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
                            elements = elements.Take(pager.rows).OrderBy(m => m.id); ;
                        }
                        else
                        {
                            elements = elements.Skip((pager.page - 1) * pager.rows).Take(pager.rows).OrderBy(m => m.id); ;
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
