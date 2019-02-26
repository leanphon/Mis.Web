using Apps.Model;
using Apps.Model.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    #region 分析样本空间为：公司全体离职人员

    /// <summary>
    /// 从岗位角度分析
    /// </summary>
    public class DimensionCompanyPost
    {
        public long PostId { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }
    }

    /// <summary>
    /// 从年龄角度分析
    /// </summary>
    public class DimensionCompanyAge
    {
        /// <summary>
        /// 年龄、按岁计算
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 3年为一个阶梯
        /// </summary>
        public static readonly int Module = 3;

        public static int Format(int src)
        {
            return Module * ( src / Module);
        }
    }

    /// <summary>
    /// 从司龄角度分析
    /// </summary>
    public class DimensionCompanyWorkAge
    {
        /// <summary>
        /// 小于2年，按月计算、大于2年的对月取整按年计算
        /// </summary>
        public int WorkAge { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 12个月为一个阶梯
        /// </summary>
        public static readonly int Module = 12;

        public static int Format(int src)
        {
            if (src < 12)
            {
                return src;
            }
            return Module * (src / Module);
        }
    }

    /// <summary>
    /// 从学历角度分析
    /// </summary>
    public class DimensionCompanyEducation
    {
        public string Education { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }
    }


    /// <summary>
    /// 从实发薪资角度分析
    /// </summary>
    public class DimensionCompanySalary
    {
        /// <summary>
        /// 按500一个阶梯进行计算
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 500元为一个阶梯
        /// </summary>
        public static readonly int Module = 500;

        public static double Format(double src)
        {
            return Module * (src / Module);
        }
    }

    #endregion

    #region 分析样本空间：为平均薪资范畴

    /// <summary>
    /// 平均工资
    /// </summary>
    public class DimensionSalaryAve
    {
        /// <summary>
        /// 按500一个阶梯进行计算
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 500元为一个阶梯
        /// </summary>
        public static readonly int Module = 500;

        public static double Format(double src)
        {
            return Module * (src / Module);
        }
    }
    /// <summary>
    /// 从部门内同岗位角度分析
    /// </summary>
    public class DimensionSalaryAveRelateDepPost : DimensionSalaryAve
    {
        public long DepartmentId { get; set; }
        public long PostId { get; set; }
    }
    /// <summary>
    /// 从部门内角度分析
    /// </summary>
    public class DimensionSalaryAveRelateDep : DimensionSalaryAve
    {
        public long DepartmentId { get; set; }
    }
    /// <summary>
    /// 从同岗位角度分析
    /// </summary>
    public class DimensionSalaryAveRelatePost : DimensionSalaryAve
    {
        public long PostId { get; set; }
    }
    /// <summary>
    /// 从同岗位离职员工角度分析
    /// </summary>
    public class DimensionSalaryAveRelatePostLeave : DimensionSalaryAve
    {
        public long PostId { get; set; }
    }
    /// <summary>
    /// 从个人上三个月角度分析
    /// </summary>
    public class DimensionSalaryAveRelateNearThree : DimensionSalaryAve
    {
    }

    #endregion

    #region 分析样本空间：同岗位范畴

    /// <summary>
    /// 从年龄角度分析
    /// </summary>
    public class DimensionPostRelateAge
    {
        /// <summary>
        /// 年龄、按岁计算
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 3年为一个阶梯
        /// </summary>
        public static readonly int Module = 3;

        public static int Format(int src)
        {
            if (src <= 18)
            {
                return 18;
            }

            if (src > 45)
            {
                return 46;
            }

            return Module * (src / Module);
        }
    }

    /// <summary>
    /// 从司龄角度分析
    /// </summary>
    public class DimensionPostRelateWorkAge
    {
        /// <summary>
        /// 小于2年，按月计算、大于2年的对月取整按年计算
        /// </summary>
        public int WorkAge { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 12个月为一个阶梯
        /// </summary>
        public static readonly int Module = 12;

        public static int Format(int src)
        {
            if (src < 12)
            {
                return src;
            }
            return Module * (src / Module);
        }
    }


    #endregion

    #region 分析样本空间：考核范畴

    /// <summary>
    /// 从部门角度分析
    /// </summary>
    public class DimensionAssessmentAveRelateDep
    {
        /// <summary>
        /// 部门名称计算
        /// </summary>
        public long DepartmentId { get; set; }
        /// <summary>
        /// 考核得分
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 该属性值在总离职人员里所占比重
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 3年为一个阶梯
        /// </summary>
        public static readonly int Module = 5;

        public static double Format(double src)
        {
            if (src <= 40)
            {
                return 40;
            }

            return Module * (src / Module);
        }
    }

    /// <summary>
    /// 从公司角度分析
    /// </summary>
    public class DimensionAssessmentAveRelateCompany
    {
        /// <summary>
        /// 考核得分
        /// </summary>
        public double Score { get; set; }

    }

    /// <summary>
    /// 从离职员工当月角度分析
    /// </summary>
    public class DimensionAssessmentAveRelateLeave
    {
        /// <summary>
        /// 考核得分
        /// </summary>
        public double Score { get; set; }

    }

    /// <summary>
    /// 从个人上三个月角度分析
    /// </summary>
    public class DimensionAssessmentAveRelateNearThree
    {
        /// <summary>
        /// 考核得分
        /// </summary>
        public double Score { get; set; }

    }



    #endregion

    public class LeaveManager
    {

        #region 属性定义

        public  double LeaveLimitPercent = 0.6;

        public delegate OperateResult CalItem(Employee e);
        public delegate double AnalyzeItem(Employee e);

        public SystemDB DbCache { get; set; }

        /// <summary>
        /// 一次性从数据库加载到内存
        /// </summary>
        public List<EmployeeLeave> EmployeeLeaveList{ get; set; }

        public List<DimensionCompanyAge> ListCompanyAge { get; set; }
        public List<DimensionCompanyPost> ListCompanyPost { get; set; }
        public List<DimensionCompanyEducation> ListCompanyEducation { get; set; }
        public List<DimensionCompanySalary> ListCompanySalary { get; set; }
        public List<DimensionCompanyWorkAge> ListCompanyWorkAge { get; set; }

        public List<DimensionSalaryAveRelateDepPost> ListSalaryAveDepPost { get; set; }
        public List<DimensionSalaryAveRelateDep> ListSalaryAveDep { get; set; }
        public List<DimensionSalaryAveRelatePost> ListSalaryAvePost { get; set; }
        public List<DimensionSalaryAveRelatePostLeave> ListSalaryAvePostLeave { get; set; }
        public List<DimensionSalaryAveRelateNearThree> ListSalaryAveNearThree { get; set; }

        /// <summary>
        /// key为岗位id
        /// </summary>
        public Dictionary<long, List<DimensionPostRelateAge>> MapPostRelateAge { get; set; }
        /// <summary>
        /// key为岗位id
        /// </summary>
        public Dictionary<long, List<DimensionPostRelateWorkAge>> MapPostRelateWorkAge { get; set; }

        public List<DimensionAssessmentAveRelateDep> ListAssessmentAveDep { get; set; }
        public DimensionAssessmentAveRelateCompany AssessmentAveCompany { get; set; }
        public DimensionAssessmentAveRelateLeave AssessmentAveLeave { get; set; }
        public DimensionAssessmentAveRelateNearThree AssessmentAveNearThree { get; set; }

#endregion
        /// <summary>
        /// 员工离职后，把该员工数据信息录入离职数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static  OperateResult Leave(Employee model)
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

        public static object HandlePageData(object listData, Pager pager)
        {
            dynamic listObj = listData;
            if (listObj == null)
            {
                return null;
            }
            int total = listObj.Count();
            int pages = 0;
            IEnumerable<object> pageData = new List<object>();

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
                    pageData = listObj.Take(pager.rows);
                }
                else
                {
                    pageData = listObj.Skip((pager.page - 1) * pager.rows).Take(pager.rows);
                }
            }
            var pagerData = new
            {
                pages,
                total,
                rows = pageData.ToList()
            };

            return pagerData;
        }

        public static OperateResult LeaveWarningByPager(QueryParam param = null)
        {
            try
            {
                using (var db = new SystemDB())
                {
                    var elements = db.employeeList.Include("department").Where(m => m.state != "离职")
                    .Select(m => m);

                    #region 先查询出部门及子部门，再过滤

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

                    #region 模糊过滤名字
                    if (param?.filters != null)
                    {
                        if (param.filters.Keys.Contains("employeeName"))
                        {
                            var p = param.filters["employeeName"];
                            elements = elements.Where(t => t.name.Contains(p.value));
                        }
                    }
                    #endregion


                    var result = from e in elements.Include("department").Include("postInfo").AsEnumerable()
                                 join rate in db.EmployeeLeaveRateList on e.id equals rate.EmployeeId
                                 let age = Utility.CalYears(e.birthday, DateTime.Now)
                                 let workAge = Utility.CalMonths(e.entryDate ?? DateTime.Now, DateTime.Now)
                                 orderby rate.Rate descending, workAge
                                 select new
                                 {
                                     e.id,
                                     e.name,
                                     e.number,
                                     departmentName = e.department.name,
                                     postName = e.postInfo?.name ?? "",
                                     age,
                                     workAge = Utility.FormatWorkAge(workAge),
                                     resultScore = rate.Rate * 100 + "%"

                                 };

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

                    var pageData = new
                    {
                        pages,
                        total,
                        rows = result.ToList()
                    };

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = pageData,
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

        #region 预统计分析

        OperateResult BeforeAnalyze()
        {
            try
            {
                DbCache = new SystemDB();
                EmployeeLeaveList = DbCache.EmployeeLeaveList.ToList();

                #region 全体

                PreCalRelateCompanyAge();
                PreCalRelateCompanyPost();
                PreCalRelateCompanyWorkAge();
                PreCalRelateCompanyEducation();
                PreCalRelateCompanySalary();

                #endregion

                #region 薪酬

                PreCalSalaryRelateDep();
                PreCalSalaryRelateDepPost();
                PreCalSalaryRelatePost();
                PreCalSalaryRelatePostLeave();

                #endregion

                #region 岗位

                PreCalPostRelateAge();
                PreCalPostRelateWorkAge();


                #endregion

                #region 考核

                PreCalAssessmentRelateCompany();
                PreCalAssessmentRelateLeave();
                PreCalAssessmentRelateDep();

                #endregion
                /*
                #region 全体

                OperateResult or = PreCalRelateCompanyAge();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalRelateCompanyPost();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalRelateCompanyWorkAge();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalRelateCompanyEducation();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalRelateCompanySalary();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }


                #endregion

                #region 薪酬

                or = PreCalSalaryRelateDep();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalSalaryRelateDepPost();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalSalaryRelatePost();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalSalaryRelatePostLeave();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }

                #endregion

                #region 岗位

                or = PreCalPostRelateAge();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }

                or = PreCalPostRelateWorkAge();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }


                #endregion

                #region 考核

                or = PreCalAssessmentRelateCompany();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalAssessmentRelateLeave();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }
                or = PreCalAssessmentRelateDep();
                if (or.status == OperateStatus.Error)
                {
                    return or;
                }

                #endregion

                */
                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult SaveLeaveRateToDb(List<EmployeeLeaveRate> listRate)
        {
            try
            {
                DbCache.EmployeeLeaveRateList.RemoveRange(DbCache.EmployeeLeaveRateList);

                DbCache.SaveChanges();

                DbCache.EmployeeLeaveRateList.AddRange(listRate);

                DbCache.SaveChanges();

                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        public OperateResult AnalyzeOneTime()
        {
            OperateResult or = BeforeAnalyze();
            if (or.status == OperateStatus.Error)
            {
                return or;
            }

            try
            {
                var result = from e in DbCache.employeeList.AsEnumerable()
                    where e.state != "离职"
                    let age = Utility.CalYears(e.birthday, DateTime.Now)
                    let workAge = Utility.CalMonths(e.entryDate ?? DateTime.Now, DateTime.Now)
                    let resultScore = GetEmployeeLeaveDegree(e)
                    orderby resultScore descending
                    select new EmployeeLeaveRate()
                    {
                        EmployeeId = e.id,
                        Rate = resultScore
                    };

                or =  SaveLeaveRateToDb(result.ToList());

                Trace.WriteLine("<<< Exception Trace >>> Analyze One Time Finish");

                
                return or;
            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }
        }

        public async Task<OperateResult> AnalyzeOneTimeAsync()
        {
            return await Task.Run(() => AnalyzeOneTime());
        }

        public double GetEmployeeLeaveDegree(Employee e)
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


        #region 大数据预统计分析


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        OperateResult PreCalRelateCompanyPost()
        {
            try
            {
                var total = EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = EmployeeLeaveList.GroupBy(g => g.postId)
                    .Select(m => new { postId = m.Key, count = m.Count() })
                    .OrderByDescending(m => m.count);

                ListCompanyPost = new List<DimensionCompanyPost>();

                foreach (var item in dimensionGroup)
                {
                    count += item.count;

                    ListCompanyPost.Add(new DimensionCompanyPost
                    {
                        PostId = item.postId ?? 0,
                        Rate = (double)count/total
                    });
                }
                return new OperateResult
                {
                    status = OperateStatus.Success,
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        OperateResult PreCalRelateCompanyAge()
        {
            try
            {
                var total = EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = from e in EmployeeLeaveList
                    let age = Utility.CalYears(e.birthday, e.entryDate??DateTime.Now)
                    let groupAge = DimensionCompanyAge.Format(age)
                    group e by groupAge
                    into g
                    orderby g.Count() descending 
                    select new
                    {
                        age = g.Key,
                        count = g.Count()
                    };


                ListCompanyAge = new List<DimensionCompanyAge>();

                foreach (var item in dimensionGroup)
                {
                    count += item.count;

                    ListCompanyAge.Add(new DimensionCompanyAge
                    {
                        Age = item.age,
                        Rate = (double)count/total
                    });

                }
                return new OperateResult
                {
                    status = OperateStatus.Success,
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


        OperateResult PreCalRelateCompanyEducation()
        {
            try
            {

                var total = EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = EmployeeLeaveList.GroupBy(g => g.education)
                    .Select(m => new { education = m.Key, count = m.Count() })
                    .OrderByDescending(m => m.count);

                ListCompanyEducation = new List<DimensionCompanyEducation>();

                foreach (var item in dimensionGroup)
                {
                    count += item.count;

                    ListCompanyEducation.Add(new DimensionCompanyEducation
                    {
                        Education = item.education,
                        Rate = (double)count / total
                    });
                }
                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult PreCalRelateCompanyWorkAge()
        {
            try
            {

                var total = EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = from e in EmployeeLeaveList
                    let workAge = Utility.CalMonths(e.entryDate??DateTime.Now, e.entryDate??DateTime.Now)
                    let groupWorkAge = DimensionCompanyWorkAge.Format(workAge)
                    group e by groupWorkAge
                    into g
                    orderby g.Count() descending 
                    select new
                    {
                        workAge = g.Key,
                        count = g.Count()
                    };
                    

                ListCompanyWorkAge = new List<DimensionCompanyWorkAge>();

                foreach (var item in dimensionGroup)
                {
                    count += item.count;

                    ListCompanyWorkAge.Add(new DimensionCompanyWorkAge
                    {
                        WorkAge = item.workAge,
                        Rate = (double)count / total
                    });

                }
                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult PreCalRelateCompanySalary()
        {
            try
            {
                var total = EmployeeLeaveList.Count();
                var count = 0;

                var dimensionGroup = from e in EmployeeLeaveList
                    let salary = DimensionCompanySalary.Format(e.salary)
                    group e by salary
                    into g
                    orderby g.Count() descending 
                    select new
                    {
                        salary = g.Key,
                        count = g.Count()
                    };


                ListCompanySalary = new List<DimensionCompanySalary>();

                foreach (var item in dimensionGroup)
                {
                    count += item.count;

                    ListCompanySalary.Add(new DimensionCompanySalary
                    {
                        Salary = item.salary,
                        Rate = (double)count / total

                    });

                }
                return new OperateResult
                {
                    status = OperateStatus.Success,
                    data = true,
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

        #endregion

        #region 薪酬范畴预统计分析

        OperateResult PreCalSalaryRelateDepPost()
        {
            try
            {
                var month = DbCache.assessmentInfoList
                    .OrderByDescending(m => m.month)
                    .Select(m => m.month).FirstOrDefault();

                if (month == null)
                {
                    return new OperateResult
                    {
                        content = "无考核数据"
                    };
                }

                //得到最近一月部门内该岗位平均工资
                var ave = from e in DbCache.salaryRecordList.Include("assessmentInfo")
                    where e.assessmentInfo.month == month
                    group e by new
                    {
                        e.assessmentInfo.employee.departmentId,
                        e.assessmentInfo.employee.postId
                    }
                    into g
                    select new DimensionSalaryAveRelateDepPost
                    {
                        DepartmentId = g.Key.departmentId,
                        PostId = g.Key.postId ?? 0,
                        Salary = g.Average(m => m.actualTotal)
                    };

                ListSalaryAveDepPost = ave.ToList();

                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult PreCalSalaryRelateDep()
        {
            try
            {
                var month = DbCache.assessmentInfoList
                    .OrderByDescending(m => m.month)
                    .Select(m => m.month).FirstOrDefault();

                if (month == null)
                {
                    return new OperateResult
                    {
                        content = "无考核数据"
                    };
                }

                //得到最近一月部门内该岗位平均工资
                var ave = from e in DbCache.salaryRecordList.Include("assessmentInfo")
                    where e.assessmentInfo.month == month
                    group e by new
                    {
                        e.assessmentInfo.employee.departmentId,
                    }
                    into g
                    select new DimensionSalaryAveRelateDep
                    {
                        DepartmentId = g.Key.departmentId,
                        Salary = g.Average(m => m.actualTotal)
                    };

                ListSalaryAveDep = ave.ToList();

                return new OperateResult
                {
                    status = OperateStatus.Success,
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
        OperateResult PreCalSalaryRelatePost()
        {
            try
            {
                var month = DbCache.assessmentInfoList
                    .OrderByDescending(m => m.month)
                    .Select(m => m.month).FirstOrDefault();

                if (month == null)
                {
                    return new OperateResult
                    {
                        content = "无考核数据"
                    };
                }

                //得到最近一月部门内该岗位平均工资
                var ave = from e in DbCache.salaryRecordList.Include("assessmentInfo")
                    where e.assessmentInfo.month == month
                    group e by new
                    {
                        e.assessmentInfo.employee.postId
                    }
                    into g
                    select new DimensionSalaryAveRelatePost
                    {
                        PostId = g.Key.postId ?? 0,
                        Salary = g.Average(m => m.actualTotal)
                    };

                ListSalaryAvePost = ave.ToList();

                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult PreCalSalaryRelatePostLeave()
        {
            try
            {
                //得到最近一月部门内该岗位平均工资
                var ave = from e in EmployeeLeaveList
                    join employee in DbCache.employeeList on e.employeeId equals employee.id 
                    group e by new
                    {
                        employee.postId
                    }
                    into g
                    select new DimensionSalaryAveRelatePostLeave()
                    {
                        PostId = g.Key.postId ?? 0,
                        Salary = g.Average(m => m.salaryAverage)
                    };

                ListSalaryAvePostLeave = ave.ToList();

                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        #endregion

        #region 同岗位范畴预统计分析
        OperateResult PreCalPostRelateAge()
        {
            try
            {
                var posts = (from e in EmployeeLeaveList
                    select e.postId).Distinct();

                MapPostRelateAge = new Dictionary<long, List<DimensionPostRelateAge>>();

                foreach (var postId in posts)
                {
                    List<DimensionPostRelateAge> list = new List<DimensionPostRelateAge>();
                    MapPostRelateAge.Add(postId??0, list);

                    var dimensionGroup = (from e in EmployeeLeaveList
                        where e.postId == postId
                        let age = Utility.CalYears(e.birthday, e.entryDate??DateTime.Now)
                        let groupAge = DimensionPostRelateAge.Format(age)
                        group e by groupAge
                        into g
                        orderby g.Count() descending 
                        select new
                        {
                            age = g.Key,
                            count = g.Count()
                        }).ToList();

                    var total = dimensionGroup.Count();
                    double step = 1.0 / total;
                    double percent = 1.0;

                    foreach (var item in dimensionGroup)
                    {
                        list.Add(new DimensionPostRelateAge()
                        {
                            Age = item.age,
                            Rate = percent
                        });

                        percent -= step;
                    }
                }

                return new OperateResult
                {
                    status = OperateStatus.Success,
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


        OperateResult PreCalPostRelateWorkAge()
        {
            try
            {
                var posts = (from e in EmployeeLeaveList
                    select e.postId).Distinct();

                MapPostRelateWorkAge = new Dictionary<long, List<DimensionPostRelateWorkAge>>();

                foreach (var postId in posts)
                {
                    List<DimensionPostRelateWorkAge> list = new List<DimensionPostRelateWorkAge>();
                    MapPostRelateWorkAge.Add(postId??0, list);

                    var dimensionGroup = (from e in EmployeeLeaveList
                        where e.postId == postId
                        let age = Utility.CalMonths(e.birthday, e.entryDate??DateTime.Now)
                        let groupAge = DimensionPostRelateWorkAge.Format(age)
                        group e by groupAge
                        into g
                        orderby g.Count() descending 
                        select new
                        {
                            age = g.Key,
                            count = g.Count()
                        }).ToList();

                    var total = dimensionGroup.Count();
                    double step = 1.0 / total;
                    double percent = 1.0;

                    foreach (var item in dimensionGroup)
                    {
                        list.Add(new DimensionPostRelateWorkAge()
                        {
                            WorkAge = item.age,
                            Rate = percent
                        });
                        percent -= step;
                    }
                }

                return new OperateResult
                {
                    status = OperateStatus.Success,
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


        #endregion

        #region 考核范畴预统计分析

        OperateResult PreCalAssessmentRelateDep()
        {
            try
            {
                //得到最近一次部门内考核得分
                var month = DbCache.assessmentInfoList
                    .OrderByDescending(m => m.month)
                    .Select(m => m.month).FirstOrDefault();


                var ave = DbCache.assessmentInfoList
                    .Where(m => m.month==month)
                    .GroupBy(m => m.employee.departmentId)
                    .Select(m => new DimensionAssessmentAveRelateDep()
                    {
                        DepartmentId = m.Key, Score = m.Average(e=>e.performanceScore.Value)
                    });

                ListAssessmentAveDep = ave.ToList();

                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult PreCalAssessmentRelateCompany()
        {
            try
            {
                //得到最近一次部门内考核得分
                var month = DbCache.assessmentInfoList
                    .OrderByDescending(m => m.month)
                    .Select(m => m.month).FirstOrDefault();
                if (month == null)
                {
                    return new OperateResult
                    {
                        content = "无考核数据"
                    };
                }


                var ave = DbCache.assessmentInfoList
                    .Where(m => m.month == month)
                    .Select(m => m.performanceScore.Value)
                    .Average();

                AssessmentAveCompany = new DimensionAssessmentAveRelateCompany()
                {
                    Score = ave
                };

                return new OperateResult
                {
                    status = OperateStatus.Success,
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

        OperateResult PreCalAssessmentRelateLeave()
        {
            try
            {
                //得到最近一次部门内考核得分
                var month = DbCache.assessmentInfoList
                    .OrderByDescending(m => m.month)
                    .Select(m => m.month).FirstOrDefault();

                var ave = (from e in EmployeeLeaveList
                    join a in DbCache.assessmentInfoList
                        on e.employeeId equals a.employeeId
                    orderby a.month descending
                    group a by a.employeeId
                    into g
                    select g.First().performanceScore).Average();


                AssessmentAveLeave = new DimensionAssessmentAveRelateLeave()
                {
                    Score = ave.Value
                };

                return new OperateResult
                {
                    status = OperateStatus.Success,
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


        #endregion


        #endregion

        #region 大数据计算

        /// <summary>
        /// 通过历史返回计算多条件的最终匹配度
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public  double AnalyzeByBigData(Employee e)
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
        private  OperateResult CalPost(Employee employee)
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
                var rate = ListCompanyPost.Where(m => m.PostId == employee.postId)
                    .Select(m => m.Rate).FirstOrDefault();

                if (rate > LeaveLimitPercent)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true,
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
        private  OperateResult CalAge(Employee employee)
        {
            try
            {
                int age = Utility.CalYears(employee.birthday, DateTime.Now);
                age = DimensionCompanyAge.Format(age);

                var rate = ListCompanyAge.Where(m => m.Age == age)
                    .Select(m => m.Rate).FirstOrDefault();

                if (rate > LeaveLimitPercent)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true,
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
        private  OperateResult CalEducation(Employee employee)
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
                var rate = ListCompanyEducation.Where(m => m.Education == employee.education)
                    .Select(m => m.Rate).FirstOrDefault();

                if (rate > LeaveLimitPercent)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true,
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
        private  OperateResult CalWorkAge(Employee employee)
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
                int workAge = Utility.CalMonths(employee.entryDate??DateTime.Now, DateTime.Now);

                workAge = DimensionCompanyWorkAge.Format(workAge);

                var rate = ListCompanyWorkAge.Where(m => m.WorkAge == workAge)
                    .Select(m => m.Rate).FirstOrDefault();

                if (rate > LeaveLimitPercent)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true,
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
        private  OperateResult CalSalaryActualTotal(Employee employee)
        { 
            try
            {
                var salaryInfo = DbCache.salaryInfoList.Where(m => m.id == employee.salaryInfoId)
                    .Select(m => m).FirstOrDefault();
                if (salaryInfo == null)
                {
                    return new OperateResult
                    {
                        content = "无薪酬信息",
                    };
                }

                var actualTotal = salaryInfo.GetSalaryTotal();


                actualTotal = DimensionCompanySalary.Format(actualTotal);

                var rate = ListCompanySalary.Where(m => Math.Abs(m.Salary - actualTotal) < 0.01)
                    .Select(m => m.Rate).FirstOrDefault();

                if (rate > LeaveLimitPercent)
                {
                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = true,
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
            return new OperateResult
            {
                status = OperateStatus.Success,
                data = false,
            };
        }

        #endregion


        #region 薪酬相关计算

        public  double AnalyzeBySalary(Employee e)
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
        private  OperateResult CalSalaryInDepInPost(Employee employee)
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

                //得到最近一次该员工的工资数据
                var record = DbCache.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m=>m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                var salary = ListSalaryAveDepPost.Where(m => m.DepartmentId==employee.departmentId && m.PostId==employee.postId)
                    .Select(m => m.Salary).FirstOrDefault();

                if (record.actualTotal < salary)
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
        private  OperateResult CalSalaryInDep(Employee employee)
        {
            try
            {

                //得到最近一次该员工的工资数据
                var record = DbCache.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                var salary = ListSalaryAveDep.Where(m => m.DepartmentId == employee.departmentId)
                    .Select(m => m.Salary).FirstOrDefault();

                if (record.actualTotal < salary)
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
        private  OperateResult CalSalaryInPost(Employee employee)
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
                //得到最近一次该员工的工资数据
                var record = DbCache.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
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
                var salary = ListSalaryAvePost.Where(m => m.PostId == employee.postId)
                    .Select(m => m.Salary).FirstOrDefault();

                if (record.actualTotal < salary)
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
        private  OperateResult CalSalaryInHistoryPost(Employee employee)
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
                //得到最近一次该员工的工资数据
                var record = DbCache.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
                    .OrderByDescending(m => m.assessmentInfo.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在工资数据，无法就行分析",
                    };
                }

                var salary = ListSalaryAvePostLeave.Where(m => m.PostId == employee.postId)
                    .Select(m => m.Salary).FirstOrDefault();

                if (record.actualTotal < salary)
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
        private  OperateResult CalSalaryPersonal(Employee employee)
        {
            try
            {
                //得到最近一次该员工的工资数据
                var record = DbCache.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
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
                var ave = DbCache.salaryRecordList.Where(m => m.assessmentInfo.employeeId == employee.id)
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

        public  double AnalyzeByPostCrossAge(Employee e)
        {
            OperateResult or = CalPostCrossAge(e);
            if (or.status == OperateStatus.Success)
            {
                double rate = (double)or.data;

                return rate;
            }
            return 0;

        }

        public  double AnalyzeByPostCrossWorkAge(Employee e)
        {
            OperateResult or = CalPostCrossWorkAge(e);
            if (or.status == OperateStatus.Success)
            {
                double rate = (double)or.data;

                return rate;
            }
            return 0;

        }

        /// <summary>
        /// 岗位与年龄交叉计算
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private  OperateResult CalPostCrossAge(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            int employeeAge = Utility.CalYears(employee.birthday, DateTime.Now);
            employeeAge = DimensionPostRelateAge.Format(employeeAge);

            try
            {
                if (MapPostRelateAge.ContainsKey(employee.postId??0))
                {
                    var p = MapPostRelateAge[employee.postId ?? 0];

                    var rate = p.Where(m => m.Age == employeeAge)
                        .Select(m => m.Rate).FirstOrDefault();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = rate
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
        private  OperateResult CalPostCrossWorkAge(Employee employee)
        {
            if (employee.postInfo == null)
            {
                return new OperateResult
                {
                    content = "无岗位信息",
                };
            }

            int workAge = Utility.CalMonths(employee.entryDate??DateTime.Now, DateTime.Now);
            workAge = DimensionPostRelateWorkAge.Format(workAge);

            try
            { 
                if (MapPostRelateWorkAge.ContainsKey(employee.postId ?? 0))
                {
                    var p = MapPostRelateWorkAge[employee.postId ?? 0];

                    var rate = p.Where(m => m.WorkAge == workAge)
                        .Select(m => m.Rate).FirstOrDefault();

                    return new OperateResult
                    {
                        status = OperateStatus.Success,
                        data = rate
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

            return new OperateResult
            {
                status = OperateStatus.Error,
            };
        }

        #endregion



        #region 考核相关计算

        public  double AnalyzeByAssessment(Employee e)
        {
            int condition = 0;
            int count = 0;

            List<CalItem> calFuns = new List<CalItem>();
            calFuns.Add(CalAssessmentInDep);
            calFuns.Add(CalAssessmentInCompany);
            calFuns.Add(CalAssessmentInLeave);
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
        private  OperateResult CalAssessmentInDep(Employee employee)
        {
            try
            {

                //得到最近一次该员工的工资数据
                var record = DbCache.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在考核数据，无法就行分析",
                    };
                }

                var ave = ListAssessmentAveDep.Where(m => m.DepartmentId == employee.departmentId)
                    .Select(m => m.Score).FirstOrDefault();

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
        private  OperateResult CalAssessmentInCompany(Employee employee)
        {
            try
            {
                //得到最近一次该员工的工资数据
                var record = DbCache.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在考核数据，无法就行分析",
                    };
                }


                if (record.performanceScore < AssessmentAveCompany.Score )
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
        /// 当月低于离职员工当月考核得分
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private OperateResult CalAssessmentInLeave(Employee employee)
        {
            try
            {

                //得到最近一次该员工的工资数据
                var record = DbCache.assessmentInfoList.Where(m => m.employeeId == employee.id)
                    .OrderByDescending(m => m.month)
                    .Select(m => m).FirstOrDefault();

                if (record == null)
                {
                    return new OperateResult
                    {
                        content = "还未存在考核数据，无法就行分析",
                    };
                }

                if (record.performanceScore <= AssessmentAveLeave.Score)
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
        private OperateResult CalAssessmentInHistory(Employee employee)
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

                //得到上三次考核得分
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

    

    /*
    public class LeaveManager
    {
        public  double LeaveLimitPercent = 0.6;

        public delegate OperateResult CalItem(Employee e);
        public delegate double AnalyzeItem(Employee e);

        /// <summary>
        /// 员工离职后，把该员工数据信息录入离职数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  OperateResult Leave(Employee model)
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



        public  OperateResult RemoveAll()
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


        public  OperateResult LeaveWarningByPager(QueryParam param = null)
        {
            try
            {
                SystemDB db = new SystemDB();

                var elements = db.employeeList.Include("department").Where(m => m.state != "离职")
                    .Select(m => m);

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

        public  double GetEmployeeLeaveDegree(Employee e)
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
        public  double AnalyzeByBigData(Employee e)
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
        private  OperateResult CalPost(Employee employee)
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
        private  OperateResult CalAge(Employee employee)
        {
            try
            {
                SystemDB dbLeave = new SystemDB();

                var total = dbLeave.EmployeeLeaveList.Count();
                var count = 0;

                int age = Model.Utility.Utility.CalYears(employee.birthday, DateTime.Now);

                var dimensionGroup = dbLeave.EmployeeLeaveList.AsEnumerable().GroupBy(g => Model.Utility.Utility.CalYears(g.birthday, g.entryDate??DateTime.Now))
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
        private  OperateResult CalEducation(Employee employee)
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
        private  OperateResult CalWorkAge(Employee employee)
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
                int workAge = Model.Utility.Utility.CalMonths(employee.entryDate??DateTime.Now, DateTime.Now);

                var dimensionGroup = dbLeave.EmployeeLeaveList.AsEnumerable().GroupBy(g => Model.Utility.Utility.CalMonths(g.entryDate??DateTime.Now, g.entryDate??DateTime.Now))
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
        private  OperateResult CalSalaryActualTotal(Employee employee)
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

        public  double AnalyzeBySalary(Employee e)
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
        private  OperateResult CalSalaryInDepInPost(Employee employee)
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
        private  OperateResult CalSalaryInDep(Employee employee)
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
        private  OperateResult CalSalaryInPost(Employee employee)
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
        private  OperateResult CalSalaryInHistoryPost(Employee employee)
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
        private  OperateResult CalSalaryPersonal(Employee employee)
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

        public  double AnalyzeByPostCrossAge(Employee e)
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

        public  double AnalyzeByPostCrossWorkAge(Employee e)
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
        private  OperateResult CalPostCrossAge(Employee employee)
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
                        let age = Model.Utility.Utility.CalYears(e.birthday, e.entryDate??DateTime.Now)
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
        private  OperateResult CalPostCrossWorkAge(Employee employee)
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

            int employeeAge = Model.Utility.Utility.CalMonths(employee.entryDate??DateTime.Now, DateTime.Now);

            try
            {
                var db = new SystemDB();

                foreach (var stageItem in liststageItems)
                {
                    var count = (from e in db.EmployeeLeaveList.AsEnumerable()
                                 where e.postId == employee.id
                                 let age = Model.Utility.Utility.CalMonths(e.entryDate??DateTime.Now, e.entryDate??DateTime.Now)
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

        public  double AnalyzeByAssessment(Employee e)
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
        private  OperateResult CalAssessmentInDep(Employee employee)
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
        private  OperateResult CalAssessmentInCompany(Employee employee)
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
        private  OperateResult CalAssessmentInHistory(Employee employee)
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

    */
}
