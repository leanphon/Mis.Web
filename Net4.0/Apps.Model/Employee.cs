﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apps.Model
{
    public enum EmployeeDeedType
    {
        [Description("入职")]
        Entry = 0,
        [Description("转正")]
        Formal,
        [Description("离职")]
        Leave,
        [Description("停薪留职")]
        UnpaidLeave,
        [Description("奖励")]
        Award,
        [Description("处罚")]
        Punish,
    }

    public class EmployeeDeed
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "类别")]
        public EmployeeDeedType type { get; set; }

        [Display(Name = "日期")]
        public DateTime time { get; set; }

        [Display(Name = "备注")]
        public string remark { get; set; }

        [Display(Name = "员工")]
        public long employeeId { get; set; }
        public virtual Employee employee { get; set; }


    }

    public enum EmployeeState
    {
        [Description("试用期")]
        Entry = 0,
        [Description("转正")]
        Formal,
        [Description("离职")]
        Leave,
        [Description("停薪留职")]
        UnpaidLeave,
    }


    public class Employee
    {
        public long id { get; set; }
        // 姓名
        [Display(Name = "姓名")]
        [Required(ErrorMessage ="{0}必须输入")]
        [StringLength(20, ErrorMessage ="{0}长度不得超过20")]
        public string name { get; set; }
        // 工号
        [Display(Name = "工号")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(10, ErrorMessage = "{0}长度不得超过10")]
        public string number { get; set; }

        [Display(Name = "性别")]
        [Required(ErrorMessage = "{0}必须输入")]
        public string sex { get; set; }
        // 身份证号
        [Display(Name = "身份证")]
        public string idCard { get; set; }
        // 电话
        [Display(Name = "电话")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}长度不得超过20")]
        public string phone { get; set; }
        // 出生日期
        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "{0}必须输入")]
        public DateTime birthday { get; set; }
        // 银行卡号
        [Display(Name = "工资卡")]
        public string bankCard { get; set; }
        // 员工状态，试用期、转正、离职
        [Display(Name = "状态")]
        [Required(ErrorMessage = "{0}必须输入")]
        //public EmployeeState state { get; set; }
        public string state { get; set; }
        // 入职日期
        [Display(Name = "入职日期")]
        public DateTime? entryDate { get; set; }
        // 转正日期
        [Display(Name = "转正日期")]
        public DateTime? formalDate { get; set; }
        // 离职日期
        [Display(Name = "离职日期")]
        public DateTime? leaveDate { get; set; }
        // 紧急联系人
        [Display(Name = "紧急联系人")]
        public string emergencyContact { get; set; }
        // 紧急联系人电话
        [Display(Name = "紧急联系人电话")]
        public string emergencyPhone { get; set; }
        // 部门
        [Display(Name = "部门")]
        [Required(ErrorMessage = "{0}必须输入")]
        public long departmentId { get; set; }
        [ForeignKey("departmentId")]
        public virtual Department department { get; set; }

        [Display(Name = "籍贯")]
        public string nativePlace { get; set; }
        [Display(Name = "现居住地")]
        public string residence { get; set; }

        [Display(Name = "学历")]
        public string education { get; set; }
        [Display(Name = "婚姻状况")]
        public string marriage { get; set; }

        [Display(Name = "工作年限")]
        public string experience { get; set; }

        [Display(Name = "合同编号")]
        public string contractSerial { get; set; }
        [Display(Name = "人才来源")]
        public string source { get; set; }



    }

    public class EmployeeExport
    {
        [Display(Name = "序号")]
        public long index { get; set; }

        // 工号
        [Display(Name = "工号")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(10, ErrorMessage = "{0}长度不得超过10")]
        public string number { get; set; }

        // 姓名
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}长度不得超过20")]
        public string name { get; set; }

        // 部门
        [Display(Name = "部门")]
        [Required(ErrorMessage = "{0}必须输入")]
        public string departmentName { get; set; }

        [Display(Name = "性别")]
        [Required(ErrorMessage = "{0}必须输入")]
        public string sex { get; set; }
        // 身份证号
        [Display(Name = "身份证")]
        public string idCard { get; set; }
        // 电话
        [Display(Name = "电话")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}长度不得超过20")]
        public string phone { get; set; }
        // 出生日期
        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "{0}必须输入")]
        public DateTime birthday { get; set; }
        // 银行卡号
        [Display(Name = "工资卡")]
        public string bankCard { get; set; }
        // 员工状态，试用期、转正、离职
        [Display(Name = "状态")]
        [Required(ErrorMessage = "{0}必须输入")]
        public string state { get; set; }
        // 入职日期
        [Display(Name = "入职日期")]
        public DateTime? entryDate { get; set; }
        // 转正日期
        [Display(Name = "转正日期")]
        public DateTime? formalDate { get; set; }
        // 离职日期
        [Display(Name = "离职日期")]
        public DateTime? leaveDate { get; set; }
        // 紧急联系人
        [Display(Name = "紧急联系人")]
        public string emergencyContact { get; set; }
        // 紧急联系人电话
        [Display(Name = "紧急联系人电话")]
        public string emergencyPhone { get; set; }
    }
}