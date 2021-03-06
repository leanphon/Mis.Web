﻿using Apps.Model;
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
        public OperateResult Add(FunctionRight model)
        {
            using (DbContextRoot db = new DbContextRoot())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }
            }

        }
        public OperateResult Remove(long id)
        {
            using (DbContextRoot db = new DbContextRoot())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }

        }

        public OperateResult Update(FunctionRight model)
        {
            using (DbContextRoot db = new DbContextRoot())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }
            }

        }
        public OperateResult GetById(long id)
        {
            using (DbContextRoot db = new DbContextRoot())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }

        }

        public OperateResult GetAll(QueryParam param = null)
        {
            using (DbContextRoot db = new DbContextRoot())
            {
                try
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
                                   };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = elements.ToList(),
                    };

                }
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }
        }

        public OperateResult GetByPager(QueryParam param = null)
        {
            using (DbContextRoot db = new DbContextRoot())
            {
                try
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
                                       e.authorize
                                   };

                    int total = elements.Count();
                    int pages = 0;
                    Pager pager = param.pager;
                    if (pager == null || pager.rows == 0)
                    {
                        pages = total > 0 ? 1 : 0;
                    }
                    else
                    {
                        pages = total / (pager.rows == 0 ? 10 : pager.rows);
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }
        }


    }
}
