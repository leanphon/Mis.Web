
using Apps.Model;
using Apps.Model.Privilege;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL
{
    public class RoleManager
    {
        public static OperateResult  Add(Role model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var match = from m in db.roleList
                                where m.name.Equals(model.name)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的",
                        };
                    }

                    db.roleList.Add(model);
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "添加角色：" + model.name
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

                    var element = db.roleList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该记录",
                        };
                    }

                    db.roleList.Remove(element);

                    db.Entry(element).State = EntityState.Deleted;
                    db.SaveChanges();


                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除角色：" + element.name
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
                    db.roleList.RemoveRange(db.roleList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有角色"
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

        public static OperateResult  Update(Role model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.roleList
                                    where e.id != model.id && e.name == model.name
                                    select e
                                    ).ToList();
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的",
                        };
                    }


                    db.Entry(model).State = EntityState.Modified;

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "修改角色：" + model.name
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

                    var element = (from m in db.roleList
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

                    var elements = from e in db.roleList
                                   select e;

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
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.roleList
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

        public static OperateResult  GetRightById(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    // 根据给定的roleId和rightId，判断它是否已经被授权
                    Func<long, long, bool> AssignCheck = (roleId, rightId) =>
                    {

                        var roleRight = (from e in db.roleList.Include("rightList")
                                         where e.id == roleId
                                         select e.rightList
                                     ).FirstOrDefault();
                        var query = from e in roleRight
                                    where e.id == rightId
                                    select e;

                        return query.Count() > 0;
                    };

                    var modules = (from e in db.moduleList
                                   where e.onlyRoot != 1
                                   orderby e.showIndex ascending
                                   select new
                                   {
                                       id = e.id,
                                       name = e.name,
                                       _parentId = e.parentId,
                                       rightId = (long)-1,
                                       @checked = false,
                                   }).ToList();

                    var max = (from e in db.moduleList
                                   //where e.onlyRoot != 1
                               select e.id).Max();

                    var rights = (from e in db.rightList.Include("module").AsEnumerable()
                                  where e.module.onlyRoot != 1
                                  let check = AssignCheck(id, e.id)
                                  let rid = ++max
                                  select new
                                  {
                                      id = rid,
                                      name = e.name,
                                      _parentId = (long?)e.moduleId,
                                      rightId = e.id,
                                      @checked = check,
                                  }).ToList();

                    var elements = modules.Concat(rights);

                    var data = new
                    {
                        pages = 1,
                        total = elements.Count(),
                        rows = elements
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
        /// 先移除role所有的权限，然后重新加入
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static OperateResult  AssignRight(long roleId, List<long> idList)
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
                            content = "找不到角色"
                        };
                    }

                    role.rightList.Clear();

                    foreach (var rid in idList)
                    {
                        var model = db.rightList.Find(rid);
                        role.rightList.Add(model);
                    }

                    db.Entry(role).State = EntityState.Modified;

                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    //db.Configuration.ValidateOnSaveEnabled = true;

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "分配权限：" + role.name
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
    }
}
