using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apps.Model.Finance
{
    public class PurchaseRecord
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "采购单号")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0}的长度在{2}至{1}个字符间")]
        public string recordSerial { get; set; }

        [Required]
        [Display(Name ="采购单名称")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0}的长度在{2}至{1}个字符间")]
        public string name { get; set; }

        [Required]
        [Display(Name = "合计")]
        public double total { get; set; }

        [Required]
        [Display(Name = "优惠")]
        public double discount { get; set; }

        [Required]
        [Display(Name = "实际总价")]
        public double actulTotal { get; set; }

        [Display(Name = "付款时间")]
        public DateTime payDateTime { get; set; }

        [Display(Name = "录入时间")]
        public DateTime inputDateTime { get; set; }

        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0}的长度在{1}个字符间")]
        public string remark { get; set; }

        [Required]
        [Display(Name = "供应商")]
        public long supplierId { get; set; }
        public virtual Supplier supplier { get; set; }

        public ICollection<PurchaseItem> purchaseItems;

    }
}
