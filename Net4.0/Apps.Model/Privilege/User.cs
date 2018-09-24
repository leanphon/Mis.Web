using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Privilege
{
    public class RootUser
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(128, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string passwd { get; set; }

    }

    public class User
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(128, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string passwd { get; set; }

        [Display(Name = "角色")]
        public long roleId { get; set; }
        [ForeignKey("roleId")]
        [JsonIgnore]
        public virtual Role role { get; set; }
    }
}
