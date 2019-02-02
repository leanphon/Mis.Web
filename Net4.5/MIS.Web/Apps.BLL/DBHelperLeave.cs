using Apps.Model;

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

            modelBuilder.Entity<EmployeeLeave>().ToTable("EmployeeLeave");


        }

        public DbSet<EmployeeLeave> EmployeeLeaveList { get; set; }

    }

}
