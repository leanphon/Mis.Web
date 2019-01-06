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
    /// 薪水记录
    /// </summary>
    public class SalaryRecord
    {
        [Key]
        public long id { get; set; }

        // 工资单号
        [Display(Name = "工资单号")]
        public string billSerial { get; set; }

        public long assessmentInfoId { get; set; }
        [ForeignKey("assessmentInfoId")]
        public virtual AssessmentInfo assessmentInfo { get; set; }

        // 岗位工资
        [Display(Name = "层级工资")]
        public double levelSalary { get; set; }
        // 全勤奖
        [Display(Name = "全勤奖")]
        public double fullAttendanceRewards { get; set; }

        [Display(Name = "工作日加班")]
        public double normalOvertimeRewards { get; set; }

        [Display(Name = "节假日加班")]
        public double holidayOvertimeRewards { get; set; }


        // 绩效工资
        [Display(Name = "绩效奖金")]
        public double performanceRewards { get; set; }
        // 效益工资
        [Display(Name = "效益奖金")]
        public double benefitRewards { get; set; }
        // 工龄奖
        [Display(Name = "工龄奖")]
        public double seniorityRewards { get; set; }
        // 补发工资
        [Display(Name = "补贴")]
        public double subsidy { get; set; }
        // 补发工资
        [Display(Name = "补发")]
        public double reissue { get; set; }


        // 社保
        [Display(Name = "社保")]
        public double socialSecurity { get; set; }
        // 公积金
        [Display(Name = "公积金")]
        public double publicFund { get; set; }
        // 缴税
        [Display(Name = "个人所得税")]
        public double tax { get; set; }

        [Display(Name = "其他扣款")]
        public double chargeback { get; set; }
        // 应发工资
        [Display(Name = "应发工资")]
        public double shouldTotal { get; set; }
        // 实发工资
        [Display(Name = "实发工资")]
        public double actualTotal { get; set; }

        [Display(Name = "录入时间")]
        public DateTime inputDate { get; set; }

        /// <summary>
        /// 可取值：待审核、已审核
        /// </summary>
        [Display(Name = "状态")]
        public string status { get; set; }

    }


    public class SalaryRecordExport
    {
        [Display(Name = "序号")]
        public long index { get; set; }

        [Display(Name = "月份")]
        public string month { get; set; }


        // 工资单号
        [Display(Name = "工资单号")]
        public string billSerial { get; set; }

        [Display(Name = "工号")]
        public string employeeNumber { get; set; }

        [Display(Name = "姓名")]
        public string employeeName { get; set; }

        [Display(Name = "部门")]
        public string departmentName { get; set; }


        // 岗位工资
        [Display(Name = "岗位工资")]
        public double levelSalary { get; set; }
        // 全勤奖
        [Display(Name = "全勤奖")]
        public double fullAttendanceRewards { get; set; }

        [Display(Name = "工作日加班")]
        public double normalOvertimeRewards { get; set; }

        [Display(Name = "节假日加班")]
        public double holidayOvertimeRewards { get; set; }


        // 绩效工资
        [Display(Name = "绩效奖金")]
        public double performanceRewards { get; set; }
        // 效益工资
        [Display(Name = "效益奖金")]
        public double benefitRewards { get; set; }
        // 工龄奖
        [Display(Name = "工龄奖")]
        public double seniorityRewards { get; set; }
        // 补发工资
        [Display(Name = "补贴")]
        public double subsidy { get; set; }
        // 补发工资
        [Display(Name = "补发")]
        public double reissue { get; set; }


        // 社保
        [Display(Name = "社保")]
        public double socialSecurity { get; set; }
        // 公积金
        [Display(Name = "公积金")]
        public double publicFund { get; set; }
        // 缴税
        [Display(Name = "个人所得税")]
        public double tax { get; set; }
        // 应发工资
        [Display(Name = "应发工资")]
        public double shouldTotal { get; set; }
        // 实发工资
        [Display(Name = "实发工资")]
        public double actualTotal { get; set; }

        [Display(Name = "录入时间")]
        public DateTime inputDate { get; set; }

        [Display(Name = "状态")]
        public string status { get; set; }

    }
}
