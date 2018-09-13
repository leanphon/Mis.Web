using System.ComponentModel.DataAnnotations;

namespace Apps.Model
{
    public class Role
    {
        public int id { get; set; }
        [Display(Name = "角色名")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(15, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }
    }

    public class RoleRights
    {
        public int id { get; set; }
        public int roleId { get; set; }
        public int rightId { get; set; }
    }
}