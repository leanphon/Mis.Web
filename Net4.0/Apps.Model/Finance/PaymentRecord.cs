using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Finance
{
    public class PaymentRecord
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "付款单号")]
        [StringLength(30, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string recordSerial { get; set; }

        [Required]
        [Display(Name = "付款单名称")]
        [StringLength(30, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }

        [Required]
        [Display(Name = "所属项目")]
        public long paymentProjectId { get; set; }
        public virtual PaymentProject paymentProject { get; set; }


        [Required]
        [Display(Name = "付款金额")]
        public double total { get; set; }

        [Required]
        [Display(Name = "付款时间")]
        public DateTime payDateTime { get; set; }

        [Display(Name = "描述")]
        [StringLength(100, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string description { get; set; }

        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string remark { get; set; }

        [Display(Name = "录入时间")]
        public DateTime inputDateTime { get; set; }

        
    }
}
