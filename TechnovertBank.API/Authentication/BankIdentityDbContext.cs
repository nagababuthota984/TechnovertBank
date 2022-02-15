using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechnovertBank.API.Authentication
{
    public class BankIdentityDbContext :IdentityDbContext<BankUser>
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
