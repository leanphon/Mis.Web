using System.ComponentModel.DataAnnotations;

namespace Apps.Model
{
    public class FunctionRight
    {
        public int id { get; set; }

        [Display(Name = "功能权限名")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(15, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }

        [Display(Name = "url地址")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string url { get; set; }

        [Display(Name = "所属功能模块")]
        [Required(ErrorMessage = "{0}必须填写！")]
        public int moduleId { get; set; }
        public virtual Module module { get; set; }
    }
}