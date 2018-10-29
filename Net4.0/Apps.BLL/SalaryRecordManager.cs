using Apps.Model;
using Apps.Model.Uitility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL
{
    public class SalaryRecordManager
    {
        public OperateResult Add(SalaryRecord model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var match = (from m in db.salaryRecordList.Include("assessmentInfo")
                                 where m.assessmentInfoId == model.assessmentInfoId
                                 select m
                                ).FirstOrDefault();
                    if (match != null)
                    {
                        return new OperateResult
                        {
                            content = "数据已经存在",
                        };
                    }

                    model.inputDate = DateTime.Now;

                    db.salaryRecordList.Add(model);
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
        public OperateResult AddBatch(List<SalaryRecord> listModel)
        {
            bool fail = false;
            OperateResult result = new OperateResult();

            foreach (SalaryRecord model in listModel)
            {
                OperateResult or = Add(model);
                if (or.status != OperateStatus.Success)
                {
                    fail = true;

                    result.content += "工号(" + model.assessmentInfo.employee.number + ")数据保存失败, error（" + or.content + "） ;";
                }

            }
            if (!fail)
            {
                result.status = OperateStatus.Success;
                result.content = "批量数据保存成功";
            }
            return result;
        }

        public OperateResult Remove(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = db.salaryRecordList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "该工资单",
                        };
                    }

                    db.salaryRecordList.Remove(element);

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

        public OperateResult Update(SalaryRecord model)
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
        public OperateResult GetById(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = (from m in db.salaryRecordList
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
                    var elements = (from e in db.salaryRecordList
                                    select new
                                    {
                                        e.id,
                                        e.billSerial,
                                        e.postSalary,
                                        e.fullAttendanceRewards,
                                        e.performanceRewards,
                                        e.benefitRewards,
                                        e.seniorityRewards,
                                        e.subsidy,
                                        e.reissue,
                                        e.socialSecurity,
                                        e.publicFund,
                                        e.tax,
                                        e.shouldTotal,
                                        e.actualTotal,
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


        public OperateResult GetByPager(QueryParam param = null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = from e in db.salaryRecordList.Include("assessmentInfoList")
                                   join salaryInfo in db.salaryInfoList.Include("levelInfo").Include("performanceInfo").Include("benefitInfo").AsEnumerable()
                                   on e.assessmentInfo.employeeId equals salaryInfo.employeeId
                                   join employee in db.employeeList.Include("department")
                                   on e.assessmentInfo.employeeId equals employee.id

                                   select new
                                   {
                                       e.billSerial,
                                       e.assessmentInfoId,
                                       e.assessmentInfo.month,
                                       employeeId = employee.id,
                                       employeeName = employee.name,
                                       employeeNumber = employee.number,
                                       departmentId = employee.departmentId,
                                       departmentName = employee.department.name,
                                       e.postSalary,
                                       e.fullAttendanceRewards,
                                       e.performanceRewards,
                                       e.benefitRewards,
                                       e.normalOvertimeRewards,
                                       e.holidayOvertimeRewards,
                                       e.subsidy,
                                       e.reissue,
                                       e.socialSecurity,
                                       e.publicFund,
                                       e.tax,
                                       e.shouldTotal,
                                       e.actualTotal,
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

        public OperateResult GetAssessmentByPager(QueryParam param = null)
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
                    var elements = from e in db.assessmentInfoList.Include("employee").AsEnumerable()
                                   join salaryInfo in db.salaryInfoList.Include("levelInfo").Include("performanceInfo").Include("benefitInfo")
                                   on e.employeeId equals salaryInfo.employeeId
                                   join department in db.departmentList
                                   on e.employee.departmentId equals department.id

                                   let postSalary = CalPostSalary(salaryInfo.levelInfo.postSalary, e.shouldWorkTime, e.actualWorkTime)
                                   let fullAttendanceRewards = CalFullAttendanceRewards(salaryInfo.levelInfo.fullAttendanceRewards, e.shouldWorkTime, e.actualWorkTime)
                                   let performanceRewards = CalPerformanceRewards(salaryInfo.performanceInfo.performanceRewards, e.performanceScore ?? 0)
                                   let benefitRewards = CalBenefitRewards(salaryInfo.benefitInfo.benefitRewards, e.benefitScore ?? 0)
                                   let seniorityRewards = CalSeniorityRewards(salaryInfo.levelInfo.seniorityRewardsBase, month, e.employee.entryDate ?? DateTime.Now)
                                   let normalOvertimeRewards = CalNormalOvertimeRewards(salaryInfo.levelInfo.postSalary, e.normalOvertime??0)
                                   let holidayOvertimeRewards = CalHolidayOvertimeRewards(salaryInfo.levelInfo.postSalary, e.holidayOvertime ?? 0)
                                   let shouldTotal = postSalary + fullAttendanceRewards + performanceRewards + benefitRewards + seniorityRewards + normalOvertimeRewards + holidayOvertimeRewards

                                   where e.month == month && !(db.salaryRecordList.Any(c => c.assessmentInfoId == e.id))
                                   select new
                                   {
                                       billSerial = GenerateBillSerial(month, e.employee.number),
                                       assessmentInfoId = e.id,
                                       employeeId = e.employeeId,
                                       employeeName = e.employee.name,
                                       employeeNumber = e.employee.number,
                                       departmentId = e.employee.departmentId,
                                       departmentName = department.name,
                                       postSalary = postSalary,
                                       shouldWorkTime = e.shouldWorkTime,
                                       actualWorkTime = e.actualWorkTime,
                                       isFullAttendance = isFullAttendanceRewards(e.shouldWorkTime, e.actualWorkTime) ? "是" : "否",
                                       fullAttendanceRewards = fullAttendanceRewards,
                                       performanceRewardsBase = salaryInfo.performanceInfo.performanceRewards,
                                       performanceScore = e.performanceScore,
                                       performanceRewards = performanceRewards,
                                       benefitRewardsBase = salaryInfo.benefitInfo.benefitRewards,
                                       benefitScore = e.benefitScore,
                                       benefitRewards = benefitRewards,
                                       seniorityRewardsBae = salaryInfo.levelInfo.seniorityRewardsBase,
                                       seniorityRewards = seniorityRewards,
                                       normalOvertimeRewards = normalOvertimeRewards,
                                       holidayOvertimeRewards = holidayOvertimeRewards,
                                       subsidy = 0,
                                       reissue = 0,
                                       socialSecurity = 0,
                                       publicFund = 0,
                                       tax = 0,
                                       shouldTotal = shouldTotal,
                                       actualTotal = shouldTotal,
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

        private string GenerateBillSerial(string month, string employeeNumber)
        {
            DateTime dtMonth;
            DateTime dtNow = DateTime.Now;


            if (DateTime.TryParse(month, out dtMonth))
            {
                string serial = string.Format("S-{0}-{1}{2}{3}{4}{5}", employeeNumber, dtMonth.Year,
                    dtMonth.Month, dtMonth.Day, dtNow.Hour, dtNow.Minute);


                return serial;
            }

            return "";
        }


        private double CalPostSalary(double rewardsBase, double shouldWorkTime, double actualWorkTime)
        {
            if (shouldWorkTime < 0.000001) //视为等于0
            {
                return 0.0;
            }

            if (Math.Abs(shouldWorkTime - actualWorkTime) < 0.000001)//视为全勤
            {
                return Math.Round(rewardsBase, 2);
            }
            else
            {
                return Math.Round(rewardsBase * actualWorkTime / shouldWorkTime, 2);
            }
        }

        private double CalSeniorityRewards(double rewardsBase, string month, DateTime entryDate)
        {
            DateTime dtMonth = DateTime.Now;
            if (DateTime.TryParse(month, out dtMonth))
            {
                int days = DateTime.DaysInMonth(dtMonth.Year, dtMonth.Month);
                dtMonth = dtMonth.AddDays(days - 1);

                int seniority = Utility.Utility.CalYears(entryDate, dtMonth);

                return Math.Round(seniority * rewardsBase, 2);
            }

            return 0.00;
        }

        private double CalPerformanceRewards(double rewardsBase, double score)
        {
            return Math.Round(rewardsBase * score / 100, 2);
        }

        private double CalBenefitRewards(double rewardsBase, double score)
        {
            return Math.Round(rewardsBase * score / 100, 2);
        }
        private double CalFullAttendanceRewards(double rewardsBase, double shouldWorkTime, double actualWorkTime)
        {
            if (Math.Abs(shouldWorkTime - actualWorkTime) < 0.00001)
            {
                return Math.Round(rewardsBase, 2);
            }
            return 0.00;
        }

        private bool isFullAttendanceRewards(double shouldWorkTime, double actualWorkTime)
        {
            if (Math.Abs(shouldWorkTime - actualWorkTime) < 0.00001)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 以一个月30天每天8小时为标准，用岗位工资来计算加班工资
        /// </summary>
        /// <param name="postSalary"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private double CalNormalOvertimeRewards(double postSalary, double time)
        {
            return Math.Round(postSalary / (30 * 8) * time, 2);
        }

        /// <summary>
        /// 节假日加班按照工作日加班的2倍计算
        /// </summary>
        /// <param name="postSalary"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private double CalHolidayOvertimeRewards(double postSalary, double time)
        {
            return 2 * CalNormalOvertimeRewards(postSalary, time);
        }


        public OperateResult ExportAll(QueryParam param = null)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = from e in db.salaryRecordList.Include("assessmentInfoList")
                                   join salaryInfo in db.salaryInfoList.Include("levelInfo").Include("performanceInfo").Include("benefitInfo").AsEnumerable()
                                   on e.assessmentInfo.employeeId equals salaryInfo.employeeId
                                   join employee in db.employeeList.Include("department")
                                   on e.assessmentInfo.employeeId equals employee.id

                                   select new
                                   {
                                       e.billSerial,
                                       e.assessmentInfoId,
                                       e.assessmentInfo.month,
                                       employeeId = employee.id,
                                       employeeName = employee.name,
                                       employeeNumber = employee.number,
                                       departmentId = employee.departmentId,
                                       departmentName = employee.department.name,
                                       e.postSalary,
                                       e.fullAttendanceRewards,
                                       e.performanceRewards,
                                       e.benefitRewards,
                                       e.normalOvertimeRewards,
                                       e.holidayOvertimeRewards,
                                       e.subsidy,
                                       e.reissue,
                                       e.socialSecurity,
                                       e.publicFund,
                                       e.tax,
                                       e.shouldTotal,
                                       e.actualTotal,
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
                                   select new SalaryRecordExport
                                   {
                                       index = rowIndex++,
                                       billSerial = e.billSerial,
                                       month = e.month,
                                       employeeName = e.employeeName,
                                       employeeNumber = e.employeeNumber,
                                       departmentName = e.departmentName,
                                       postSalary = e.postSalary,
                                       fullAttendanceRewards = e.fullAttendanceRewards,
                                       performanceRewards = e.performanceRewards,
                                       benefitRewards = e.benefitRewards,
                                       normalOvertimeRewards = e.normalOvertimeRewards,
                                       holidayOvertimeRewards = e.holidayOvertimeRewards,
                                       subsidy = e.subsidy,
                                       reissue = e.reissue,
                                       socialSecurity = e.socialSecurity,
                                       publicFund = e.publicFund,
                                       tax = e.tax,
                                       shouldTotal = e.shouldTotal,
                                       actualTotal = e.actualTotal,
                                       inputDate = e.inputDate
                                   };




                    DataTable dt = DataTableHelper.ToDataTable<SalaryRecordExport>(results.ToList());

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
