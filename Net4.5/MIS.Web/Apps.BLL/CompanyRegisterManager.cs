﻿using Apps.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Apps.BLL.Utility;

namespace Apps.BLL
{
    public class CompanyRegisterManager
    {
        public static OperateResult  Add(CompanyRegister model)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var match = from m in db.companyRegisterList
                                where m.name.Equals(model.name) || m.code.Equals(model.code)
                                select m;
                    if (match.Count() > 0)
                    {
                        return new OperateResult
                        {
                            content = "该公司已经存在",
                        };
                    }

                    db.companyRegisterList.Add(model);
                    Company c = new Company
                    {
                        name = model.name,
                        code = model.code
                    };
                    db.companyList.Add(c);

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
        public static OperateResult  Remove(long id)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var element = db.companyRegisterList.Find(id);

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该部门",
                        };
                    }

                    db.companyRegisterList.Remove(element);

                    db.Entry(element).State = EntityState.Deleted;

                    var company = (from c in db.companyList
                                   where c.name == element.name
                                   select c).FirstOrDefault();
                    db.companyList.Remove(company);
                    db.Entry(company).State = EntityState.Deleted;


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

        public static OperateResult RemoveAll()
        {
            try
            {
                using (SystemDB db = new SystemDB())
                {
                    db.companyRegisterList.RemoveRange(db.companyRegisterList.ToList());

                    db.SaveChanges();

                    LogManager.Add(new LogRecord
                    {
                        userId = SessionHelper.GetUserId(),
                        time = DateTime.Now,
                        type = "Info",
                        content = "删除所有公司注册信息"
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


        public static OperateResult  Update(CompanyRegister model)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var elements = (from e in db.companyRegisterList
                                    where e.id != model.id && e.name == model.name
                                    select e
                                    ).ToList();
                    if (elements.Count() >= 1)
                    {
                        return new OperateResult
                        {
                            content = "已经存在同名的部门",
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

            }
            catch (Exception ex)
            {
                return new OperateResult
                {
                    content = Model.Utility.Utility.GetExceptionMsg(ex),
                };
            }
        }
        public static OperateResult  GetByCode(string code)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var element = (from m in db.companyRegisterList
                                   where code == m.code
                                   select m
                                ).FirstOrDefault();

                    if (element == null)
                    {
                        return new OperateResult
                        {
                            content = "不存在该公司",
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
        public static OperateResult  GetById(long id)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var element = (from m in db.companyRegisterList
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

        public static OperateResult  GetAll(QueryParam param = null)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var elements = from e in db.companyRegisterList
                                   select e;

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

        public static OperateResult  GetByPager(QueryParam param = null)
        {
            try
            {
                using (DbContextRoot db = new DbContextRoot())
                {

                    var elements = from e in db.companyRegisterList
                                   select e;

                    int total = elements.Count();
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
