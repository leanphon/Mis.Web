using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apps.Model.Privilege
{
    public class FunctionRight
    {
        public long id { get; set; }

        [Display(Name = "功能权限名")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(15, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }

        [Display(Name = "url地址")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]

        public string url { get; set; }

        [Display(Name = "图标")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string icon { get; set; }

        [Display(Name = "所属功能模块")]
        [Required(ErrorMessage = "{0}必须填写！")]
        public long moduleId { get; set; }
        [ForeignKey("moduleId")]
        public virtual Module module { get; set; }
    }
}