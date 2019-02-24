namespace Apps.BLL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Apps.Model.Finance;
    using Apps.Model;
    using Model.Privilege;

    public class DbContextRoot : DbContext
    {
        public DbContextRoot() 
            : base("name=AppsDB")
        {
        }

        public DbContextRoot(string conStr)
            : base(conStr)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostInfo>().ToTable("PostInfos");
            modelBuilder.Entity<LevelInfo>().ToTable("LevelInfos");
            modelBuilder.Entity<PerformanceInfo>().ToTable("PerformanceInfos");
            modelBuilder.Entity<BenefitInfo>().ToTable("BenefitInfos");
            modelBuilder.Entity<SalaryInfo>().ToTable("SalaryInfos");
            modelBuilder.Entity<SalaryRecord>().ToTable("SalaryRecords");
            modelBuilder.Entity<AssessmentInfo>().ToTable("AssessmentInfos");
            //modelBuilder.Entity<RoleRights>().ToTable("RoleRights");
            modelBuilder.Entity<Company>().ToTable("Companys");
            modelBuilder.Entity<EmployeeCareerRecord>().ToTable("CareerRecords");
            //modelBuilder.Entity<EmployeeLeave>().ToTable("EmployeeLeave");

            modelBuilder.Entity<Role>()
            .HasMany(t => t.rightList)
            .WithMany(t => t.roleList)
            .Map(m =>
            {
                m.MapLeftKey("roleId");
                m.MapRightKey("rightId");
                m.ToTable("RoleRights");
            });

        }

        public DbSet<Module> moduleList { get; set; }
        public DbSet<FunctionRight> rightList { get; set; }
        public DbSet<Role> roleList { get; set; }
        public DbSet<User> userList { get; set; }
        public DbSet<LogRecord> logRecordList { get; set; }


        public DbSet<CompanyRegister> companyRegisterList { get; set; }

        public DbSet<Company> companyList { get; set; }
        public DbSet<Employee> employeeList { get; set; }
        public DbSet<EmployeeCareerRecord> employeeCareerList { get; set; }
        public DbSet<Department> departmentList { get; set; }

        public DbSet<PostInfo> postInfoList { get; set; }
        public DbSet<LevelInfo> levelInfoList { get; set; }
        public DbSet<PerformanceInfo> performanceInfoList { get; set; }
        public DbSet<BenefitInfo> benefitInfoList { get; set; }
        public DbSet<SalaryInfo> salaryInfoList { get; set; }
        public DbSet<AssessmentInfo> assessmentInfoList { get; set; }
        public DbSet<SalaryRecord> salaryRecordList { get; set; }

        /// <summary>
        /// ¿Î÷∞¡–±Ì
        /// </summary>
        public DbSet<EmployeeLeave> EmployeeLeaveList { get; set; }

    }

    public class SystemDB : DbContextRoot
    {
        public SystemDB()
            : base("name=AppsDB")
        //: base(SessionHelper.GetDbName())
        {
        }

        public SystemDB(string dbName)
            : base(dbName)
        {
        }

    }

}
