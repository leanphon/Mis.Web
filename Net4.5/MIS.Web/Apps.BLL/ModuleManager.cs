using Apps.Model;
using Apps.Model.Privilege;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Apps.BLL.Utility;
using Remotion.Data.Linq.Clauses;

namespace Apps.BLL
{
    public class ModuleManager
    {
        public OperateResult Add(Module model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var match = from m in db.moduleList
                                where m.name.Equals(model.name)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该模块已经存在",
                        };
                    }

                    db.moduleList.Add(model);
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
        public OperateResult Remove(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = db.moduleList.Find(id);
                    var elements = db.moduleList.ToList();

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该模块",
                        };
                    }
                    var query = from e in elements
                                where e.parentId == element.id
                                select e;
                    if (query.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该模块存在下级模块，不能删除",
                        };
                    }

                    db.moduleList.Remove(element);

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

        public OperateResult Update(Module model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.moduleList
                                    where e.id != model.id && e.name == model.name
                                    select e
                                    ).ToList();
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的模块",
                        };
                    }


                    var matchs = (from e in db.moduleList
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
        public OperateResult GetById(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.moduleList.Include("parent")
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

        public OperateResult GetAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.moduleList
                                    orderby e.showIndex ascending
                                    select new
                                    {
                                        e.id,
                                        e.name,
                                        _parentId = e.parentId,
                                        parentName = e.parent.name
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


        public OperateResult GetByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.moduleList
                                   orderby e.showIndex ascending
                                   select new
                                   {
                                       e.id,
                                       e.name,
                                       _parentId = e.parentId
                                   };

                    int total = elements.Count();
                    /*
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
                    */
                    var data = new
                    {
                        pages = 1,
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

        public OperateResult GetModuleTree(QueryParam param = null)
        {
            if (param == null || param.filters == null
                              || !param.filters.Keys.Contains("roleId"))
            {
                return new OperateResult
                {
                    content = "没有权限"
                };
            }

            var p = param.filters["roleId"];
            long roleId = Convert.ToInt64(p.value ?? "0");

            try
            {
                Dictionary<Module, IEnumerable<Module>> map = new Dictionary<Module, IEnumerable<Module>>();

                var or = GetModulesByRoleId(roleId);
                if (or.status != OperateStatus.Success)
                {
                    return or;
                }

                IEnumerable<Module> elements = or.data as IEnumerable<Module>;

                using (SystemDB db = new SystemDB())
                {
                    var parents = from e in elements
                                  where e.parentId == null
                                  select e;
                    foreach (var m in parents)
                    {
                        var suns = from s in elements
                                   where s.parentId == m.id
                                   select s;

                        map.Add(m, suns);
                    }

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = map,
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

        public OperateResult GetModulesByRoleId(long roleId)
        {
            if (roleId == -1) // root 用户
            {
                try
                {
                    using (SystemDB db = new SystemDB())
                    {
                        var data = (from e in db.moduleList
                                    orderby e.showIndex ascending
                                    select e)
                            .ToList();

                        return new OperateResult
                        {
                            data = data,
                            status = OperateStatus.Success
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
            else
            {
                try
                {
                    using (SystemDB db = new SystemDB())
                    {
                        var role = (from e in db.roleList.Include("rightList")
                                    where e.id == roleId
                                    select e).FirstOrDefault();
                        if (role == null)
                        {
                            return new OperateResult
                            {
                                content = "没有权限"
                            };
                        }

                        if (role.type == RoleType.Admin)
                        {
                            var data = (from e in db.moduleList
                                where e.onlyRoot != 1
                                orderby e.showIndex ascending
                                select e).ToList();

                            return new OperateResult
                            {
                                data = data,
                                status = OperateStatus.Success
                            };
                        }
                        else
                        {
                            var data = (from e in role.rightList
                                join m in db.moduleList on e.moduleId equals m.id
                                orderby m.showIndex ascending
                                select m).Distinct().ToList();

                            var parents = (from e in data
                                join m in db.moduleList on e.parentId equals m.id
                                select m).Distinct().ToList();

                            return new OperateResult
                            {
                                data = data.Concat(parents),
                                status = OperateStatus.Success
                            };
                        }
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
}
