using Apps.Model;
using Apps.Model.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Apps.BLL.Utility;

namespace Apps.BLL
{

    /// <summary>
    /// 分析维度
    /// </summary>
    public class Dimension
    {
        public string dimension { get; set; }
        public double score { get; set; }
        public double value { get; set; }
        public double average { get; set; }

    }


    public class LeaveManager
    {
        public static double LeaveLimitPercent = 0.6;

        public delegate OperateResult CalItem(Employee e);
        public delegate double AnalyzeItem(Employee e);

        /// <summary>
        /// 员工离职后，把该员工数据信息录入离职数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperateResult Leave(Employee model)
        {
            if (model == null)
            {
                return new OperateResult
                {
                    content = "离职员工数据为null"
                };
            }
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    EmployeeLeave employee = Model.Utility.Utility.AutoCopy<Employee, EmployeeLeave>(model);

                    employee.employeeId = model.id;

                    // 岗位
                    #region
                    var postInfo = (from e in db.postInfoList
                            where e.id == model.postId
                            select e
                        ).FirstOrDefault();
                    if (postInfo != null)
                    {
                        employee.postName = postInfo.name;
                    }
                    #endregion

                    //薪资
                    #region 
                    var salary = (from e in db.salaryInfoList
                                  where e.id == model.salaryInfoId
                                  select e).FirstOrDefault();
                    if (salary != null)
                    {
                        OperateResult or = SalaryRecordManager.GetSalaryAveById(model.id);

                        if (or.status == OperateStatus.Success)
                        {
                            employee.salary = salary.GetSalaryTotal();
                            employee.salaryAverage = (double)or.data;
                        }
                    }
                    #endregion


                    // 离职记录基数表
                    #region
                    db.EmployeeLeaveList.Add(employee);
                    db.SaveChanges();

                    #endregion

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



        public static OperateResult RemoveAll()
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    db.EmployeeLeaveList.RemoveRange(db.EmployeeLeaveList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有离职员工"
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


        public static OperateResult LeaveWarningByPager(QueryParam param = null)
        {
            try
            {
                SystemDB db = new SystemDB();

                var elements = db.employeeList.Include("department").Where(m => m.state != "离职")
                    .Select(m => m);

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


                // 预警计算

                #region

                var result = from e in elements.AsEnumerable()
                    let age = Model.Utility.Utility.CalYears(e.birthday, DateTime.Now)
                    let workAge = Model.Utility.Utility.CalMonths(e.entryDate ?? DateTime.Now, DateTime.Now)
                    let resultScore = GetEmployeeLeaveDegree(e)
                                     orderby resultScore descending 
                    select new
                    {
                        e.id,
                        e.name,
                        e.number,
                        departmentName = "",
                        postName = "",
                        age,
                        workAge,
                        resultScore = resultScore * 100 + "%"

                    };

                #endregion
                int total = result.Count();
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
                        result = result.Take(pager.rows);
                    }
                    else
                    {
                        result = result.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                    }
                }
                var data = new
                {
                    pages,
                    total,
                    rows = result.ToList()
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
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

        }

        public static double GetEmployeeLeaveDegree(Employee e)
        {
            Dictionary<AnalyzeItem, double> analyzeList = new Dictionary<AnalyzeItem, double>();
            analyzeList.Add(AnalyzeByBigData, 0.3);
            analyzeList.Add(AnalyzeBySalary, 0.3);
            analyzeList.Add(AnalyzeByPostCrossAge, 0.15);
            analyzeList.Add(AnalyzeByPostCrossWorkAge, 0.1);
            analyzeList.Add(AnalyzeByAssessment, 0.15);

            double percent = 0;

            foreach (var fun in analyzeList)
            {
                double matchDegree = fun.Key(e);

                percent += matchDegree * fun.Value;

            }

            if (percent < 0.3)
            {
                return 0.3;
            }
            else if (percent >= 0.3 && percent <= 0.4)
            {
                return 0.5;
            }
            else if (percent > 0.4 && percent <= 0.5)
            {
                return 0.55;
            }
            else if (percent > 0.5 && percent <= 0.6)
            {
                return 0.6;
            }
            else if (percent > 0.6 && percent <= 0.7)
            {
                return 0.7;
            }
            else if (percent > 0.7 && percent <= 0.8)
            {
                return 0.75;
            }
            else if (percent > 0.8)
            {
                return 0.8;
            }

            return 0.0;
        }

        #region 大数据计算

        /// <summary>
        /// 通过历史返回计算多条件的最终匹配度
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double AnalyzeByBigData(Employee e)
        {
            int condition = 0;
            int count = 0;

            List<CalItem> calFuns = new List<CalItem>();
            calFuns.Add(CalPost);
            calFuns.Add(CalAge);
            calFuns.Add(CalEducation);
            calFuns.Add(CalWorkAge);
            calFuns.Add(CalSalaryActualTotal);

            foreach (var fun in calFuns)
            {
                OperateResult or = fun(e);

                if (or.status == OperateStatus.Success)
                {
                    condition++;
                    bool fit = (bool)or.data;
                    count = fit ? count + 1 : count;
                }
            }

            if (count == 0 || condition == 0)
            {
                return 0;
            }
            else
            {
                return count / (double)condition;
            }
        }



        /// <summary>
        /// 岗位离职率占比相加达60%的岗位
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>计算成功返回status返回success，如果成立则data返回true，否则返回false</returns>
        private static OperateResult CalPost(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            try
            {
                SystemDB dbLeave = new SystemDB();

                var total = dbLeave.EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = dbLeave.EmployeeLeaveList.GroupBy(g => g.postName)
                    .Select(m => new {postName = m.Key, count = m.Count()})
                    .OrderBy(m => m.count);

                foreach (var item in dimensionGroup)
                {
                    count += item.count;
                    if (item.postName == employee.postInfo.name)
                    {
                        double percent = count / (double)total;
                        if (percent < LeaveLimitPercent)
                        {
                            return new OperateResult
                            {
                                status = OperateStatus.Success,
                                data = true,
                            };
                        }
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
            return new OperateResult
            {
                status = OperateStatus.Success,
                data = false,
            };
        }

        /// <summary>
        /// 年龄段离职率占比相加达60%的年龄段
        /// 按照年为单元计算
        /// </summary>
        /// <param name="employee">未离职员工</param>
        /// <returns>计算成功返回status返回success，如果成立则data返回true，否则返回false</returns>
        private static OperateResult CalAge(Employee employee)
        {
            try
            {
                SystemDB dbLeave = new SystemDB();

                var total = dbLeave.EmployeeLeaveList.Count();
                var count = 0;

                int age = Model.Utility.Utility.CalYears(employee.birthday, DateTime.Now);

                var dimensionGroup = dbLeave.EmployeeLeaveList.AsEnumerable().GroupBy(g => Model.Utility.Utility.CalYears(g.birthday, g.leaveDate.Value))
                    .Select(m => new { age = m.Key, count = m.Count() })
                    .OrderBy(m => m.count);

                foreach (var item in dimensionGroup)
                {
                    count += item.count;
                    if (item.age == age)
                    {
                        double percent = count / (double)total;
                        if (percent < LeaveLimitPercent)
                        {
                            return new OperateResult
                            {
                                status = OperateStatus.Success,
                                data = true,
                            };
                        }
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
            return new OperateResult
            {
                status = OperateStatus.Success,
                data = false,
            };
        }


        /// <summary>
        /// 学历段离职率占比相加达60%的学历段
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>计算成功返回status返回success，如果成立则data返回true，否则返回false</returns>
        private static OperateResult CalEducation(Employee employee)
        {
            if (employee.education == null)
            {
                return new OperateResult
                {
                    content = "无学历信息",
                };
            }

            try 
            {
                SystemDB dbLeave = new SystemDB();

                var total = dbLeave.EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = dbLeave.EmployeeLeaveList.GroupBy(g => g.education)
                    .Select(m => new { education = m.Key, count = m.Count() })
                    .OrderBy(m => m.count);

                foreach (var item in dimensionGroup)
                {
                    count += item.count;
                    if (item.education == employee.education)
                    {
                        double percent = count / (double)total;
                        if (percent < LeaveLimitPercent)
                        {
                            return new OperateResult
                            {
                                status = OperateStatus.Success,
                                data = true,
                            };
                        }
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
            return new OperateResult
            {
                status = OperateStatus.Success,
                data = false,
            };
        }

        /// <summary>
        /// 司龄月数离职率占比相加达60%的司龄段
        /// 按照月为单元计算
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>计算成功返回status返回success，如果成立则data返回true，否则返回false</returns>
        private static OperateResult CalWorkAge(Employee employee)
        {
            if (employee.entryDate == null)
            {
                return new OperateResult()
                {
                    content = "入职日期有误"
                };
            }
            try
            {
                SystemDB dbLeave = new SystemDB();

                var total = dbLeave.EmployeeLeaveList.Count();
                var count = 0;

                //按照月计算
                int workAge = Model.Utility.Utility.CalMonths(employee.entryDate.Value, DateTime.Now);

                var dimensionGroup = dbLeave.EmployeeLeaveList.AsEnumerable().GroupBy(g => Model.Utility.Utility.CalMonths(g.entryDate.Value, g.leaveDate.Value))
                    .Select(m => new { workAge = m.Key, count = m.Count() })
                    .OrderBy(m => m.count);

                foreach (var item in dimensionGroup)
                {
                    count += item.count;
                    if (item.workAge == workAge)
                    {
                        double percent = count / (double)total;
                        if (percent < LeaveLimitPercent)
                        {
                            return new OperateResult
                            {
                                status = OperateStatus.Success,
                                data = true,
                            };
                        }
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
            return new OperateResult
            {
                status = OperateStatus.Success,
                data = false,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee">实发工资</param>
        /// <returns>计算成功返回status返回success，如果成立则data返回true，否则返回false</returns>
        private static OperateResult CalSalaryActualTotal(Employee employee)
        { 
            try
            {
                var db = new SystemDB();

                var total = db.EmployeeLeaveList.Count();
                var count = 0;

                var salaryInfo = db.salaryInfoList.Where(m => m.id == employee.salaryInfoId)
                    .Select(m => m).FirstOrDefault();
                if (salaryInfo == null)
                {
                    return new OperateResult
                    {
                        content = "无薪酬信息",
                    };
                }

                var actualTotal = salaryInfo.GetSalaryTotal();

                var dimensionGroup = db.EmployeeLeaveList.GroupBy(g => g.salary)
                    .Select(m => new { salary = m.Key, count = m.Count() })
                    .OrderByDescending(m => m.count)
                    .ThenBy(m => m.salary);

                foreach (var item in dimensionGroup)
                {
                    count += item.count;
                    if (Math.Abs(item.salary - actualTotal) < 0.01)
                    {
                        double percent = count / (double)total;
                        if (percent < LeaveLimitPercent)
                        {
                            return new OperateResult
                            {
                                status = OperateStatus.Success,
                                data = true,
                            };
                        }
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
            return new OperateResult
            {
                status = OperateStatus.Success,
                data = false,
            };
        }

        #endregion


        #region 薪酬相关计算

        public static double AnalyzeBySalary(Employee e)
        {
            int condition = 0;
            int count = 0;

            List<CalItem> calFuns = new List<CalItem>();
            calFuns.Add(CalSalaryInDepInPost);
            calFuns.Add(CalSalaryInDep);
            calFuns.Add(CalSalaryInPost);
            calFuns.Add(CalSalaryInHistoryPost);
            calFuns.Add(CalSalaryPersonal);

            foreach (var fun in calFuns)
            {
                OperateResult or = fun(e);

                if (or.status == OperateStatus.Success)
                {
                    condition++;
                    bool fit = (bool)or.data;
                    count = fit ? count + 1 : count;
                }
            }

            if (count == 0 || condition == 0)
            {
                return 0;
            }
            else
            {
                return count / (double)condition;
            }
        }

        /// <summary>
        /// 当月低于该部门该岗位平均实发工资
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalSalaryInDepInPost(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m=>m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                //得到最近一月部门内该岗位平均工资
                var ave = db.salaryRecordList.Where(m => m.assessmentInfo.month == record.assessmentInfo.month
                                                         && m.assessmentInfo.employee.departmentId ==
                                                         record.assessmentInfo.employee.departmentId
                                                         && m.assessmentInfo.employee.postId ==
                                                         record.assessmentInfo.employee.postId)
                    .Average(m => m.actualTotal);

                if (record.actualTotal < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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
        /// 当月低于该部门平均工资
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalSalaryInDep(Employee employee)
        {
            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                //得到最近一月部门内平均工资
                var ave = db.salaryRecordList.Where(m => m.assessmentInfo.month == record.assessmentInfo.month
                                                         && m.assessmentInfo.employee.departmentId ==
                                                         record.assessmentInfo.employee.departmentId)
                    .Average(m => m.actualTotal);

                if (record.actualTotal < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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
        /// 当月低于该全体该岗位平均工资
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalSalaryInPost(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                //得到最近一月部门内平均工资
                var ave = db.salaryRecordList.Where(m => m.assessmentInfo.month == record.assessmentInfo.month
                                                         && m.assessmentInfo.employee.postId ==
                                                         record.assessmentInfo.employee.postId)
                    .Average(m => m.actualTotal);

                if (record.actualTotal < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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
        /// 当月实发工资小于或等于该岗位已离职员工平均工资
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalSalaryInHistoryPost(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                var ave = (from s in db.salaryRecordList
                    join e in db.EmployeeLeaveList on s.assessmentInfo.employeeId equals e.employeeId
                    where e.postId == employee.postId
                    select s.actualTotal).Average();

                if (record.actualTotal < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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
        /// 当月低于其上三个月平均实发工资
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalSalaryPersonal(Employee employee)
        {
            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                //得到该员工平均工资
                var ave = db.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Skip(1).Take(3)
                    .Average(m => m.actualTotal);

                if (record.actualTotal < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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


        #endregion

        #region 交叉计算

        public static double AnalyzeByPostCrossAge(Employee e)
        {
            int condition = 0;
            int count = 0;

            List<CalItem> calFuns = new List<CalItem>();
            calFuns.Add(CalPostCrossAge);

            foreach (var fun in calFuns)
            {
                OperateResult or = fun(e);

                if (or.status == OperateStatus.Success)
                {
                    condition++;
                    bool fit = (bool)or.data;
                    count = fit ? count + 1 : count;
                }
            }

            if (count == 0 || condition == 0)
            {
                return 0;
            }
            else
            {
                return count / (double)condition;
            }
        }

        public static double AnalyzeByPostCrossWorkAge(Employee e)
        {
            int condition = 0;
            int count = 0;

            List<CalItem> calFuns = new List<CalItem>();
            calFuns.Add(CalPostCrossWorkAge);

            foreach (var fun in calFuns)
            {
                OperateResult or = fun(e);

                if (or.status == OperateStatus.Success)
                {
                    condition++;
                    bool fit = (bool)or.data;
                    count = fit ? count + 1 : count;
                }
            }

            if (count == 0 || condition == 0)
            {
                return 0;
            }
            else
            {
                return count / (double)condition;
            }
        }

        private class StageItem
        {
            public int min { get; set; }
            public int max { get; set; }
            public int count { get; set; }
        }

        /// <summary>
        /// 岗位与年龄交叉计算
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalPostCrossAge(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            List<StageItem> liststageItems = new List<StageItem>();
            liststageItems.Add(new StageItem { min = 0, max = 18 });
            liststageItems.Add(new StageItem { min = 19, max = 22 });
            liststageItems.Add(new StageItem { min = 23, max = 25 });
            liststageItems.Add(new StageItem { min = 26, max = 29 });
            liststageItems.Add(new StageItem { min = 29, max = 31 });
            liststageItems.Add(new StageItem { min = 32, max = 35 });
            liststageItems.Add(new StageItem { min = 36, max = 40 });
            liststageItems.Add(new StageItem { min = 41, max = 45 });
            liststageItems.Add(new StageItem { min = 46, max = 100 });

            int employeeAge = Model.Utility.Utility.CalYears(employee.birthday, DateTime.Now);

            try
            {
                var db = new SystemDB();

                foreach (var stageItem in liststageItems)
                {
                    var count = (from e in db.EmployeeLeaveList
                        where e.postId == employee.id
                        let age = Model.Utility.Utility.CalYears(e.birthday, e.leaveDate.Value)
                        where age >= stageItem.min && age <= stageItem.max
                        select e).Count();

                    stageItem.count = count;
                }

                // 排序
                var orderList = liststageItems.OrderByDescending(m => m.count);
                double rate = 1 / orderList.Count();
                double percent = 1.0;

                foreach (var stageItem in orderList)
                {
                    if (employeeAge >= stageItem.min && employeeAge <= stageItem.max)
                    {
                        return new OperateResult
                        {
                            status = OperateStatus.Success,
                            data = percent
                        };
                    }

                    percent -= rate;
                }

            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

            return new OperateResult
            {
                status = OperateStatus.Error,
            };
        }

        /// <summary>
        /// 岗位与司龄交叉计算
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalPostCrossWorkAge(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            List<StageItem> liststageItems = new List<StageItem>();
            liststageItems.Add(new StageItem { min = 0, max = 1 });
            liststageItems.Add(new StageItem { min = 2, max = 2 });
            liststageItems.Add(new StageItem { min = 3, max = 3 });
            liststageItems.Add(new StageItem { min = 4, max = 4 });
            liststageItems.Add(new StageItem { min = 5, max = 5 });
            liststageItems.Add(new StageItem { min = 6, max = 6 });
            liststageItems.Add(new StageItem { min = 7, max = 7 });
            liststageItems.Add(new StageItem { min = 8, max = 8 });
            liststageItems.Add(new StageItem { min = 9, max = 9 });
            liststageItems.Add(new StageItem { min = 10, max = 10 });
            liststageItems.Add(new StageItem { min = 11, max = 11 });
            liststageItems.Add(new StageItem { min = 12, max = 23 });
            liststageItems.Add(new StageItem { min = 24, max = 35 });
            liststageItems.Add(new StageItem { min = 36, max = 47 });
            liststageItems.Add(new StageItem { min = 48, max = 59 });
            liststageItems.Add(new StageItem { min = 60, max = 1200 });

            int employeeAge = Model.Utility.Utility.CalMonths(employee.entryDate.Value, DateTime.Now);

            try
            {
                var db = new SystemDB();

                foreach (var stageItem in liststageItems)
                {
                    var count = (from e in db.EmployeeLeaveList.AsEnumerable()
                                 where e.postId == employee.id
                                 let age = Model.Utility.Utility.CalMonths(e.entryDate.Value, e.leaveDate.Value)
                                 where age >= stageItem.min && age <= stageItem.max
                                 select e).Count();

                    stageItem.count = count;
                }

                // 排序
                var orderList = liststageItems.OrderByDescending(m => m.count);
                double rate = 1 / orderList.Count();
                double percent = 1.0;

                foreach (var stageItem in orderList)
                {
                    if (employeeAge >= stageItem.min && employeeAge <= stageItem.max)
                    {
                        return new OperateResult
                        {
                            status = OperateStatus.Success,
                            data = percent
                        };
                    }

                    percent -= rate;
                }

            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }

            return new OperateResult
            {
                status = OperateStatus.Error,
            };
        }

        #endregion



        #region 考核相关计算

        public static double AnalyzeByAssessment(Employee e)
        {
            int condition = 0;
            int count = 0;

            List<CalItem> calFuns = new List<CalItem>();
            calFuns.Add(CalAssessmentInDep);
            calFuns.Add(CalAssessmentInCompany);
            calFuns.Add(CalAssessmentInHistory);

            foreach (var fun in calFuns)
            {
                OperateResult or = fun(e);

                if (or.status == OperateStatus.Success)
                {
                    condition++;
                    bool fit = (bool)or.data;
                    count = fit ? count + 1 : count;
                }
            }

            if (count == 0 || condition == 0)
            {
                return 0;
            }
            else
            {
                return count / (double)condition;
            }
        }
        /// <summary>
        /// 当月低于该部门平均考核分
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalAssessmentInDep(Employee employee)
        {
            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在考核数据，无法就行分析",
                    };
                }

                //得到最近一次部门内考核得分
                var ave = db.assessmentInfoList.Where(m => m.employee.departmentId == employee.departmentId
                                                           && m.month == record.month)
                        .Select(m => m)
                        .Average(m => m.performanceScore);

                if (record.performanceScore < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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
        /// 当月低于该公司平均考核分
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalAssessmentInCompany(Employee employee)
        {
            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在考核数据，无法就行分析",
                    };
                }

                //得到最近一次部门内考核得分
                var ave = db.assessmentInfoList.Where(m => m.month == record.month)
                    .Select(m => m)
                    .Average(m => m.performanceScore);

                if (record.performanceScore < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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
        /// 当月考核分低于其上三个月平均考核分
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private static OperateResult CalAssessmentInHistory(Employee employee)
        {
            try
            {
                var db = new SystemDB();

                //得到最近一次该员工的工资数据
                var record = db.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在考核数据，无法就行分析",
                    };
                }

                //得到上三次部门内考核得分
                var ave = db.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Skip(1).Take(3)
                    .Select(m => m)
                    .Average(m => m.performanceScore);

                if (record.performanceScore < ave)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true
                    };
                }
                else
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = false
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


        #endregion
    }
}
