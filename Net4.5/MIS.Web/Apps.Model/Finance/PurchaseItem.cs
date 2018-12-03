using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apps.Model.Finance
{
    public class PurchaseItem
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "物料序号")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0}的长度在{2}至{1}个字符间")]
        public string ItemSerial { get; set; }

        [Required]
        [Display(Name = "物料名称")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "{0}的长度在{2}至{1}个字符间")]
        public string name { get; set; }

        [Required]
        [Display(Name = "规格型号")]
        public string specification { get; set; }

        [Required]
        [Display(Name = "单位")]
        public double unit { get; set; }

        [Required]
        [Display(Name = "数量")]
        public double quantity { get; set; }

        [Required]
        [Display(Name = "单价")]
        public double discount { get; set; }


    }
}
