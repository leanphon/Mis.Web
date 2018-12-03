using System.ComponentModel.DataAnnotations;

namespace Apps.Model
{
    public class Product
    {
        public long id { get; set; }
        // 名称
        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage = "{0}长度不能超出20")]
        public string name { get; set; }

        // 价格
        [Display(Name = "价格")]
        public double price { get; set; }

        // 描述
        [Display(Name = "生产商")]
        [StringLength(50, ErrorMessage = "{0}长度不能超出20")]
        public string producer { get; set; }

        // 描述
        [Display(Name = "描述")]
        [DataType(DataType.MultilineText)]
        [StringLength(100, ErrorMessage = "{0}长度不能超出100")]
        public string description { get; set; }

        [Display(Name = "类别名称")]
        public int? categoryId { get; set; }
        public virtual Category category { get; set; }
    }
}