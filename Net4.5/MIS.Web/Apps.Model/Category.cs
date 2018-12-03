using System.ComponentModel.DataAnnotations;

namespace Apps.Model
{
    public class Category
    {
        public int id { get; set; }
        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "{0}必须填写")]
        [StringLength(20, ErrorMessage ="{0}长度不能超出20")]
        public string name { get; set; }

        [Display(Name = "父类别")]
        public int? parentId { get; set; }
        public virtual Category parent { get; set; }
    }
}