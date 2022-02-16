using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechnovertBank.API.Authentication
{
    /// <summary>
    /// It adds and manages users and roles related tables to the database. Inherits IdentityDbContext instead of DbContext.
    /// </summary>
    public class BankIdentityDbContext :IdentityDbContext<IdentityUser>
    {
        public BankIdentityDbContext()
        {

        }
        public BankIdentityDbContext(DbContextOptions<BankIdentityDbContext> options):base(options)
        {

        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Database=BankStorage;Trusted_Connection=True;");
            }
        }
    }
}
