using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apps.Model
{
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
        public string status { get; set; }
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

    }
}