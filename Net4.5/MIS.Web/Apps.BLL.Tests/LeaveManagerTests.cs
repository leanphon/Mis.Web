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

            db.Database.CreateIfNotExists();

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
                    Trace.WriteLine("App Trace >>> " + or.content);
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
                    Trace.WriteLine("App Trace >>> " + or.content);
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
                    Trace.WriteLine("App Trace >>> " + or.content);
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
                    Trace.WriteLine("App Trace >>> " + or.content);
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
                    Trace.WriteLine("App Trace >>> " + or.content);
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

        public bool EmployeeEntryStub()
        {
            Random random = new Random();
            
            for (var i = 0; i < EmployeeStubSize; i++)
            {
                Employee e = new Employee();

                e.name = "Employee-" + i;
                e.departmentId = random.Next(1, DepartmentStubSize + 1);
                e.number = "Employee-" + i;
                e.postId = random.Next(1, PostStubSize + 1);
                e.idCard = "45010719880202131" + random.Next(1, 10);
                e.sex = random.Next(1, 3) == 1 ? "男" : "女";
                e.education = GetEducationFromRandom();
                e.state = "试用期";
                e.phone = "1397710543" + i;
                e.experience = i / 10;

                e.salaryInfo = new SalaryInfo()
                {
                    levelId = random.Next(1, LevelStubSize + 1),
                    performanceId = random.Next(1, PerformanceStubSize + 1),
                    benefitId = random.Next(1, BenefitStubSize + 1),
                };

                e.birthday = GetTimeFromRandom(new DateTime(1970, 1 ,1));
                e.entryDate = GetTimeFromRandom(new DateTime(2005, 1, 1));
                e.formalDate = GetTimeFromRandom(e.entryDate.Value.AddMonths(3));
                e.contractBegin = GetTimeFromRandom(new DateTime(2017, 1, 1));
                e.contractEnd = GetTimeFromRandom(e.contractBegin.Value.AddMonths(12));

                OperateResult or = EmployeeManager.Add(e);
                if (or.status == OperateStatus.Error)
                {
                    Trace.WriteLine("App Trace >>> " + or.content);
                    return false;
                }
            }

            return true;
        }
        public bool EmployeeLeaveStub(int employeeId)
        {
            var or = EmployeeManager.GetById(employeeId);
            if (or.status == OperateStatus.Error  || or.data==null)
            {
                return false;
            }

            var time = GetTimeFromRandom(new DateTime(2018, 1, 1));

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
                    Trace.WriteLine("App Trace >>> " + or.content);
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
                    Trace.WriteLine("App Trace >>> " + or.content);
                    return false;
                }
            }

            return true;
        }

        [TestMethod()]
        public void LeaveWarningTest()
        {
            //打桩：基础数据
            Assert.AreEqual(true, DepartmentAddStub());
            Assert.AreEqual(true, PostAddStub());
            Assert.AreEqual(true, LevelAddStub());
            Assert.AreEqual(true, PerformanceAddStub());
            Assert.AreEqual(true, BenefitAddStub());


            //打桩：在职员工，离职员工、考核数据
            Assert.AreEqual(true, EmployeeEntryStub());
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-7")) ;
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-7"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-8")) ;
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-8"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-9"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-9"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-10"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-10"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-11"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-11"));
            Assert.AreEqual(true, EmployeeAssessmentStub("2018-12"));
            Assert.AreEqual(true, EmployeeSalaryInputStub("2018-12"));

            // 先离职
            Assert.AreEqual(EmployeeLeaveStub(1), true);

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