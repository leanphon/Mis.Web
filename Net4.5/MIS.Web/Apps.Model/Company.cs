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

        [Display(Name = "公司名称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(50, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }


        [Display(Name = "公司简称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string code { get; set; }

        [Display(Name = "logo图片")]
        public string logo { get; set; }

        [Display(Name = "登录时背景图片")]
        public string loginImg { get; set; }

        [Display(Name = "主页面背景图片")]
        public string mainImg { get; set; }

        [Display(Name = "公司地址")]
        [StringLength(100, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string address { get; set; }

    }
}
