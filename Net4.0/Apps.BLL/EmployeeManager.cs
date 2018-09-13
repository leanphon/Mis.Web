using Apps.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

                    db.Entry(element).State = EntityState.Deleted;
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


                    db.Entry(model).State = EntityState.Modified;

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
                                        e.status,
                                        e.entryDate,
                                        e.formalDate,
                                        e.leaveDate,
                                        e.emergencyContact,
                                        e.emergencyPhone,
                                        e.departmentId,
                                        parentName = e.department.name
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
                                       e.status,
                                       e.entryDate,
                                       e.formalDate,
                                       e.leaveDate,
                                       e.emergencyContact,
                                       e.emergencyPhone,
                                       departmentName = e.department.name
                                   };

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

        public OperateResult GetSalaryInfoById(long id)
        {
            using (SystemDB db = new SystemDB())
            {
                try
                {
                    var element = (from m in db.salaryInfoList.Include("postInfo").Include("employee")
                                   .Include("performanceInfo").Include("benefitInfo")
                                   where m.employeeId==id
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
                        db.Entry(model).State = EntityState.Modified;
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

    }
}
