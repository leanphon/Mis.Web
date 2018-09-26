using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Privilege
{
    public enum RoleType
    {
        Super = 0,
        [Display(Name = "角色名")]
        Admin,
        System,
        Operator,
        Guest = 20
    }

    public class Role
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "角色名")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }

        [Display(Name = "描述")]
        public string description { get; set; }

        [Display(Name = "权限")]
        [JsonIgnore]
        public List<FunctionRight> rightList { get; set; }

    }


    public class RoleRights
    {
        [Key]
        public long id { get; set; }
        public long roleId { get; set; }
        public long rightId { get; set; }

    }
}
