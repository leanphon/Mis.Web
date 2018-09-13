using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apps.Model
{
    public class Module
    {
        public int id { get; set; }

        [Display(Name = "模块名")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(15, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }

        [Display(Name = "显示顺序")]
        public int showIndex { get; set; }

    }
}