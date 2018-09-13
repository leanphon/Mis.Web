using Apps.Model;
using Apps.Model.Uitility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Apps.BLL
{
    public class AssessmentManager
    {
        public OperateResult Add(AssessmentInfo model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var match = from m in db.assessmentInfoList
                                where m.employeeId==model.employeeId && m.month==model.month
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "数据已经存在",
                        };
                    }
                    model.inputDate = DateTime.Now;

                    db.assessmentInfoList.Add(model);
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

        public OperateResult AddBatch(List<AssessmentInfo> listModel, string month)
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

                    EmployeeManager em = new EmployeeManager();
                    OperateResult orEm = em.GetById(model.employeeId);
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

        public OperateResult Remove(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = db.benefitInfoList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该绩效",
                        };
                    }

                    db.benefitInfoList.Remove(element);

                    db.Entry(element).State = System.Data.Entity.EntityState.Deleted;
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

        public OperateResult Update(AssessmentInfo model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;

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

        public OperateResult SaveBatch(List<AssessmentInfo> listModel)
        {
            bool fail = false;
            OperateResult result = new OperateResult();

            foreach (AssessmentInfo model in listModel)
            {
                OperateResult or = Update(model);
                if (or.status != OperateStatus.Success)
                {
                    fail = true;

                    EmployeeManager em = new EmployeeManager();
                    OperateResult orEm = em.GetById(model.employeeId);
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


        public OperateResult GetById(long id)
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
                        content = ex.Message,
                    };
                }

            }

        }

        public OperateResult GetAll(QueryParam param = null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }
        }

        public OperateResult GetPage(QueryParam param = null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = from e in db.assessmentInfoList.Include("employee").Include("department")
                                   select new
                                   {
                                       e.month,
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
                                       e.inputDate
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


        /// <summary>
        /// 1、如果员工有给定month的考核数据，则取出考核数据，否则返回默认考核数据
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //public OperateResult GetEmployeesByPager(QueryParam param = null)
        //{
        //    using (SystemDB db = new SystemDB())
        //    {
        //        try
        //        {
        //            if (param == null || param.filters == null || !param.filters.Keys.Contains("month"))
        //            {
        //                return new OperateResult
        //                {
        //                    content = "需要月份查询条件",
        //                };
        //            }
        //            // 得到给定月份的已经存在考核数据的记录
        //            string month = param.filters["month"].value ?? "";
        //            var assessmnetList = from m in db.assessmentInfoList
        //                                 where m.month == month
        //                                 select m;
        //            var elements = from m in db.employeeList.Include("department")
        //                           join t in assessmnetList
        //                           on m.id equals t.employeeId
        //                           into newTable
        //                           from e in newTable.DefaultIfEmpty()
        //                           select new
        //                           {
        //                               id = (e.id != null) ? e.id  : 0,
        //                               employeeId = m.id,
        //                               employeeName = m.name,
        //                               employeeNumber = m.number,
        //                               departmentId = m.departmentId,
        //                               departmentName = m.department.name,
        //                               shouldWorkTime = (e.shouldWorkTime != null) ? e.shouldWorkTime : 0,
        //                               actualWorkTime = (e.actualWorkTime != null) ? e.actualWorkTime : 0,
        //                               normalOvertime = e.normalOvertime ?? 0,
        //                               holidayOvertime = e.holidayOvertime ?? 0,
        //                               lateTime = e.lateTime ?? 0,
        //                               earlyTime = e.earlyTime ?? 0,
        //                               absenteeismTime = e.absenteeismTime ?? 0,
        //                               personalLeaveTime = e.personalLeaveTime ?? 0,
        //                               sickLeaveTime = e.sickLeaveTime ?? 0,
        //                               annualVacationTime = e.annualVacationTime ?? 0,
        //                               performanceScore = e.performanceScore ?? 0,
        //                               benefitScore = e.benefitScore ?? 0,
        //                           };

        //            // 先查询出部门及子部门，再过滤
        //            #region
        //            if (param != null && param.filters != null)
        //            {
        //                if (param.filters.Keys.Contains("departmentId"))
        //                {
        //                    var p = param.filters["departmentId"];
        //                    long departmentId = Convert.ToInt64(p.value ?? "0");


        //                    Func<long, IQueryable<long>> GetSonFun = null;
        //                    GetSonFun = id =>
        //                    {
        //                        // 查找属于给定部门的员工
        //                        var sons = from e in db.departmentList
        //                                   where e.parentId == id
        //                                   select e.id;
        //                        IQueryable<long> many = sons;
        //                        // 查找属于给定部门子部门的员工
        //                        foreach (var it in sons)
        //                        {
        //                            many = many.Concat(GetSonFun(it));
        //                        }
        //                        return many;
        //                    };

        //                    // 所有部门
        //                    var departments = (from e in db.departmentList
        //                                       where e.id == departmentId
        //                                       select e.id).Concat(GetSonFun(departmentId));

        //                    elements = elements.Where(t => departments.Contains(t.departmentId));
        //                }
        //            }
        //            #endregion

        //            // 模糊过滤名字
        //            #region
        //            if (param != null && param.filters != null)
        //            {
        //                if (param.filters.Keys.Contains("employeeName"))
        //                {
        //                    var p = param.filters["employeeName"];
        //                    elements = elements.Where(t => t.employeeName.Contains(p.value));
        //                }
        //            }
        //            #endregion



        //            int total = elements.Count();
        //            int pages = 0;
        //            Pager pager = param.pager;
        //            if (pager == null || pager.rows == 0)
        //            {
        //                pages = total > 0 ? 1 : 0;
        //            }
        //            else
        //            {
        //                pages = total / (pager.rows == 0 ? 10 : pager.rows);
        //                pages = total % pager.rows == 0 ? pages : pages + 1;
        //                if (pager.page <= 1)
        //                {
        //                    elements = elements.Take(pager.rows);
        //                }
        //                else
        //                {
        //                    elements = elements.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
        //                }
        //            }

        //            var data = new
        //            {
        //                pages,
        //                total,
        //                rows = elements.ToList()
        //            };

        //            return new OperateResult
        //            {
        //                status = OperateStatus.Success,
        //                data = data,
        //            };

        //        }
        //        catch (Exception ex)
        //        {
        //            return new OperateResult
        //            {
        //                content = ex.Message,
        //            };
        //        }

        //    }
        //}

        public OperateResult GetEmployeesByPager(QueryParam param = null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
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
                    var elements = from m in db.employeeList.Include("department")
                                   where !(assessmnetList.Any(c => c.employeeId==m.id && c.month==month))
                                   select new
                                   {
                                       employeeId = m.id,
                                       employeeName = m.name,
                                       employeeNumber = m.number,
                                       employeeStatus = m.status,
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
                        if (param.filters.Keys.Contains("status"))
                        {
                            var p = param.filters["status"];
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

        /// <summary>
        /// 返回值中的data是DataTable
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public OperateResult ExportAll(QueryParam param = null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = from e in db.assessmentInfoList.Include("employee").Include("department")
                                   select new
                                   {
                                       e.month,
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
                                       e.inputDate
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

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        content = "导出成功",
                        data = dt,
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