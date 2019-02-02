using Apps.BLL.Utility;
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
using System.Reflection;

namespace Apps.BLL
{
    public class AssessmentManager
    {
        public static OperateResult Add(AssessmentInfo model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var match = from m in db.assessmentInfoList
                                where m.employeeId == model.employeeId && m.month == model.month
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "单号为（"+ model.billSerial +"）的员工数据已经存在;",
                        };
                    }

                    var salary = (from s in db.employeeList
                        where s.id == model.employeeId
                              && s.salaryInfoId != null
                        select s).FirstOrDefault();
                    if (salary == null)
                    {
                        return new OperateResult
                        {
                            content = "单号为（" + model.billSerial + "）的员工还未设定薪酬;",
                        };
                    }


                    model.inputDate = DateTime.Now;
                    model.status = "未审核";

                    db.assessmentInfoList.Add(model);
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "添加考核:" + model.billSerial
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
                    content = "单号为（" + model.billSerial + "）的员工，保存发生错误：" 
                              + Model.Utility.Utility.GetExceptionMsg(ex) + ";",
                };
            }

        }

        public static OperateResult AddBatch(List<AssessmentInfo> listModel, string month)
        {
            bool fail = false;
            OperateResult result = new OperateResult();

            foreach (AssessmentInfo model in listModel)
            {
                model.month = month;
                OperateResult or = Add(model);
                if (or.status != OperateStatus.Success)
                {
                    fail = true;

                    result.content += or.content;
                }

            }
            if (!fail)
            {
                result.status = OperateStatus.Success;
                result.content = "批量考核数据保存成功";
            }
            return result;
        }

        public static OperateResult Remove(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var element = db.assessmentInfoList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该绩效",
                        };
                    }

                    db.assessmentInfoList.Remove(element);

                    db.Entry(element).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除考核:" + element.billSerial
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
                    db.assessmentInfoList.RemoveRange(db.assessmentInfoList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有考核"
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

        public static OperateResult Update(AssessmentInfo model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    model.inputDate = DateTime.Now;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "修改考核:" + model.billSerial
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

        public static OperateResult SaveBatch(List<AssessmentInfo> listModel)
        {
            bool fail = false;
            OperateResult result = new OperateResult();

            foreach (AssessmentInfo model in listModel)
            {
                OperateResult or = Update(model);
                if (or.status != OperateStatus.Success)
                {
                    fail = true;

                    OperateResult orEm = EmployeeManager.GetById(model.employeeId);
                    Employee e = orEm.data as Employee;
                    if (e != null)
                    {
                        result.content += "工号(" + e.number + ") 考核数据保存失败, error（" + or.content + "） ;";
                    }
                }

            }
            if (!fail)
            {
                result.status = OperateStatus.Success;
                result.content = "批量考核数据保存成功";
            }
            return result;
        }

        public static OperateResult UpdateStatusBatch(List<long> listModel, string status)
        {
            bool fail = false;
            OperateResult result = new OperateResult();

            foreach (var id in listModel)
            {
                OperateResult or = UpdateStatus(id, status);
                if (or.status != OperateStatus.Success)
                {
                    fail = true;

                    result.content += "id(" + id + ") 考核数据保存失败, error（" + or.content + "） ;";
                }

            }
            if (!fail)
            {
                result.status = OperateStatus.Success;
                result.content = "批量考核数据保存成功";
            }
            return result;
        }

        public static OperateResult UpdateStatus(long id, string status)
        {
            if (status != "未审核" && status != "已审核")
            {
                return new OperateResult
                {
                    content = "数据错误",
                };
            }

            try
            {
                using (SystemDB db = new SystemDB())
                {
                    //如果工资记录已经锁定，则不能再修改状态
                    var salary = (from s in db.salaryRecordList
                        where s.assessmentInfoId == id && s.status == "已审核"
                                  select s).FirstOrDefault();
                    if (salary != null)
                    {
                        return new OperateResult
                        {
                            content = "已经存在审核通过的工资记录，不能修改",
                        };
                    }

                    var element = (from m in db.assessmentInfoList
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

                    element.status = status;
                    db.Entry(element).State = EntityState.Modified;
                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "修改状态:" + element.billSerial + ", " + element.status
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

        public static OperateResult GetById(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = (from m in db.assessmentInfoList
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
                        content = Model.Utility.Utility.GetExceptionMsg(ex),
                    };
                }

            }

        }

        public static OperateResult GetAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    var elements = (from e in db.benefitInfoList
                                    select new
                                    {
                                        e.id,
                                        e.code,
                                        e.benefitRewards
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
                    var elements = from e in db.assessmentInfoList.Include("employee").Include("department")
                                   orderby e.employee.number
                                   select new
                                   {
                                       e.id,
                                       e.month,
                                       e.billSerial,
                                       employeeId = e.employee.id,
                                       employeeNumber = e.employee.number,
                                       employeeName = e.employee.name,
                                       departmentId = e.employee.departmentId,
                                       departmentName = e.employee.department.name,
                                       e.shouldWorkTime,
                                       e.actualWorkTime,
                                       e.normalOvertime,
                                       e.holidayOvertime,
                                       e.lateTime,
                                       e.earlyTime,
                                       e.absenteeismTime,
                                       e.personalLeaveTime,
                                       e.sickLeaveTime,
                                       e.annualVacationTime,
                                       e.performanceScore,
                                       e.benefitScore,
                                       e.inputDate,
                                       e.status

                                   };

                    #region 查询过滤
                    if (param != null && param.filters != null)
                    {
                        // 先查询出部门及子部门，再过滤
                        #region 过滤部门
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

                        // 过滤月份
                        #region 过滤月份
                        if (param.filters.Keys.Contains("month"))
                        {
                            var p = param.filters["month"];
                            elements = elements.Where(t => t.month == p.value);
                        }
                        #endregion

                        // 模糊过滤员工名字
                        #region 过滤名字
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.employeeName.Contains(p.value));
                        }
                        #endregion

                        // 精确过滤状态
                        #region 过滤状态
                        if (param != null && param.filters != null)
                        {
                            if (param.filters.Keys.Contains("status"))
                            {
                                var p = param.filters["status"];
                                elements = elements.Where(t => t.status.Equals(p.value));
                            }
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


        public static OperateResult GetEmployeesAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    if (param == null || param.filters == null || !param.filters.Keys.Contains("month"))
                    {
                        return new OperateResult
                        {
                            content = "需要月份查询条件",
                        };
                    }
                    // 得到给定月份的已经存在考核数据的记录
                    string month = param.filters["month"].value ?? "";
                    var assessmnetList = from m in db.assessmentInfoList
                                         where m.month == month
                                         select m;
                    var elements = from m in db.employeeList.Include("department").AsEnumerable()
                                   where !(assessmnetList.Any(c => c.employeeId == m.id && c.month == month))
                                   select new
                                   {
                                       billSerial = GenerateBillSerial(month, m.number),
                                       employeeId = m.id,
                                       employeeName = m.name,
                                       employeeNumber = m.number,
                                       employeeStatus = m.state,
                                       departmentId = m.departmentId,
                                       departmentName = m.department.name,
                                   };


                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param != null && param.filters != null)
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
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.employeeName.Contains(p.value));
                        }
                    }
                    #endregion

                    // 过滤状态
                    #region
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.employeeStatus));
                        }
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


        /// <summary>
        /// 1、如果员工有给定month的考核数据，则取出考核数据，否则返回默认考核数据
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static OperateResult GetEmployeesByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    if (param == null || param.filters == null || !param.filters.Keys.Contains("month"))
                    {
                        return new OperateResult
                        {
                            content = "需要月份查询条件",
                        };
                    }
                    // 得到给定月份的已经存在考核数据的记录
                    string month = param.filters["month"].value ?? "";
                    var assessmnetList = from m in db.assessmentInfoList
                                         where m.month == month
                                         select m;
                    var elements = from m in db.employeeList.Include("department").AsEnumerable()
                                   where !(assessmnetList.Any(c => c.employeeId == m.id && c.month == month))
                                   select new
                                   {
                                       billSerial = GenerateBillSerial(month, m.number),
                                       employeeId = m.id,
                                       employeeName = m.name,
                                       employeeNumber = m.number,
                                       employeeStatus = m.state,
                                       departmentId = m.departmentId,
                                       departmentName = m.department.name,
                                   };


                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param != null && param.filters != null)
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
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.employeeName.Contains(p.value));
                        }
                    }
                    #endregion

                    // 过滤状态
                    #region
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("state"))
                        {
                            var p = param.filters["state"];
                            elements = elements.Where(t => p.value.Contains(t.employeeStatus));
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
        private static string GenerateBillSerial(string month, string employeeNumber)
        {
            DateTime dtMonth;

            if (DateTime.TryParse(month, out dtMonth))
            {
                string serial = string.Format("S-{0}-{1}{2:D2}{3:D2}", employeeNumber, dtMonth.Year,
                    dtMonth.Month, dtMonth.Day);


                return serial;
            }

            return "";
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
                    var elements = from e in db.assessmentInfoList.Include("employee").Include("department")
                                   select new
                                   {
                                       e.month,
                                       e.billSerial,
                                       employeeNumber = e.employee.number,
                                       employeeName = e.employee.name,
                                       departmentId = e.employee.departmentId,
                                       departmentName = e.employee.department.name,
                                       e.shouldWorkTime,
                                       e.actualWorkTime,
                                       e.normalOvertime,
                                       e.holidayOvertime,
                                       e.lateTime,
                                       e.earlyTime,
                                       e.absenteeismTime,
                                       e.personalLeaveTime,
                                       e.sickLeaveTime,
                                       e.annualVacationTime,
                                       e.performanceScore,
                                       e.benefitScore,
                                       e.inputDate,
                                       e.status
                                   };

                    // 先查询出部门及子部门，再过滤
                    #region
                    if (param != null && param.filters != null)
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

                    // 过滤月份
                    #region
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("month"))
                        {
                            var p = param.filters["month"];
                            elements = elements.Where(t => t.month == p.value);
                        }
                    }
                    #endregion

                    // 模糊过滤名字
                    #region
                    if (param != null && param.filters != null)
                    {
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.employeeName.Contains(p.value));
                        }
                    }
                    #endregion

                    long rowIndex = 1;
                    var results = from e in elements.AsEnumerable()
                                  let index = rowIndex++
                                  select new AssessmentInfoExport
                                  {
                                      index = index,
                                      month = e.month,
                                      employeeNumber = e.employeeNumber,
                                      employeeName = e.employeeName,
                                      departmentName = e.departmentName,
                                      shouldWorkTime = e.shouldWorkTime,
                                      actualWorkTime = e.actualWorkTime,
                                      normalOvertime = e.normalOvertime ?? 0,
                                      holidayOvertime = e.holidayOvertime ?? 0,
                                      lateTime = e.lateTime ?? 0,
                                      earlyTime = e.earlyTime ?? 0,
                                      absenteeismTime = e.absenteeismTime ?? 0,
                                      personalLeaveTime = e.personalLeaveTime ?? 0,
                                      sickLeaveTime = e.sickLeaveTime ?? 0,
                                      annualVacationTime = e.annualVacationTime ?? 0,
                                      performanceScore = e.performanceScore ?? 0,
                                      benefitScore = e.benefitScore ?? 0,
                                      inputDate = e.inputDate,
                                  };

                    DataTable dt = DataTableHelper.ToDataTable<AssessmentInfoExport>(results.ToList());



                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "导出考核"
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

        public static OperateResult ImportExcel(string fileName)
        {
            try
            {
                var excelFile = new ExcelQueryFactory(fileName);
                var props = typeof(AssessmentInfoExport).GetProperties();
                foreach (var p in props)
                {
                    var colName = DataTableHelper.GetColumnDisplay(p);
                    excelFile.AddMapping(p.Name, colName);
                }

                var tsheet = excelFile.Worksheet<AssessmentInfoExport>(0);
                var query = (from e in tsheet
                             select e).ToList();

                IEnumerable<AssessmentInfo> elements;
                using (SystemDB db = new SystemDB())
                {
                    elements = (from e in query
                                join d in db.employeeList
                                 on e.employeeNumber equals d.number
                                select new AssessmentInfo
                                {
                                    month = e.month,
                                    employeeId = d.id,
                                    shouldWorkTime = e.shouldWorkTime,
                                    actualWorkTime = e.actualWorkTime,
                                    normalOvertime = e.normalOvertime,
                                    lateTime = e.lateTime,
                                    earlyTime = e.earlyTime,
                                    absenteeismTime = e.absenteeismTime,
                                    personalLeaveTime = e.personalLeaveTime,
                                    sickLeaveTime = e.sickLeaveTime,
                                    annualVacationTime = e.annualVacationTime,
                                    performanceScore = e.performanceScore,
                                    benefitScore = e.benefitScore,
                                    inputDate = e.inputDate,
                                    status = e.status,

                                }).ToList();
                }



                bool fail = false;
                OperateResult result = new OperateResult();
                foreach (var model in elements)
                {
                    OperateResult or = Add(model);
                    if (or.status != OperateStatus.Success)
                    {
                        fail = true;

                        result.content += "工号(" + model.employeeId + ")数据保存失败, error（" + or.content + "） ;";
                    }

                }
                if (!fail)
                {
                    result.status = OperateStatus.Success;
                    result.content = "批量数据保存成功";
                }
                return result;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

    }
}