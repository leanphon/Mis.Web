namespace Apps.BLL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Apps.Model.Finance;
    using Apps.Model;
    using Model.Privilege;

    public partial class SystemDB : DbContext
    {
        public SystemDB()
            : base("name=SystemDB")
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
            modelBuilder.Entity<User>().ToTable("Users");



        }



        public DbSet<PurchaseRecord> purchaseRecords { get; set; }

        public DbSet<PaymentRecord> paymentRecords { get; set; }
        public DbSet<PaymentProject> paymentProjects { get; set; }



        public DbSet<Module> moduleList { get; set; }
        public DbSet<FunctionRight> rightList { get; set; }
        public DbSet<Role> roleList { get; set; }
        public DbSet<RoleRights> roleRightsList { get; set; }
        public DbSet<User> userList { get; set; }


        public DbSet<Employee> employeeList { get; set; }
        public DbSet<Department> departmentList { get; set; }
        public DbSet<Category> categoryList { get; set; }
        public DbSet<Product> productList { get; set; }


        public DbSet<PostInfo> postInfoList { get; set; }
        public DbSet<LevelInfo> levelInfoList { get; set; }
        public DbSet<PerformanceInfo> performanceInfoList { get; set; }
        public DbSet<BenefitInfo> benefitInfoList { get; set; }
        public DbSet<SalaryInfo> salaryInfoList { get; set; }
        public DbSet<AssessmentInfo> assessmentInfoList { get; set; }
        public DbSet<SalaryRecord> salaryRecordList { get; set; }



        ///// 后续拆到其他数据库
        public DbSet<Customer> customerList { get; set; }
        public DbSet<Baby> babyList { get; set; }
    }
}
