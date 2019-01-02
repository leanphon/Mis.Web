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
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL
{

    public class EmployeeManager
    {
        public OperateResult Add(Employee model)
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
        public OperateResult Remove(long id)
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

        public OperateResult Update(Employee model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.employeeList
                                    where e.id != model.id && e.number == model.number
                                    select e
                                    ).ToList();
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在该工号",
                        };
                    }


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

        public OperateResult GetSalaryInfoById(long id)
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

        public OperateResult UpdateSalary(long employeeId, SalaryInfo model)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.salaryInfoList
                                    where e.id == model.id
                                    select e
                                    ).AsNoTracking().ToList();

                    if (elements.Count() == 1)
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
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

                        db.salaryInfoList.Add(model);
                        db.SaveChanges();

                        employee.salaryInfo = model;
                        employee.salaryInfoId = model.id;
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
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

        //public OperateResult UpdateState(long id, EmployeeState state)
        public OperateResult Formal(long id, string state, DateTime time)
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

        public OperateResult Leave(long id, string state, DateTime time)
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

        public OperateResult SelectDepartment(long id, long departmentId)
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

        public OperateResult GetById(long id)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var element = (from m in db.employeeList.Include("department").Include("salaryInfo")
                                .Include("salaryInfo.postInfo").Include("salaryInfo.levelInfo")
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

        public OperateResult GetAll(QueryParam param = null)
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


        public OperateResult GetByPager(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList.Include("salaryInfo").Include("salaryInfo.postInfo")
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
                                      postName = e.salaryInfo.postInfo.name

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
                            elements = elements.Where(t => t.name.Contains(p.value));
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
        public OperateResult ExportAll(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeList
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

                                  };


                    DataTable dt = DataTableHelper.ToDataTable<EmployeeExport>(results.ToList());

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

        public OperateResult ImportExcel(string fileName)
        {
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
                var query = (from e in tsheet
                             select e).ToList();

                IEnumerable<Employee> elements;
                using (SystemDB db = new SystemDB())
                {
                    elements = (from e in query
                                join d in db.departmentList
                                 on e.departmentName equals d.name
                                select new Employee
                                {
                                    number = e.number,
                                    name = e.name,
                                    departmentId = d.id,
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

                                }).ToList();
                }



                bool fail = false;
                OperateResult result = new OperateResult();
                if (elements.Count() == 0)
                {
                    result.content = "无数据或数据不完整，导入失败！";

                    return result;
                }
                foreach (var model in elements)
                {
                    OperateResult or = Add(model);
                    if (or.status != OperateStatus.Success)
                    {
                        fail = true;

                        result.content += "工号(" + model.number + ")数据保存失败, error（" + or.content + "） ;";
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

        public OperateResult AnalyseByAge(QueryParam param = null)
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


        public OperateResult AnalyseByGender(QueryParam param = null)
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
        public OperateResult AnalyseByWorkAge(QueryParam param = null)
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
        public OperateResult AnalyseBySalary(QueryParam param = null)
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

                    var ruslt = elements.ToList();

                    foreach (var e in ruslt)
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


        public OperateResult AddCareerRecordBatch(List<EmployeeCareerRecord> lstData)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    foreach (var model in lstData)
                    {
                        model.status = "audit";
                        db.employeeCareerList.Add(model);
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
        public OperateResult RemoveCareerRecord(long id)
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

        public OperateResult UpdateCareerRecord(EmployeeCareerRecord model)
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


        public OperateResult GetCareerRecordById(long id)
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
        public OperateResult GetCareerRecordsById(long employeeId)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = from e in db.employeeCareerList
                                  where employeeId == e.employeeId
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

        public OperateResult GetAllCareerRecords(QueryParam param = null)
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {

                    var elements = (from e in db.employeeCareerList.Include("employee")
                                    select new
                                    {
                                        e.id,
                                        e.type,
                                        e.time,
                                        e.employee.name,
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


        public OperateResult GetAllCareerRecordByPager(QueryParam param = null)
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
                                       e.employee.departmentId
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
                            elements = elements.Where(t => t.name.Contains(p.value));
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
