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
        }


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
