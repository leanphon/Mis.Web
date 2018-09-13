using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model
{
    /// <summary>
    /// 岗位信息
    /// </summary>
    public class PostInfo
    {
        [Key]
        public long id { get; set; }
        //岗位名称
        [Display(Name = "岗位名称")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string name { get; set; }

        // 级别
        [Display(Name = "级别代码")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(10, ErrorMessage = "{0}长度不得超过10")]
        public string code { get; set; }

        


        // 级别工资
        [Display(Name = "岗位工资")]
        [Required(ErrorMessage = "{0}必须输入")]
        public double postSalary { get; set; }

        // 全勤奖
        [Display(Name = "全勤奖")]
        [Required(ErrorMessage = "{0}必须输入")]
        public double fullAttendanceRewards { get; set; }

        [Display(Name = "工龄奖基数")]
        [Required(ErrorMessage = "{0}必须输入")]
        public double seniorityRewardsBase { get; set; }


    }

    /// <summary>
    /// 绩效奖
    /// </summary>
    public class PerformanceInfo
    {
        public long id { get; set; }

        // 级别
        [Display(Name = "绩效代码")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(10, ErrorMessage = "{0}长度不得超过10")]
        public string code { get; set; }

        // 级别工资
        [Display(Name = "绩效奖金")]
        [Required(ErrorMessage = "{0}必须输入")]
        public double performanceRewards { get; set; }
    }

    /// <summary>
    /// 效益奖
    /// </summary>
    public class BenefitInfo
    {
        [Key]
        public long id { get; set; }

        // 级别
        [Display(Name = "效益代码")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(10, ErrorMessage = "{0}长度不得超过10")]
        public string code { get; set; }

        // 级别工资
        [Display(Name = "效益奖金")]
        [Required(ErrorMessage = "{0}必须输入")]
        public double benefitRewards { get; set; }
    }

    /// <summary>
    /// 设定个人薪酬组成
    /// </summary>
    public class SalaryInfo
    {
        public long id { get; set; }

        public long employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual Employee employee { get; set; }


        [Display(Name = "岗位")]
        public long postId { get; set; }
        [ForeignKey("postId")]
        public virtual PostInfo postInfo { get; set; }

        [Display(Name = "绩效奖")]
        public long performanceId { get; set; }
        [ForeignKey("performanceId")]
        public virtual PerformanceInfo performanceInfo { get; set; }


        [Display(Name = "效益奖")]
        public long benefitId { get; set; }
        [ForeignKey("benefitId")]
        public virtual BenefitInfo benefitInfo { get; set; }



    }


}
