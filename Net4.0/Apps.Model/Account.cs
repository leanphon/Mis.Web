using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Model
{
    public enum AccountState
    {
        Initial = 0, //初始状态未激活
        Normal, // 正常使用状态
        Lock, //锁定状态
        Destroy, //销毁状态
    }


    public class Account
    {
        public long id { get; set; }

        [Display(Name = "帐号")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string username { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(128, ErrorMessage = "{0}的长度不能大于128！")]
        public string passwd { get; set; }

        [Display(Name = "电话")]
        [StringLength(15, ErrorMessage = "{0}的长度不能大于15！")]
        public string phone { get; set; }

        [Display(Name = "email")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string email { get; set; }

        [Display(Name = "帐号状态")]
        public AccountState state { get; set; }

        public DateTime lastLoginTime { get; set; }

        [Display(Name = "角色")]
        //[Required(ErrorMessage = "{0}必须填写！")]
        public int roleId { get; set; }
        //public virtual Role role { get; set; }


        public static string FormateState(AccountState state)
        {
            switch(state)
            {
                case AccountState.Initial:
                    return "尚未激活";
                case AccountState.Normal:
                    return "正常";
                case AccountState.Lock:
                    return "锁定";
                case AccountState.Destroy:
                    return "禁用";
                default:
                    return "";
            }
        }
    }

    public class LoginParamsModel
    {
        [Display(Name = "帐号")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string username { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string passwd { get; set; }

    }

    public class ResetPasswdModel
    {
        [Display(Name = "帐号")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string username { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string oldPasswd { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(30, ErrorMessage = "{0}的长度不能大于30！")]
        public string newPasswd { get; set; }

    }

}