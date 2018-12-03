using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apps.Model
{
    public class Department
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

        // 上级部门
        [Display(Name = "上级部门")]
        public long? parentId { get; set; }
        public virtual Department parent { get; set; }
    }
}