using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apps.Model.Privilege
{
    public class FunctionRight
    {
        public long id { get; set; }

        [Display(Name = "功能权限名")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(20, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }

        [Display(Name = "url地址")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度在{1}个字符之内")]

        public string url { get; set; }

        [Display(Name = "图标")]
        [StringLength(30, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string icon { get; set; }

        [Display(Name = "所属功能模块")]
        [Required(ErrorMessage = "{0}必须填写！")]
        public long moduleId { get; set; }
        [ForeignKey("moduleId")]
        [JsonIgnore]
        public virtual Module module { get; set; }

        [Display(Name = "授权")]
        public string authorize { get; set; }

        [JsonIgnore]
        public ICollection<Role> roleList { get; set; }
    }
}