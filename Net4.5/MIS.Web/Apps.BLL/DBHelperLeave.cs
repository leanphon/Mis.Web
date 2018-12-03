namespace Apps.BLL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Model.Leave;

    public class DbHelperLeave : DbContext
    {
        public DbHelperLeave() 
            : base("name=Leave")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveSalary>().ToTable("Salary");
            modelBuilder.Entity<LeaveAddress>().ToTable("Address");
            modelBuilder.Entity<LeaveAge>().ToTable("Age");
            modelBuilder.Entity<LeaveDepartment>().ToTable("Department");
            modelBuilder.Entity<LeaveEducation>().ToTable("Education");
            modelBuilder.Entity<LeaveExperience>().ToTable("Experience");
            modelBuilder.Entity<LeaveMarriage>().ToTable("Marriage");
            modelBuilder.Entity<LeaveNation>().ToTable("Nation");
            modelBuilder.Entity<LeaveNativePlace>().ToTable("NativePlace");
            modelBuilder.Entity<LeaveSex>().ToTable("Sex");
            modelBuilder.Entity<LeaveSource>().ToTable("Source");
            modelBuilder.Entity<LeaveWorkAge>().ToTable("WorkAge");
            modelBuilder.Entity<LeaveRecord>().ToTable("LeaveRecord");
            modelBuilder.Entity<LeavePolitical>().ToTable("Political");

        }

        public DbSet<LeaveRecord> leaveRecordList { get; set; }
        public DbSet<LeaveSalary> salaryList { get; set; }
        public DbSet<LeaveAddress> addressList { get; set; }
        public DbSet<LeaveAge> ageList { get; set; }
        public DbSet<LeaveDepartment> departmentList { get; set; }
        public DbSet<LeaveEducation> educationList { get; set; }
        public DbSet<LeaveExperience> experienceList { get; set; }
        public DbSet<LeaveMarriage> marriageList { get; set; }
        public DbSet<LeaveNation> nationList { get; set; }
        public DbSet<LeaveNativePlace> nativePlaceList { get; set; }
        public DbSet<LeavePolitical> politicalList { get; set; }
        public DbSet<LeaveSex> sexList { get; set; }
        public DbSet<LeaveSource> sourceList { get; set; }
        public DbSet<LeaveWorkAge> workAgeList { get; set; }
    }

}
