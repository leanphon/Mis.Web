using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Apps.Model
{
    /// <summary>
    /// 公司向系统注册的信息
    /// </summary>
    public class CompanyRegister
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "公司名称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(30, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }

        [Display(Name = "公司简称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string code { get; set; }


    }
}
