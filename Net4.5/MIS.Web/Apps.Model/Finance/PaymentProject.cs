using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Finance
{
    public class PaymentProject
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "项目编号")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0}的长度在{1}个字符间")]
        public string projectSerial { get; set; }

        [Required]
        [Display(Name = "项目名称")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0}的长度在{1}个字符间")]
        public string name { get; set; }

        [Display(Name = "描述")]
        [StringLength(100, ErrorMessage = "{0}的长度在{1}个字符间")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }

    }
}
