using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.Model.Leave
{
    public class EmployeeLeave : Employee
    {
        /// <summary>
        /// 离职前的岗位
        /// </summary>
        public string postName { get; set; }

        /// <summary>
        /// 离职前的理论工资
        /// </summary>
        public double salary { get; set; }

        /// <summary>
        /// 入职以来的月平均工资
        /// </summary>
        public double salaryAverage { get; set; }

    }
}
