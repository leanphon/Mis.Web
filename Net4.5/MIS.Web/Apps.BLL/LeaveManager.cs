using Apps.Model;
using Apps.Model.Leave;
using Apps.Model.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.BLL
{

    public class Dimension
    {
        public string dimension { get; set; }
        public double score { get; set; }
        public double value { get; set; }
        public double average { get; set; }

    }

    public class LeaveManager
    {
        public static OperateResult Leave(Employee model)
        {
            try
            {
                using (DbHelperLeave db = new DbHelperLeave())
                {
                    ///// 离职记录基数表
                    //#region 
                    //if (model.address != null)
                    //{
                    //    LeaveRecord m = new LeaveRecord
                    //    {
                    //        employeeId = model.id,
                    //    };
                    //    db.leaveRecordList.Add(m);
                    //}
                    //#endregion

                    ///薪资
                    #region 
                    using (SystemDB dbBase = new SystemDB())
                    {
                        var salary = (from e in dbBase.salaryInfoList
                                      where e.id == model.salaryInfoId
                                      select e).FirstOrDefault();

                        LeaveSalary m = new LeaveSalary
                        {
                            employeeId = model.id,
                            salary = salary.GetSalaryTotal()
                        };
                        db.salaryList.Add(m);

                    }

                    if (model.address != null)
                    {
                        LeaveAddress m = new LeaveAddress
                        {
                            employeeId = model.id,
                            address = model.address,
                        };

                        CompanyManager manger = new CompanyManager();
                        OperateResult or = manger.GetFirst();
                        if (or.data != null)
                        {
                            Company c = or.data as Company;
                            m.toCompanyDistance = MapHelper.GetTowPointDistance(model.address, c.address);

                            db.addressList.Add(m);

                        }
                    }
                    #endregion


                    ///居住地
                    #region 
                    if (model.address != null)
                    {
                        LeaveAddress m = new LeaveAddress
                        {
                            employeeId = model.id,
                            address = model.address,
                        };

                        CompanyManager manger = new CompanyManager();
                        OperateResult or = manger.GetFirst();
                        if (or.data != null)
                        {
                            Company c = or.data as Company;
                            m.toCompanyDistance = MapHelper.GetTowPointDistance(model.address, c.address);

                            db.addressList.Add(m);

                        }
                    }
                    #endregion

                    ///年龄
                    #region 
                    if (model.birthday != null)
                    {
                        LeaveAge m = new LeaveAge
                        {
                            employeeId = model.id,
                            age = Model.Utility.Utility.CalYears(model.birthday, DateTime.Now),
                        };
                        db.ageList.Add(m);

                    }
                    #endregion

                    ///部门
                    #region 
                    LeaveDepartment department = new LeaveDepartment
                    {
                        employeeId = model.id,
                        departmentId = model.departmentId,
                    };
                    db.departmentList.Add(department);
                    #endregion

                    ///学历
                    #region 
                    if (model.education != null)
                    {
                        LeaveEducation m = new LeaveEducation
                        {
                            employeeId = model.id,
                            education = model.education,
                        };
                        db.educationList.Add(m);

                    }
                    #endregion

                    ///学历
                    #region 
                    if (model.education != null)
                    {
                        LeaveEducation m = new LeaveEducation
                        {
                            employeeId = model.id,
                            education = model.education,
                        };
                        db.educationList.Add(m);

                    }
                    #endregion

                    ///工作年限
                    #region 
                    LeaveExperience experience = new LeaveExperience
                    {
                        employeeId = model.id,
                        experience = model.experience != null ? model.experience.Value : 0
                    };
                    if (model.entryDate != null && model.leaveDate != null)
                    {
                        experience.experience += Model.Utility.Utility.CalYears(model.entryDate.Value, model.leaveDate.Value);
                    }


                    db.experienceList.Add(experience);

                    #endregion

                    ///婚姻状况
                    #region 
                    if (model.marriage != null)
                    {
                        LeaveMarriage m = new LeaveMarriage
                        {
                            employeeId = model.id,
                            marriage = model.marriage,
                        };
                        db.marriageList.Add(m);

                    }
                    #endregion

                    ///民族
                    #region 
                    if (model.nation != null)
                    {
                        LeaveNation m = new LeaveNation
                        {
                            employeeId = model.id,
                            nation = model.nation,
                        };
                        db.nationList.Add(m);

                    }
                    #endregion

                    ///籍贯
                    #region 
                    if (model.nativePlace != null)
                    {
                        LeaveNativePlace m = new LeaveNativePlace
                        {
                            employeeId = model.id,
                            nativePlace = model.nativePlace,
                        };
                        db.nativePlaceList.Add(m);

                    }
                    #endregion

                    ///政治面貌
                    #region 
                    if (model.political != null)
                    {
                        LeavePolitical m = new LeavePolitical
                        {
                            employeeId = model.id,
                            political = model.political,
                        };
                        db.politicalList.Add(m);

                    }
                    #endregion

                    ///性别
                    #region 
                    if (model.sex != null)
                    {
                        LeaveSex m = new LeaveSex
                        {
                            employeeId = model.id,
                            sex = model.sex,
                        };
                        db.sexList.Add(m);

                    }
                    #endregion

                    ///人才来源
                    #region 
                    if (model.source != null)
                    {
                        LeaveSource m = new LeaveSource
                        {
                            employeeId = model.id,
                            source = model.source,
                        };
                        db.sourceList.Add(m);

                    }
                    #endregion

                    ///司龄
                    #region 
                    if (model.entryDate != null && model.leaveDate != null)
                    {
                        LeaveWorkAge m = new LeaveWorkAge
                        {
                            employeeId = model.id,
                            workAge = Model.Utility.Utility.CalYears(model.entryDate.Value, model.leaveDate.Value),
                        };

                        db.workAgeList.Add(m);

                    }
                    #endregion

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


        /// <summary>
        /// 以给定样本的[最小值,平均值]，[平均值,最大值]作为上下两个区间各划分50个刻度（50分）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="ave"></param>
        /// <param name="val"></param>
        /// <returns>分数</returns>
        private static double CalAddressScore(double min, double max, double ave, double val)
        {
            if (max == ave) //单个样本，或者所有样本相同
            {
                if (val > max)
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                double score = 0;
                if (val > ave)
                {
                    score = 50 + (val - ave) / (max - ave) * 50;
                }
                else
                {
                    score = (ave - val) / (ave - min) * 50;
                }
                if (score > 100)
                {
                    score = 100;
                }
                return score;
            }
        }

        private static double CalAgeScore(long min, long max, double ave, int val)
        {
            if (max == ave) //单个样本，或者所有样本相同
            {
                if (val == max)
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                double score = 0;
                if (val > ave)
                {
                    score = 100 - (val - ave) / (max - ave) * 100;
                }
                else
                {
                    score = 100 - (ave - val) / (ave - min) * 50;
                }
                if (score > 100)
                {
                    score = 100;
                }
                return score;
            }
        }

        private static double CalSalaryScore(double min, double max, double ave, double val)
        {
            if (max == ave) //单个样本，或者所有样本相同
            {
                if (val == max)
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                double score = 0;
                if (val > ave)
                {
                    score = 50 - (val - ave) / (max - ave) * 50;
                }
                else
                {
                    score = 50 + (ave - val) / (ave - min) * 50;
                }
                if (score > 100)
                {
                    score = 100;
                }
                return score;
            }
        }

        public static OperateResult LeaveWarning(QueryParam param = null)
        {
            try
            {
                DbHelperLeave dbLeave = new DbHelperLeave();
                SystemDB db = new SystemDB();

                /// 计算历史库样本值
                #region

                // 到公司距离
                #region
                double min = 0;
                double max = 0;
                double ave = 0;
                try
                {
                    min = (from e in dbLeave.addressList.AsEnumerable()
                           let dis = e.toCompanyDistance
                           select e.toCompanyDistance).Min();
                    max = (from e in dbLeave.addressList
                           select e.toCompanyDistance).Max();
                    ave = (from e in dbLeave.addressList
                           select e.toCompanyDistance).Average();

                }
                catch (Exception)
                {
                }


                var addressSample = new
                {
                    min,
                    max,
                    ave
                };

                #endregion

                // 年龄
                #region
                long ageMin = 0;
                long ageMax = 0;
                double ageAve = 0;

                try
                {
                    ageMin = (from e in dbLeave.ageList
                              select e.age).Min();
                    ageMax = (from e in dbLeave.ageList
                              select e.age).Max();
                    ageAve = (from e in dbLeave.ageList
                              select e.age).Average();

                }
                catch (Exception )
                {
                }


                var ageSample = new
                {
                    min = ageMin,
                    max = ageMax,
                    ave = ageAve
                };

                #endregion


                // 工资
                #region

                double salaryMin = 0;
                double salaryMax = 0;
                double salaryAve = 0;

                try
                {
                    salaryMin = (from e in dbLeave.salaryList
                                 select e.salary).Min();
                    salaryMax = (from e in dbLeave.salaryList
                                 select e.salary).Max();
                    salaryAve = (from e in dbLeave.salaryList
                                 select e.salary).Average();
                }
                catch (Exception )
                {
                }

                var salarySample = new
                {
                    min = salaryMin,
                    max = salaryMax,
                    ave = salaryAve
                };

                #endregion



                #endregion


                /// 预警计算
                #region
                var company = (from e in db.companyList
                               select e).FirstOrDefault();

                var elements = from e in db.employeeList.Include("department").AsEnumerable()
                               join s in db.salaryInfoList on e.salaryInfoId equals s.id
                               let salary = s.GetSalaryTotal()
                               let age = Model.Utility.Utility.CalYears(e.birthday, DateTime.Now)
                               let distance = Model.Utility.MapHelper.GetTowPointDistance(company.address, e.address)
                               let workAge = Model.Utility.Utility.CalYears(e.entryDate != null ? e.entryDate.Value : DateTime.Now, DateTime.Now)

                               where e.state != "离职"
                               select new
                               {
                                   e.id,
                                   e.number,
                                   e.name,
                                   e.departmentId,
                                   departmentName = e.department.name,

                                   distance,
                                   age,
                                   e.education,
                                   e.experience,
                                   e.marriage,
                                   e.nation,
                                   e.nativePlace,
                                   e.political,
                                   salary,
                                   e.sex,
                                   e.source,
                                   workAge
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

                List<object> data = new List<object>();

                foreach (var e in elements)
                {
                    List<Dimension> dimensions = new List<Dimension>();

                    var addressScore = CalAddressScore(addressSample.min, addressSample.max, addressSample.ave, e.distance);
                    dimensions.Add(new Dimension
                    {
                        dimension = "居住地到公司距离",
                        score = addressScore,
                        value = e.distance,
                        average = addressSample.ave
                    });

                    var ageScore = CalAgeScore(ageSample.min, ageSample.max, ageSample.ave, e.age);
                    dimensions.Add(new Dimension
                    {
                        dimension = "年龄",
                        score = ageScore,
                        value = e.age,
                        average = ageSample.ave
                    });

                    var salayScore = CalSalaryScore(salarySample.min, salarySample.min, salarySample.ave, e.salary);
                    dimensions.Add(new Dimension
                    {
                        dimension = "薪资",
                        score = salayScore,
                        value = e.salary,
                        average = salarySample.ave
                    });
                    var resultScore = (from d in dimensions
                                       select d.score).Average();

                    data.Add(new
                    {
                        e.id,
                        e.number,
                        e.name,
                        e.departmentName,
                        resultScore,
                        dimensions
                    });

                }


                #endregion


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


    }
}
