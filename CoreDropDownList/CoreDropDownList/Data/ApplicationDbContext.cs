using Microsoft.EntityFrameworkCore;
using CoreDropDownList.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoreDropDownList.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Center> Centers { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<Collection> Collections { get; set; }


        public DbSet<Department> Departments { get; set; }
        public DbSet<Designations> Designation { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.seed();
            //modelBuilder.EntityTypeBuilder.Ignore();
        }


    }
}

