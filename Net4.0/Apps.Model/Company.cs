using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Apps.Model
{
    /// <summary>
    /// 公司组织信息封装
    /// </summary>
    public class Company
    {
        [Key]
        public long id { get; set; }
        //部门名称
        [Display(Name = "部门名称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }
        // 部门编码
        [Display(Name = "部门编码")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(10, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string code { get; set; }
    }
}
