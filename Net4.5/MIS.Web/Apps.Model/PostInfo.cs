using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Apps.Model
{
    public class PostInfo
    {
        [Key]
        public long id { get; set; }
        //岗位名称
        [Display(Name = "岗位名称")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }
    }
}
