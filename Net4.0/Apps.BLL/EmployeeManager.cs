using Apps.Model;
using Apps.Model.Uitility;
using LinqToExcel;
using System;
using System.Collections.Generic;
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
            using (SystemDB db = new SystemDB())
            {
                try
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

                    db.employeeList.Add(model);
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
        public OperateResult Remove(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }

        }

        public OperateResult Update(Employee model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }
            }

        }

        public OperateResult GetSalaryInfoById(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = (from m in db.salaryInfoList.Include("postInfo").Include("employee")
                                   .Include("performanceInfo").Include("benefitInfo")
                                   where m.employeeId == id
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

        public OperateResult UpdateSalary(SalaryInfo model)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var elements = (from e in db.salaryInfoList
                                    where e.id == model.id
                                    select e
                                    ).AsNoTracking().ToList();

                    if (elements.Count() == 1)
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        db.salaryInfoList.Add(model);

                    }


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
                    var element = (from m in db.employeeList.Include("department")
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
                                        departmentName = e.department.name
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
                                       departmentName = e.department.name
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
                                        departmentName = e.department.name
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
                                  };


                    DataTable dt = DataTableHelper.ToDataTable<EmployeeExport>(results.ToList());

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

        public OperateResult ImportExcel(string fileName)
        {
            try
            {
                var excelFile = new ExcelQueryFactory(fileName);
                ExcelMapping<EmployeeExport>(excelFile);

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
        public static void ExcelMapping<T>(ExcelQueryFactory excelFile)
        {
            var props = typeof(T).GetProperties();
            foreach (var p in props)
            {
                var colName = DataTableHelper.GetColumnDisplay(p);
                excelFile.AddMapping(p.Name, colName);
            }
        }


        public OperateResult AnalyseByAge(QueryParam param = null)
        {

            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var now = DateTime.Now;
                    var elements = from e in db.employeeList.AsEnumerable()
                                   select new
                                   {
                                       e.id,
                                       years = Utility.Utility.CalYears(e.birthday, now),
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
                catch (Exception ex)
                {
                    return new OperateResult
                    {
                        content = ex.Message,
                    };
                }

            }
        }


        public OperateResult AnalyseByGender(QueryParam param = null)
        {

            using (SystemDB db = new SystemDB())
            {
                try
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
                        .Select(e => new {gender = e.Key, count = e.Count() });

                    var category = new List<object>();
                    var data = new List<object>();

                    foreach (var e in results)
                    {
                        category.Add(e.gender);
                        data.Add(e.count);
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
