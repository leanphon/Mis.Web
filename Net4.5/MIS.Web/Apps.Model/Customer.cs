using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apps.Model
{
    public class Baby
    {
        public int id { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string babyName { get; set; }

        [Display(Name = "年龄")]
        [Required(ErrorMessage = "{0}必须填写")]
        public int babyAge { get; set; }

        [Display(Name = "关系")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string relation { get; set; }

        public long parentId { get; set; }
        public Customer parent { get; set; }
    }

    public class Customer
    {
        public long id { get; set; }
        // 姓名
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(10, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }
        // 性别
        [Display(Name = "性别")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(2, ErrorMessage = "{0}的长度不能大于2！")]
        public string gender { get; set; }

        // 联系电话
        [Display(Name = "联系电话")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string phone { get; set; }
        // 身份证
        [Display(Name = "身份证")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string idCard { get; set; }
        
        // 邮箱
        [Display(Name = "邮箱")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string email { get; set; }
        // 微信
        [Display(Name = "微信")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string weichat { get; set; }

        // QQ
        [Display(Name = "QQ")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string qq { get; set; }

        // 联系地址
        [Display(Name = "联系地址")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于20！")]
        public string address { get; set; }

    }
}