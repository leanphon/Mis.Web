
using Apps.Model;
using Apps.Model.Utility;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel.Extensions;

namespace Apps.BLL
{

    public class EmployeeManager
    {
        public static OperateResult  Add(Employee model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var match = from m in db.employeeList
                                where m.number.Equals(model.number)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该工号已经存在",
                        };
                    }

                    if (model.salaryInfo != null)
                    {
                        db.salaryInfoList.Add(model.salaryInfo);
                        db.SaveChanges();

                        model.salaryInfoId = model.salaryInfo.id;
                    }
                    
                    db.employeeList.Add(model);

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "添加员工：" + model.number
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

                    var element = db.employeeList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该员工",
                        };
                    }

                    db.employeeList.Remove(element);

                    db.Entry(element).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除员工：" + element.number
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
                    db.employeeCareerList.RemoveRange(db.employeeCareerList.ToList());

                    db.employeeList.RemoveRange(db.employeeList.ToList());

                    db.salaryInfoList.RemoveRange(db.salaryInfoList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有员工"
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


        public static OperateResult  Update(Employee model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from e in db.employeeList
                                    where e.id != model.id && e.number == model.number
                                    select e
                                    ).FirstOrDefault();
                    if (element != null)
                    {
                        return new OperateResult
                        {
                            content = "已经存在该工号",
                        };
                    }

                    var employee = (from e in db.employeeList
                            where e.id == model.id
                            select e
                        ).AsNoTracking().FirstOrDefault();
                    if (employee == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在的员工",
                        };
                    }

                    model.postId = employee.postId;
                    model.salaryInfoId = employee.salaryInfoId;

                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "修改员工：" + model.number
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

        public static OperateResult  GetSalaryInfoById(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.salaryInfoList.Include("levelInfo").Include("employee")
                                   .Include("performanceInfo").Include("benefitInfo")
                                   join e in db.employeeList on m.id equals e.salaryInfoId
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

        public static OperateResult  UpdatePost(long employeeId, long postId)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var employee = (from e in db.employeeList
                        where e.id == employeeId
                        select e).AsNoTracking().FirstOrDefault();
                    if (employee == null)
                    {
                        return new OperateResult
                        {
                            content = "访问错误",
                        };
                    }

                    employee.postId = postId;
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "修改员工岗位：" + employee.number
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
        public static OperateResult  UpdateSalary(long employeeId, SalaryInfo model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var employee = (from e in db.employeeList
                                    where e.id == employeeId
                                    select e).AsNoTracking().FirstOrDefault();
                    if (employee == null)
                    {
                        return new OperateResult
                        {
                            content = "访问错误",
                        };
                    }

                    var elements = (from e in db.salaryInfoList
                                    where e.id == model.id
                                    select e
                                    ).AsNoTracking().ToList();

                    if (elements.Count() == 1)
                    {
                        if (model.levelId != null || model.performanceId != null
                                                  || model.benefitId != null)
                        {
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            LogManager.Add(new LogRecord
                            {
                                userId = SessionHelper.GetUserId(),
                                time = DateTime.Now,
                                type = "Info",
                                content = "修改员工薪酬：" + employee.number
                            });
                        }
                    }
                    else
                    {

                        db.salaryInfoList.Add(model);
                        db.SaveChanges();

                        employee.salaryInfo = model;
                        employee.salaryInfoId = model.id;
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();

                        LogManager.Add(new LogRecord
                        {
                            userId = SessionHelper.GetUserId(),
                            time = DateTime.Now,
                            type = "Info",
                            content = "新建员工薪酬：" + employee.number
                        });
                    }


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

        public static OperateResult  Formal(long id, string state, DateTime time)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var m = (from e in db.employeeList
                             where e.id == id
                             select e
                             ).AsNoTracking().FirstOrDefault();
                    if (m == null)
                    {
                        return new OperateResult
                        {
                            content = "找不到该员工"
                        };

                    }
                    m.state = state;
                    m.formalDate = time;

                    db.Entry(m).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "员工转正：" + m.number
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

        public static OperateResult  Leave(long id, string state, DateTime time)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var m = (from e in db.employeeList
                             where e.id == id
                             select e
                             ).AsNoTracking().FirstOrDefault();
                    if (m == null)
                    {
                        return new OperateResult
                        {
                            content = "找不到该员工"
                        };

                    }
                    m.state = state;
                    m.leaveDate = time;

                    db.Entry(m).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "员工离职：" + m.number
                    });

                    LeaveManager.Leave(m);


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

        public static OperateResult  SelectDepartment(long id, long departmentId)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.employeeList
                            where id == m.id
                            select m
                        ).AsNoTracking().FirstOrDefault();

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "访问错误",
                        };
                    }

                    element.departmentId = departmentId;

                    db.Entry(element).State = EntityState.Modified;
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "员工调部门：" + element.number
                    });

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content = "修改成功"
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

                    var element = (from m in db.employeeList.Include("department").Include("salaryInfo")
                                .Include("postInfo").Include("salaryInfo.levelInfo")
                                .Include("salaryInfo.performanceInfo").Include("salaryInfo.benefitInfo")
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

                    var elements = (from e in db.employeeList
                                    select new
                                    {
                                        e.id,
                                        e.name,
                                        e.number,
                                        e.sex,
                                        e.phone,
                                        e.idCard,
                                        e.birthday,
                                        e.bankCard,
                                        e.state,
                                        e.entryDate,
                                        e.formalDate,
                                        e.leaveDate,
                                        e.emergencyContact,
                                        e.emergencyPhone,
                                        e.departmentId,
                                        departmentName = e.department.name,
                                        e.nation,
                                        e.nativePlace,
                                        e.residence,
                                        e.address,
                                        e.political,
                                        e.marriage,
                                        e.education,
                                        e.experience,
                                        e.source,
                                        e.contractSerial,
                                        e.contractBegin,
                                        e.contractEnd,
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

        public static OperateResult  GetByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList.Include("department").Include("postInfo")
                                   orderby e.number
                                   select new
                                   {
                                       e.id,
                                       e.name,
                                       e.number,
                                       e.departmentId,
                                       departmentName = e.department.name,
                                       postName = e.postInfo.name,
                                       e.sex,
                                       e.phone,
                                       e.idCard,
                                       e.birthday,
                                       e.bankCard,
                                       e.state,
                                       e.entryDate,
                                       e.formalDate,
                                       e.leaveDate,
                                       e.emergencyContact,
                                       e.emergencyPhone,
                                       e.nation,
                                       e.nativePlace,
                                       e.residence,
                                       e.address,
                                       e.political,
                                       e.marriage,
                                       e.education,
                                       e.experience,
                                       e.source,
                                       e.contractSerial,
                                       e.contractBegin,
                                       e.contractEnd,
                                       e.isSocialSecurity,
                                       e.isPension,
                                       e.isUrbanRuralMedical,
                                   };

                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                    }
                    #endregion

                    // 模糊过滤名字
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.name.Contains(p.value));
                        }
                    }
                    #endregion

                    // 过滤状态
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
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

        public static OperateResult  GetEmployeeContractByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    Func<DateTime?, bool> isNear = dt =>
                    {
                        if (dt == null)
                        {
                            return false;
                        }
                        DateTime now = DateTime.Now;
                        now = now.AddDays(30);
                        return now > dt;
                    };
                    var elements = from e in db.employeeList.Include("postInfo").AsEnumerable()
                                   where isNear(e.contractEnd)
                                   orderby e.number
                                   select new
                                   {
                                       e.id,
                                       e.name,
                                       e.number,
                                       e.departmentId,
                                       departmentName = e.department.name,
                                       postName = e.postInfo==null ? "" : e.postInfo.name,
                                       e.sex,
                                       e.phone,
                                       e.idCard,
                                       e.birthday,
                                       e.bankCard,
                                       e.state,
                                       e.entryDate,
                                       e.formalDate,
                                       e.leaveDate,
                                       e.contractSerial,
                                       e.contractBegin,
                                       e.contractEnd,
                                   };

                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                    where e.parentId == id
                                    select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                where e.id == departmentId
                                select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }

                        // 模糊过滤名字
                        #region
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.name.Contains(p.value));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
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

        public static OperateResult  GetEmployeeBirthdayByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    Func<DateTime?, bool> isNear = dt =>
                    {
                        DateTime t = dt.Value;
                        DateTime now = DateTime.Now;
                        DateTime yearBirth = new DateTime(now.Year, t.Month, t.Day);

                        now = now.AddDays(30);
                        return now > yearBirth;
                    };

                    var elements = from e in db.employeeList.AsEnumerable()
                                   where isNear(e.birthday)
                                   orderby e.number
                                   select new
                                   {
                                       e.id,
                                       e.name,
                                       e.number,
                                       e.departmentId,
                                       departmentName = e.department.name,
                                       //postName = e.postInfo == null ? "" : e.postInfo.name,
                                       e.sex,
                                       e.phone,
                                       e.idCard,
                                       e.birthday,
                                       e.bankCard,
                                       e.state,
                                       e.entryDate,
                                       e.formalDate,
                                       e.leaveDate,
                                       e.contractSerial,
                                       e.contractBegin,
                                       e.contractEnd,
                                   };

                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                    where e.parentId == id
                                    select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                where e.id == departmentId
                                select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                        // 模糊过滤名字
                        #region
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.name.Contains(p.value));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
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

        public static OperateResult  AddCareerRecordBatch(long employeeId, List<EmployeeCareerRecord> lstData)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var employee = (from e in db.employeeList
                                    where e.id == employeeId
                                    select e).FirstOrDefault();
                    if (employee == null)
                    {
                        return new OperateResult
                        {
                            content = "访问错误",
                        };
                    }

                    foreach (var model in lstData)
                    {
                        model.status = "audit";
                        db.employeeCareerList.Add(model);

                        LogManager.Add(new LogRecord
                        {
                            userId = SessionHelper.GetUserId(),
                            time = DateTime.Now,
                            type = "Info",
                            content = "添加员工事纪：" + employee.number + "," + model.description
                        });

                    }

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
        public static OperateResult  RemoveCareerRecord(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = db.employeeCareerList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在",
                        };
                    }

                    db.employeeCareerList.Remove(element);

                    db.Entry(element).State = System.Data.Entity.EntityState.Deleted;
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

        public static OperateResult  UpdateCareerRecord(EmployeeCareerRecord model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {


                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;

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


        public static OperateResult  GetCareerRecordById(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.employeeCareerList
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
        public static OperateResult  GetCareerRecordsById(long employeeId)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeCareerList
                                   where employeeId == e.employeeId
                                   orderby e.type
                                   select new
                                   {
                                       e.id,
                                       e.type,
                                       e.time,
                                       e.description
                                   };

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

        public static OperateResult  GetAllCareerRecords(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeCareerList.Include("employee")
                                   orderby e.time descending
                                   select new
                                   {
                                       e.id,
                                       e.type,
                                       e.time,
                                       e.description,
                                       e.employee.name,
                                       e.employee.state,
                                       e.employee.departmentId
                                   };


                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤

                        #region

                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }

                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }

                        #endregion

                        // 模糊过滤名字

                        #region

                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.name.Contains(p.value));
                        }

                        #endregion

                        // 过滤状态

                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
                        }
                        #endregion
                    }

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


        public static OperateResult  GetAllCareerRecordByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeCareerList.Include("employee")
                                   orderby e.time descending
                                   select new
                                   {
                                       e.id,
                                       e.type,
                                       e.time,
                                       e.description,
                                       employeeName = e.employee.name,
                                       employeeNumber = e.employee.number,
                                       employeeState = e.employee.state,
                                       e.employee.departmentId,
                                       departmentName = e.employee.department.name
                                   };


                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤

                        #region

                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }

                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }

                        #endregion

                        // 模糊过滤名字

                        #region

                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.employeeName.Contains(p.value));
                        }

                        #endregion

                        // 过滤状态

                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.employeeState));
                        }
                        #endregion
                    }

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
        public static OperateResult  ExportAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList.Include("salaryInfo").Include("postInfo")
                                   select new
                                   {
                                       e.id,
                                       e.name,
                                       e.number,
                                       e.departmentId,
                                       departmentName = e.department.name,
                                       postName = e.postInfo.name,
                                       e.sex,
                                       e.phone,
                                       e.idCard,
                                       e.birthday,
                                       e.bankCard,
                                       e.state,
                                       e.entryDate,
                                       e.formalDate,
                                       e.leaveDate,
                                       e.emergencyContact,
                                       e.emergencyPhone,
                                       e.nation,
                                       e.nativePlace,
                                       e.residence,
                                       e.address,
                                       e.political,
                                       e.marriage,
                                       e.education,
                                       e.experience,
                                       e.source,
                                       e.contractSerial,
                                       e.contractBegin,
                                       e.contractEnd,
                                       e.isSocialSecurity,
                                       e.isPension,
                                       e.isUrbanRuralMedical,
                                   };

                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                    }
                    #endregion


                    // 模糊过滤名字
                    #region
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.name.Contains(p.value));
                        }
                    }
                    #endregion

                    long rowIndex = 1;
                    var results = from e in elements.AsEnumerable()
                                  let index = rowIndex++
                                  select new EmployeeExport
                                  {
                                      index = index,
                                      number = e.number,
                                      name = e.name,
                                      departmentName = e.departmentName,
                                      postName = e.postName,
                                      sex = e.sex,
                                      phone = e.phone,
                                      idCard = e.idCard,
                                      birthday = e.birthday,
                                      bankCard = e.bankCard,
                                      state = e.state,
                                      entryDate = e.formalDate,
                                      formalDate = e.formalDate,
                                      leaveDate = e.leaveDate,
                                      emergencyContact = e.emergencyContact,
                                      emergencyPhone = e.emergencyPhone,

                                      nation = e.nation,
                                      nativePlace = e.nativePlace,
                                      residence = e.residence,
                                      address = e.address,
                                      political = e.political,
                                      marriage = e.marriage,
                                      education = e.education,
                                      experience = e.experience,
                                      source = e.source,
                                      contractSerial = e.contractSerial,
                                      contractBegin = e.contractBegin,
                                      contractEnd = e.contractEnd,
                                      isSocialSecurity = e.isSocialSecurity,
                                      isPension = e.isPension,
                                      isUrbanRuralMedical = e.isUrbanRuralMedical,
                                  };


                    DataTable dt = DataTableHelper.ToDataTable<EmployeeExport>(results.ToList());

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "导出员工信息："
                    });

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

        public static OperateResult  ImportExcel(string fileName)
        {
            List<EmployeeExport> query = new List<EmployeeExport>();
            try
            {
                var excelFile = new ExcelQueryFactory(fileName);
                var props = typeof(EmployeeExport).GetProperties();
                foreach (var p in props)
                {
                    var colName = DataTableHelper.GetColumnDisplay(p);
                    excelFile.AddMapping(p.Name, colName);
                }

                var tsheet = excelFile.Worksheet<EmployeeExport>(0);
                var t = excelFile.Worksheet(0);

                query = tsheet.ToList();
            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

            SystemDB db = new SystemDB();
            bool fail = false;
            OperateResult result = new OperateResult();

            foreach (var e in query)
            {
                try
                {
                    // 查询是否已经存在该员工信息
                    var es = (from d in db.employeeList
                        where d.number == e.number || d.idCard==e.idCard
                              || d.phone == e.phone
                        select d).FirstOrDefault();

                    if (es != null)
                    {
                        fail = true;
                        result.content += "工号(" + e.number + ")，已存在同工号或电话或身份证员工;";
                        continue;
                    }

                    var model = new Employee
                    {
                        number = e.number,
                        name = e.name,
                        sex = e.sex,
                        phone = e.phone,
                        idCard = e.idCard,
                        birthday = e.birthday,
                        bankCard = e.bankCard,
                        state = e.state,
                        entryDate = e.formalDate,
                        formalDate = e.formalDate,
                        leaveDate = e.leaveDate,
                        emergencyContact = e.emergencyContact,
                        emergencyPhone = e.emergencyPhone,

                        nation = e.nation,
                        nativePlace = e.nativePlace,
                        residence = e.residence,
                        address = e.address,
                        political = e.political,
                        marriage = e.marriage,
                        education = e.education,
                        experience = e.experience,
                        source = e.source,
                        contractSerial = e.contractSerial,
                        contractBegin = e.contractBegin,
                        contractEnd = e.contractEnd,
                        isSocialSecurity = e.isSocialSecurity,
                        isPension = e.isPension,
                        isUrbanRuralMedical = e.isUrbanRuralMedical,
                    };

                    //查询部门是否已经存在
                    var depart = (from d in db.departmentList
                        where d.name == e.departmentName
                        select d).FirstOrDefault();

                    if (depart == null)
                    {
                        fail = true;
                        result.content += "工号(" + e.number + ")，部门不存在;";
                        continue;
                    }

                    model.departmentId = depart.id;

                    //查询岗位是否已经存在
                    if (!string.IsNullOrWhiteSpace(e.postName))
                    {
                        var post = (from d in db.postInfoList
                            where d.name == e.postName
                            select d).FirstOrDefault();

                        if (post == null)
                        {
                            fail = true;
                            result.content += "工号(" + e.number + ")，岗位不存在;";
                            continue;
                        }

                        model.postId = post.id;
                    }

                    db.employeeList.Add(model);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    result.content += "工号(" + e.number + ")数据保存失败, error（"
                                      + Model.Utility.Utility.GetExceptionMsg(ex) + "） ;";
                }
            } // end for


            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            if (!fail)
            {
                result.status = OperateStatus.Success;
                result.content = "批量数据保存成功";
            }
            return result;
        }



        /// <summary>
        /// 返回值中的data是DataTable
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public static OperateResult  ExportCareerAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var elements = from e in db.employeeCareerList.Include("employee")
                        orderby e.time descending
                        select new
                        {
                            e.id,
                            e.type,
                            e.time,
                            e.description,
                            employeeName = e.employee.name,
                            employeeNumber = e.employee.number,
                            employeeState = e.employee.state,
                            e.employee.departmentId,
                            departmentName = e.employee.department.name
                        };

                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤

                        #region

                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }

                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }

                        #endregion

                        // 模糊过滤名字

                        #region

                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.employeeName.Contains(p.value));
                        }

                        #endregion

                        // 过滤状态

                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.employeeState));
                        }
                        #endregion
                    }

                    long rowIndex = 1;
                    var results = from e in elements.AsEnumerable()
                                  let index = rowIndex++
                                  select new EmployeeCareerRecordExport
                                  {
                                      index = index,
                                      employeeName = e.employeeName,
                                      employeeNumber = e.employeeNumber,
                                      departmentName = e.departmentName,
                                      type = e.type,
                                      time = e.time,
                                      description = e.description
                                  };


                    DataTable dt = DataTableHelper.ToDataTable<EmployeeCareerRecordExport>(results.ToList());

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "导出员工信息："
                    });

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



        public static OperateResult  AnalyseByAge(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var now = DateTime.Now;
                    var elements = from e in db.employeeList.AsEnumerable()
                                   select new
                                   {
                                       e.id,
                                       years = Model.Utility.Utility.CalYears(e.birthday, now),
                                       e.departmentId,
                                       e.state
                                   };

                    // 过滤
                    #region
                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤
                        #region
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
                        }
                        #endregion

                    }
                    #endregion

                    var results = elements.GroupBy(e => e.years)
                        .OrderBy(e => e.Key)
                        .Select(e => new { years = e.Key, count = e.Count() }).ToList();

                    var category = new List<object>();
                    var data = new List<object>();

                    foreach (var e in results)
                    {
                        category.Add(e.years);
                        data.Add(e.count);
                    }
                    var series = new List<object>();
                    series.Add(new { name = "员工年龄", data });
                    var legend = new List<object>();
                    legend.Add("员工年龄");

                    var resultData = new
                    {
                        title = "年龄分析",
                        category,
                        legend = legend,
                        series = series,
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = resultData,
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


        public static OperateResult  AnalyseByGender(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList.AsEnumerable()
                                   select new
                                   {
                                       e.id,
                                       e.sex,
                                       e.departmentId,
                                       e.state
                                   };

                    // 过滤
                    #region
                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤
                        #region
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
                        }
                        #endregion

                    }
                    #endregion


                    var results = elements.GroupBy(e => e.sex)
                        .Select(e => new { e.Key, count = e.Count() });

                    var category = new List<object>();
                    var data = new List<object>();

                    foreach (var e in results)
                    {
                        category.Add(e.Key);
                        data.Add(new { value = e.count, name = e.Key });
                    }
                    var series = new List<object>();
                    series.Add(new { name = "员工性别", data });
                    var legend = new List<object>();
                    legend.Add("员工性别");

                    var resultData = new
                    {
                        title = "性别分析",
                        category,
                        legend = legend,
                        series = series,
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = resultData,
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
        public static OperateResult  AnalyseByWorkAge(QueryParam param = null)
        {
            try
            {

                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList.AsEnumerable()
                                   let workAge = Model.Utility.Utility.CalYears(e.entryDate != null ? e.entryDate.Value : DateTime.Now, DateTime.Now)
                                   orderby workAge
                                   select new
                                   {
                                       e.id,
                                       workAge,
                                       e.departmentId,
                                       e.state
                                   };

                    // 过滤
                    #region
                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤
                        #region
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
                        }
                        #endregion

                    }
                    #endregion


                    var results = elements.GroupBy(e => e.workAge)
                        .Select(e => new { e.Key, count = e.Count() });

                    var category = new List<object>();
                    var data = new List<object>();

                    foreach (var e in results)
                    {
                        category.Add(e.Key);
                        data.Add(new { value = e.count, name = e.Key + "年" });
                    }
                    var series = new List<object>();
                    series.Add(new { name = "员工司龄", data });
                    var legend = new List<object>();
                    legend.Add("员工司龄");

                    var resultData = new
                    {
                        title = "司龄分析",
                        category,
                        legend = legend,
                        series = series,
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = resultData,
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
        /// 用于统计薪酬等级划分
        /// </summary>
        class KeyLevel
        {
            public string key { get; set; }
            public int min { get; set; }
            public int max { get; set; }
            public int count { get; set; }

        }
        public static OperateResult  AnalyseBySalary(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from s in db.salaryInfoList.Include("levelInfo").Include("performanceInfo").Include("benefitInfo").AsEnumerable()
                                   join e in db.employeeList
                                   on s.id equals e.salaryInfoId
                                   let salary = s.GetSalaryTotal()
                                   select new
                                   {
                                       e.id,
                                       salary,
                                       e.departmentId,
                                       e.state
                                   };



                    // 过滤
                    #region
                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤
                        #region
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
                        }
                        #endregion

                    }
                    #endregion

                    /// 分几个档次：0~2000,2000~3000,3000~5000,5000~8000,8000~12000,12000~20000,20000以上
                    /// 

                    var keyList = new List<KeyLevel>{
                        new  KeyLevel{key="2000以下", min = 0, max = 2000, count=0},
                        new KeyLevel{key="2000~3000", min = 2000, max = 3000, count=0},
                        new KeyLevel{key="3000~5000", min = 3000, max = 5000, count=0},
                        new KeyLevel{key="5000~8000", min = 5000, max = 8000, count=0},
                        new KeyLevel{key="8000~12000", min = 8000, max = 12000, count=0},
                        new KeyLevel{key="12000~20000", min = 12000, max = 20000, count=0},
                        new KeyLevel{key="20000以上", min = 20000, max = Int32.MaxValue, count=0},
                    };

                    var category = new List<object>();
                    var data = new List<object>();

                    var result = elements.ToList();

                    foreach (var e in result)
                    {
                        foreach (var k in keyList)
                        {
                            if (e.salary >= k.min && e.salary < k.max)
                            {
                                k.count++;
                                break;

                            }
                        }

                    }


                    foreach (var e in keyList)
                    {
                        if (e.count > 0)
                        {
                            data.Add(new { value = e.count, name = e.key });
                        }

                    }
                    var series = new List<object>();
                    series.Add(new { name = "员工薪酬", data });
                    var legend = new List<object>();
                    legend.Add("员工薪酬");

                    var resultData = new
                    {
                        title = "薪酬分析",
                        category,
                        legend = legend,
                        series = series,
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = resultData,
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

        public static OperateResult  AnalyseByPost(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList.AsEnumerable()
                                   select new
                                   {
                                       e.id,
                                       e.sex,
                                       e.departmentId,
                                       e.postId,
                                       e.state
                                   };

                    // 过滤
                    #region
                    if (param?.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤
                        #region
                        if (param.filters.Keys.Contains("departmentId"))
                        {
                            var p = param.filters["departmentId"];
                            long departmentId = Convert.ToInt64(p.value ?? "0");


                            Func<long, IQueryable<long>> GetSonFun = null;
                            GetSonFun = id =>
                            {
                                // 查找属于给定部门的员工
                                var sons = from e in db.departmentList
                                           where e.parentId == id
                                           select e.id;
                                IQueryable<long> many = sons;
                                // 查找属于给定部门子部门的员工
                                foreach (var it in sons)
                                {
                                    many = many.Concat(GetSonFun(it));
                                }
                                return many;
                            };

                            // 所有部门
                            var departments = (from e in db.departmentList
                                               where e.id == departmentId
                                               select e.id).Concat(GetSonFun(departmentId));

                            elements = elements.Where(t => departments.Contains(t.departmentId));
                        }
                        #endregion
                        // 过滤状态
                        #region
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.state));
                        }
                        #endregion

                    }
                    #endregion


                    var results = elements.GroupBy(e => e.postId)
                        .Select(e => new { e.Key, count = e.Count() });

                    var category = new List<object>();
                    var data = new List<object>();

                    foreach (var e in results)
                    {
                        //根据postid得到对应名称
                        var post = (from p in db.postInfoList
                            where p.id == e.Key
                            select p).FirstOrDefault();
                        if (post == null)
                        {
                            category.Add("无岗位");
                            data.Add(new { value = e.count, name = "无岗位" });
                        }
                        else
                        {
                            category.Add(post.name);
                            data.Add(new { value = e.count, name = post.name });
                        }
                    }
                    var series = new List<object>();
                    series.Add(new { name = "员工岗位", data });
                    var legend = new List<object>();
                    legend.Add("员工岗位");

                    var resultData = new
                    {
                        title = "岗位分析",
                        category,
                        legend = legend,
                        series = series,
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = resultData,
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
