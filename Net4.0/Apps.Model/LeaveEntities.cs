using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.Model.Leave
{
    public class LeaveAddress
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string address { get; set; }

        public double toCompanyDistance { get; set; }
    }

    public class LeaveAge
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public long age { get; set; }

    }

    public class LeaveDepartment
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public long departmentId { get; set; }

    }

    public class LeaveEducation
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string education { get; set; }

    }

    public class LeaveExperience
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public int experience { get; set; }

    }

    public class LeaveMarriage
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string marriage { get; set; }

    }

    public class LeaveNation
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string nation { get; set; }

    }

    public class LeaveNativePlace
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string nativePlace { get; set; }

    }

    public class LeavePolitical
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string political { get; set; }

    }

    public class LeaveSex
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string sex { get; set; }

    }

    public class LeaveSource
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public string source { get; set; }

    }
    public class LeaveWorkAge
    {
        public long id { get; set; }
        public long employeeId { get; set; }

        public long workAge { get; set; }

    }
}
