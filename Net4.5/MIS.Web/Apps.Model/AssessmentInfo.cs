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
    /// 考核信息
    /// </summary>
    public class AssessmentInfo
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "员工")]
        public long employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual Employee employee { get; set; }


        [Display(Name = "月份")]
        public string month { get; set; }

        [Display(Name = "绩效得分")]
        public double? performanceScore { get; set; }


        [Display(Name = "效益得分")]
        public double? benefitScore { get; set; }

        [Display(Name = "应出勤")]
        public double shouldWorkTime { get; set; }

        [Display(Name = "实出勤")]
        public double actualWorkTime { get; set; }

        [Display(Name = "平日加班")]
        public double? normalOvertime { get; set; }

        [Display(Name = "法定节假日加班")]
        public double? holidayOvertime { get; set; }

        [Display(Name = "迟到")]
        public double? lateTime { get; set; }

        [Display(Name = "早退")]
        public double? earlyTime { get; set; }

        [Display(Name = "旷工")]
        public double? absenteeismTime { get; set; }

        [Display(Name = "事假")]
        public double? personalLeaveTime { get; set; }

        [Display(Name = "病假")]
        public double? sickLeaveTime { get; set; }

        [Display(Name = "使用年假")]
        public double? annualVacationTime { get; set; }

        [Display(Name = "录入时间")]
        public DateTime inputDate { get; set; }

        /// <summary>
        /// 可取值：待审核、已审核
        /// </summary>
        [Display(Name = "状态")]
        public string status { get; set; }


    }

    public class AssessmentInfoExport
    {
        [Display(Name = "序号")]
        public long index { get; set; }

        [Display(Name = "月份")]
        public string month { get; set; }

        [Display(Name = "工号")]
        public string employeeNumber { get; set; }

        [Display(Name = "姓名")]
        public string employeeName { get; set; }

        [Display(Name = "部门")]
        public string departmentName { get; set; }


        [Display(Name = "应出勤")]
        public double shouldWorkTime { get; set; }

        [Display(Name = "实出勤")]
        public double actualWorkTime { get; set; }

        [Display(Name = "平日加班")]
        public double normalOvertime { get; set; }

        [Display(Name = "法定节假日加班")]
        public double holidayOvertime { get; set; }

        [Display(Name = "迟到")]
        public double lateTime { get; set; }

        [Display(Name = "早退")]
        public double earlyTime { get; set; }

        [Display(Name = "旷工")]
        public double absenteeismTime { get; set; }

        [Display(Name = "事假")]
        public double personalLeaveTime { get; set; }

        [Display(Name = "病假")]
        public double sickLeaveTime { get; set; }

        [Display(Name = "使用年假")]
        public double annualVacationTime { get; set; }


        [Display(Name = "绩效得分")]
        public double performanceScore { get; set; }


        [Display(Name = "效益得分")]
        public double benefitScore { get; set; }


        [Display(Name = "录入时间")]
        public DateTime inputDate { get; set; }

        [Display(Name = "状态")]
        public string status { get; set; }

    }
}
