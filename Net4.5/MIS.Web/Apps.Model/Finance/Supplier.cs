using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apps.Model.Finance
{
    public class Supplier
    {
        [Key]
        public long id { get; set; }

        [Required]
        [Display(Name = "供应商名称")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于{1}个字符")]
        public string name { get; set; }

        [Required]
        [Display(Name = "联系电话")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于{1}个字符")]
        public string telephone { get; set; }

        [Display(Name = "地址")]
        [StringLength(100, ErrorMessage = "{0}的长度不能大于{1}个字符")]
        public string address { get; set; }



    }
}
