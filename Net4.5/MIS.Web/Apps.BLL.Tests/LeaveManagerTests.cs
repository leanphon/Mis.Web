using Microsoft.VisualStudio.TestTools.UnitTesting;
using Apps.BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Apps.Model;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Apps.BLL.Tests
{
    [TestClass()]
    public class LeaveManagerTests : IDisposable
    {
        public readonly int DepartmentStubSize = 10;
        public readonly int PostStubSize = 10;
        public readonly int LevelStubSize = 10;
        public readonly int BenefitStubSize = 10;
        public readonly int PerformanceStubSize = 10;
        public readonly int EmployeeStubSize = 10;

        public LeaveManagerTests()
        {
            SystemDB db = new SystemDB();

            Dispose();

        }

        public bool DepartmentAddStub()
        {
            for (var i = 0; i < DepartmentStubSize; i++)
            {
                Department e = new Department();

                e.name = "Department-" + i;
                e.code = "Dep-" + i;

                OperateResult or = DepartmentManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }

            return true;
        }

        public bool PostAddStub()
        {
            for (var i = 0; i < PostStubSize; i++)
            {
                PostInfo e = new PostInfo();

                e.name = "Post-" + i;
                OperateResult or = PostManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }

            return true;
        }

        public bool LevelAddStub()
        {
            for (var i = 0; i < LevelStubSize; i++)
            {
                LevelInfo e = new LevelInfo();

                e.name = "LevelInfo-" + i;
                e.code = "Level-" + i;
                e.levelSalary = 2000 + i * 500;
                e.fullAttendanceRewards = 100+ i * 50;
                e.seniorityRewardsBase = 200;

                OperateResult or = LevelManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }
            return true;
        }

        public bool PerformanceAddStub()
        {
            
            for (var i = 0; i < PerformanceStubSize; i++)
            {
                PerformanceInfo e = new PerformanceInfo();

                e.code = "P-" + i;
                e.performanceRewards = 2000 + i * 500;
                OperateResult or = PerformanceManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }

            return true;
        }
        public bool BenefitAddStub()
        {
            for (var i = 0; i < BenefitStubSize; i++)
            {
                BenefitInfo e = new BenefitInfo();

                e.code = "B-" + i;
                e.benefitRewards = 2000 + i * 500;

                OperateResult or = BenefitManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
					StackTrace st = new StackTrace(new StackFrame(true)); 
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }

            return true;
        }

        public string GetEducationFromRandom()
        {
            Random random = new Random();

            int r = random.Next(1, 9);

            switch (r)
            {
                case 1:
                    return "小学";
                case 2:
                    return "初中";
                case 3:
                    return "中专";
                case 4:
                    return "高中";
                case 5:
                    return "专科";
                case 6:
                    return "本科";
                case 7:
                    return "硕士";
                case 8:
                    return "博士";

            }

            return "";
        }

        public DateTime GetTimeFromRandom(DateTime baseTime)
        {
            Random random = new Random();
            int year = baseTime.Year + random.Next(1, 14);
            int month = baseTime.Month + random.Next(1, 12);
            month = month > 11 ? 11 : month;
            int day = random.Next(1, 31);


            return new DateTime(year, month, day);
        }

        //public bool EmployeeEntryStub()
        //{
        //    Random random = new Random();

        //    for (var i = 0; i < EmployeeStubSize; i++)
        //    {
        //        Employee e = new Employee();

        //        e.name = "Employee-" + i;
        //        e.departmentId = random.Next(1, DepartmentStubSize + 1);
        //        e.number = "ENO-" + i;
        //        e.postId = random.Next(1, PostStubSize + 1);
        //        e.idCard = "45010719880202131" + random.Next(1, 10);
        //        e.sex = random.Next(1, 3) == 1 ? "男" : "女";
        //        e.education = GetEducationFromRandom();
        //        e.state = "试用期";
        //        e.phone = "1397710543" + i;
        //        e.experience = i / 10;

        //        e.salaryInfo = new SalaryInfo()
        //        {
        //            levelId = random.Next(1, LevelStubSize + 1),
        //            performanceId = random.Next(1, PerformanceStubSize + 1),
        //            benefitId = random.Next(1, BenefitStubSize + 1),
        //        };

        //        e.birthday = GetTimeFromRandom(new DateTime(1970, 1 ,1));
        //        e.entryDate = GetTimeFromRandom(new DateTime(2005, 1, 1));
        //        e.formalDate = GetTimeFromRandom(e.entryDate.Value.AddMonths(3));
        //        e.contractBegin = GetTimeFromRandom(new DateTime(2017, 1, 1));
        //        e.contractEnd = GetTimeFromRandom(e.contractBegin.Value.AddMonths(12));

        //        OperateResult or = EmployeeManager.Add(e);
        //        if (or.status == OperateStatus.Error)
        //        {
        //            StackTrace st = new StackTrace(new StackFrame(true));
        //        string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
        //                     $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
        //        Trace.WriteLine(msg);
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public bool EmployeeEntryStub()
        {
            Random random = new Random();

            int i = 1;
            int departmentId = 1;
            int postId = 1;

            #region 添加员工入职，部门1、岗位1，层级1

            Employee e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 1,
                performanceId = 1,
                benefitId = 1,
            };

            e.birthday = new DateTime(1970, 1, 1);
            e.entryDate = new DateTime(2005, 1, 1);
            e.formalDate = new DateTime(2005, 4, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            OperateResult or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门1、岗位1，层级2

            i = 2;
            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 2,
                performanceId = 2,
                benefitId = 2,
            };

            e.birthday = new DateTime(1970, 1, 1);
            e.entryDate = new DateTime(2005, 1, 1);
            e.formalDate = new DateTime(2005, 4, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门1、岗位1，层级3

            i = 3;
            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 3,
                performanceId = 3,
                benefitId = 3,
            };

            e.birthday = new DateTime(1970, 1, 1);
            e.entryDate = new DateTime(2005, 1, 1);
            e.formalDate = new DateTime(2005, 4, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion


            #region 添加员工入职，部门2、岗位1，层级1

            i = 4;
            departmentId = 2;
            postId = 1;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 1,
                performanceId = 1,
                benefitId = 1,
            };

            e.birthday = new DateTime(1970, 1, 1);
            e.entryDate = new DateTime(2014, 4, 1);
            e.formalDate = new DateTime(2014, 7, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门2、岗位1，层级2

            i = 5;
            departmentId = 2;
            postId = 1;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 2,
                performanceId = 2,
                benefitId = 2,
            };

            e.birthday = new DateTime(1970, 1, 1);
            e.entryDate = new DateTime(2014, 4, 1);
            e.formalDate = new DateTime(2014, 7, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门2、岗位1，层级3

            i = 6;
            departmentId = 2;
            postId = 1;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 3,
                performanceId = 3,
                benefitId = 3,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2014, 4, 1);
            e.formalDate = new DateTime(2014, 7, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion


            #region 添加员工入职，部门1、岗位2，层级1

            i = 7;
            departmentId = 1;
            postId = 2;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 1,
                performanceId = 1,
                benefitId = 1,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2015, 6, 1);
            e.formalDate = new DateTime(2015, 9, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门1、岗位2，层级2

            i = 8;
            departmentId = 1;
            postId = 2;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 2,
                performanceId = 2,
                benefitId = 2,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2015, 6, 1);
            e.formalDate = new DateTime(2015, 9, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门1、岗位2，层级3

            i = 9;
            departmentId = 1;
            postId = 2;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 3,
                performanceId = 3,
                benefitId = 3,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2015, 6, 1);
            e.formalDate = new DateTime(2015, 9, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion


            #region 添加员工入职，部门2、岗位2，层级1

            i = 10;
            departmentId = 2;
            postId = 2;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 1,
                performanceId = 1,
                benefitId = 1,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2013, 10, 1);
            e.formalDate = new DateTime(2013, 12, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门2、岗位2，层级2

            i = 11;
            departmentId = 2;
            postId = 2;

            e = new Employee();
            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 2,
                performanceId = 2,
                benefitId = 2,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2013, 10, 1);
            e.formalDate = new DateTime(2013, 12, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门2、岗位2，层级3

            i = 12;
            departmentId = 2;
            postId = 2;

            e = new Employee();

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 3,
                performanceId = 3,
                benefitId = 3,
            };

            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2013, 10, 1);
            e.formalDate = new DateTime(2013, 12, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);



            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;

            
            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion


            #region 添加员工入职，部门1、岗位1，层级3

            i = 13;
            departmentId = 1;
            postId = 1;

            e = new Employee();

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 3,
                performanceId = 3,
                benefitId = 3,
            };
            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2013, 10, 1);
            e.formalDate = new DateTime(2013, 12, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;


            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion

            #region 添加员工入职，部门2、岗位2，层级3

            i = 14;
            departmentId = 2;
            postId = 2;

            e = new Employee();

            e.salaryInfo = new SalaryInfo()
            {
                levelId = 3,
                performanceId = 3,
                benefitId = 3,
            };
            e.birthday = new DateTime(1988, 1, 1);
            e.entryDate = new DateTime(2013, 10, 1);
            e.formalDate = new DateTime(2013, 12, 1);
            e.contractBegin = new DateTime(2017, 1, 1);
            e.contractEnd = new DateTime(2019, 12, 1);

            e.name = "Employee-" + i;
            e.departmentId = departmentId;
            e.number = "ENO-" + i;
            e.postId = postId;
            e.idCard = "45010719880202131" + random.Next(1, 100);
            e.sex = random.Next(1, 3) == 1 ? "男" : "女";
            e.education = "高中";
            e.state = "试用期";
            e.phone = "1397710543" + i;
            e.experience = i / 10;


            or = EmployeeManager.Add(e);
            if (or.status == OperateStatus.Error)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                return false;
            }

            #endregion


            return true;
        }

        public bool EmployeeLeaveStub(int employeeId, DateTime time)
        {
            var or = EmployeeManager.GetById(employeeId);
            if (or.status == OperateStatus.Error  || or.data==null)
            {
                return false;
            }

            or = EmployeeManager.Leave(employeeId, "离职", time);
            if (or.status == OperateStatus.Error )
            {
                return false;
            }

            return true;
        }
        public bool EmployeeAssessmentStub(string month)
        {
            Random random = new Random();

            QueryParam queryParam = new QueryParam
            {
                pager = new Pager
                {
                    page = 1,
                    rows = EmployeeStubSize

                }
            };

            FilterModel filter = new FilterModel
            {
                key = "month",
                value = month
            };
            Dictionary<string, FilterModel> filterSet = new Dictionary<string, FilterModel>();
            filterSet.Add(filter.key, filter);
            queryParam.filters = filterSet;

            var or = AssessmentManager.GetEmployeesAll(queryParam);
            if (or.status == OperateStatus.Error)
            {
                return false;
            }

            dynamic listAssessment = or.data;

            foreach (dynamic ass in listAssessment)
            {
                AssessmentInfo e = new AssessmentInfo();

                PropertyInfo[] pArray = ass.GetType().GetProperties();
                Type t = ass.GetType();
                
                e.billSerial = Convert.ToString(t.GetProperty("billSerial").GetValue(ass));
                e.billSerial = t.GetProperty("billSerial").GetValue(ass);
                e.employeeId = t.GetProperty("employeeId").GetValue(ass);
                e.month = month;
                e.benefitScore = random.Next(40, 100);
                e.performanceScore = random.Next(40, 100);
                e.shouldWorkTime = 22;
                e.actualWorkTime = 22;

                or = AssessmentManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }

            return true;
        }
        public bool EmployeeSalaryInputStub(string month)
        {
            Random random = new Random();
            
            QueryParam queryParam = new QueryParam {
                pager = new Pager
                {
                    page = 1,
                    rows = EmployeeStubSize

                }
            };

            FilterModel filter = new FilterModel
            {
                key = "month",
                value = month
            };
            Dictionary<string, FilterModel> filterSet = new Dictionary<string, FilterModel>();
            filterSet.Add(filter.key, filter);
            queryParam.filters = filterSet;

            var or = SalaryRecordManager.GetAssessmentAll(queryParam);
            if (or.status == OperateStatus.Error)
            {
                return false;
            }

            dynamic listAssessment = or.data;
            foreach (var ass in listAssessment)
            {
                SalaryRecord e = new SalaryRecord();

                PropertyInfo[] pArray = ass.GetType().GetProperties();
                Type t = ass.GetType();

                //e.billSerial = Convert.ToString();
                //e.employeeId = Convert.ToInt64(t.GetProperty("employeeId").GetValue(ass));


                e.billSerial = t.GetProperty("billSerial").GetValue(ass);
                e.assessmentInfoId = t.GetProperty("assessmentInfoId").GetValue(ass);
                e.levelSalary = t.GetProperty("levelSalary").GetValue(ass);
                e.fullAttendanceRewards = t.GetProperty("fullAttendanceRewards").GetValue(ass);
                e.performanceRewards = t.GetProperty("performanceRewards").GetValue(ass);
                e.benefitRewards = t.GetProperty("benefitRewards").GetValue(ass); 
                e.seniorityRewards = t.GetProperty("seniorityRewards").GetValue(ass); 
                e.normalOvertimeRewards = t.GetProperty("normalOvertimeRewards").GetValue(ass); 
                e.holidayOvertimeRewards = t.GetProperty("holidayOvertimeRewards").GetValue(ass); 
                e.shouldTotal = t.GetProperty("shouldTotal").GetValue(ass);
                e.actualTotal = t.GetProperty("actualTotal").GetValue(ass);

                or = SalaryRecordManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    StackTrace st = new StackTrace(new StackFrame(true));
                string msg = $"App Trace >>> in file: {st.GetFrame(0).GetFileName()} " +
                             $"line {st.GetFrame(0).GetFileLineNumber()} message： {or.content}";
                Trace.WriteLine(msg);
                    return false;
                }
            }

            return true;
        }


        public void LeaveWarningStub()
        {
            //打桩：基础数据
            Assert.AreEqual(true, DepartmentAddStub());
            Assert.AreEqual(true, PostAddStub());
            Assert.AreEqual(true, LevelAddStub());
            Assert.AreEqual(true, PerformanceAddStub());
            Assert.AreEqual(true, BenefitAddStub());


            //打桩：入职
            Assert.AreEqual(true, EmployeeEntryStub());

            //打桩：考核数据
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-7"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-7"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-8"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-8"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-9"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-9"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-10"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-10"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-11"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-11"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-12"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-12"));

            }

        public void OutputResult(OperateResult or)
        {
            dynamic pager = or.data;
            Type tp = pager.GetType();
            PropertyInfo[] ps = tp.GetType().GetProperties();

            dynamic listLeaveWarning = tp.GetProperty("rows").GetValue(pager);

            foreach (var item in listLeaveWarning)
            {
                SalaryRecord e = new SalaryRecord();

                Type t = item.GetType();
                PropertyInfo[] pArray = t.GetProperties();
                
                long id = t.GetProperty("id").GetValue(item);
                string name = t.GetProperty("name").GetValue(item);
                string number = t.GetProperty("number").GetValue(item);
                string departmentName = t.GetProperty("departmentName").GetValue(item);
                string postName = t.GetProperty("postName").GetValue(item);
                int age = t.GetProperty("age").GetValue(item);
                int workAge = t.GetProperty("workAge").GetValue(item);
                string resultScore = t.GetProperty("resultScore").GetValue(item);


                var content =
                    $"id: {id}, name:{name}, number:{number}, departmentName:{departmentName}, postName:{postName}, age:{age}, workAge:{workAge}, resultScore:{resultScore}";

                Trace.WriteLine("App Trace Case Result >>> " + content);

            }
        }

        [TestMethod()]
        public void LeaveWarningTest1()
        {
            //打桩
            LeaveWarningStub();
            // 打桩：先离职
            Assert.AreEqual(EmployeeLeaveStub(1, new DateTime(2010, 5, 1)), true);
            Assert.AreEqual(EmployeeLeaveStub(2, new DateTime(2010, 5, 10)), true);
            Assert.AreEqual(EmployeeLeaveStub(3, new DateTime(2010, 5, 12)), true);


            var or = LeaveManager.LeaveWarningByPager(new QueryParam());
            Assert.AreEqual(or.status, OperateStatus.Success);

            OutputResult(or);

            Assert.AreEqual(EmployeeLeaveStub(4, new DateTime(2018, 9, 1)), true);
            Assert.AreEqual(EmployeeLeaveStub(5, new DateTime(2018, 9, 1)), true);
            Assert.AreEqual(EmployeeLeaveStub(6, new DateTime(2018, 9, 1)), true);


            or = LeaveManager.LeaveWarningByPager(new QueryParam());
            Assert.AreEqual(or.status, OperateStatus.Success);

            OutputResult(or);

        }


        public void Dispose()
        {
            SalaryRecordManager.RemoveAll();
            AssessmentManager.RemoveAll();
            EmployeeManager.RemoveAll();
            DepartmentManager.RemoveAll();

            PostManager.RemoveAll();
            LevelManager.RemoveAll();
            PerformanceManager.RemoveAll();
            BenefitManager.RemoveAll();
            try
            {
                FileInfo file = new FileInfo("../../TestUnint.sql");
                string script = file.OpenText().ReadToEnd();

                SystemDB db = new SystemDB();

                db.Database.ExecuteSqlCommand(script);
            }
            catch (Exception e)
            {
                Trace.WriteLine(Model.Utility.Utility.GetExceptionMsg(e));
            }
            
        }
    }
}