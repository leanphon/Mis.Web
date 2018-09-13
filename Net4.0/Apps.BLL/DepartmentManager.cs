using Apps.Model;
using System;
using System.Data.Entity;
using System.Linq;

namespace Apps.BLL
{
    public class DepartmentManager
    {
        public OperateResult Add(Department model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var match = from m in db.departmentList
                                where m.name.Equals(model.name)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该部门已经存在",
                        };
                    }

                    db.departmentList.Add(model);
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
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = db.departmentList.Find(id);
                    var elements = db.departmentList.ToList();

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该部门",
                        };
                    }
                    var query = from e in elements
                                where e.parentId == element.id
                                select e;
                    if (query.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该部门存在下级部门，不能删除",
                        };
                    }

                    db.departmentList.Remove(element);

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

        public OperateResult Update(Department model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = (from e in db.departmentList
                                    where e.id!=model.id && e.name==model.name
                                    select e
                                    ).ToList();
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的部门",
                        };
                    }
                        

                    var matchs = (from e in db.departmentList
                                    where e.id == model.parentId && e.parentId == model.id
                                    select e
                                    ).ToList();
                    if (matchs.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "该部门与其上级部门互为上下部门",
                        };
                    }
                    db.Entry(model).State = EntityState.Modified;

                    db.SaveChanges();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content ="更新成功"
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
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = (from m in db.departmentList.Include("parent")
                                 where id==m.id
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

        public OperateResult GetAll(QueryParam param= null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = (from e in db.departmentList
                                  select new
                                  {
                                      e.id,
                                      e.name,
                                      e.code,
                                      parentName = e.parent.name
                                  }
                                  ).ToList();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = elements,
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


        public OperateResult GetPage(QueryParam param=null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = from e in db.departmentList
                                   select new
                                   {
                                       e.id,
                                       e.name,
                                       e.code,
                                       _parentId = e.parentId
                                       //parentName = e.parent.name
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
