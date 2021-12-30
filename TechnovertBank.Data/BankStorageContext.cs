using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TechnovertBank.Data
{
    public partial class BankStorageContext : DbContext
    {
        public BankStorageContext()
        {
        }

        public BankStorageContext(DbContextOptions<BankStorageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Database=BankStorage;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Username, "UQ__account__F3DBC5729C769750")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.AccountNumber).HasColumnName("accountNumber");

                entity.Property(e => e.AccountType).HasColumnName("accountType");

                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("balance");

                entity.Property(e => e.BankId)
                    .HasMaxLength(450)
                    .HasColumnName("bankId");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(450)
                    .HasColumnName("customerId");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("password");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__account__bankId__4222D4EF");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_account_customer");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Bank");

                entity.HasIndex(e => e.Bankname, "UQ__bank__206168F8576BD5B7")
                    .IsUnique();

                entity.Property(e => e.BankId).HasColumnName("bankId");

                entity.Property(e => e.Bankname)
                    .IsRequired()
                    .HasColumnName("bankname");

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("branch");

                entity.Property(e => e.DefaultCurrencyName)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("defaultCurrencyName");

                entity.Property(e => e.Funds)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("funds");

                entity.Property(e => e.Ifsc)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("ifsc");

                entity.Property(e => e.OtherImps)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("otherIMPS");

                entity.Property(e => e.OtherRtgs)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("otherRTGS");

                entity.Property(e => e.SelfImps)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("selfIMPS");

                entity.Property(e => e.SelfRtgs)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("selfRTGS");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.BankId)
                    .HasMaxLength(450)
                    .HasColumnName("bankId");

                entity.Property(e => e.ExchangeRate)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("exchangeRate");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Currencies)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__currency__bankId__3A81B327");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.AadharNumber).HasColumnName("aadharNumber");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("address");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("contactNumber");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("name");

                entity.Property(e => e.PanNumber)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("panNumber");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.HasIndex(e => e.Username, "UQ__employee__F3DBC57229C10B7B")
                    .IsUnique();

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.BankId)
                    .HasMaxLength(450)
                    .HasColumnName("bankId");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("customerId");

                entity.Property(e => e.Designation).HasColumnName("designation");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__employee__bankId__3E52440B");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_customer");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransId)
                    .HasName("PK__transact__DB107FA7045820EB");

                entity.ToTable("Transaction");

                entity.Property(e => e.TransId).HasColumnName("transId");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(450)
                    .HasColumnName("accountId");

                entity.Property(e => e.BankId)
                    .HasMaxLength(450)
                    .HasColumnName("bankId");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("currency");

                entity.Property(e => e.IsBankTransaction).HasColumnName("isBankTransaction");

                entity.Property(e => e.ModeOfTransfer).HasColumnName("modeOfTransfer");

                entity.Property(e => e.OtherPartyBankId)
                    .HasMaxLength(450)
                    .HasColumnName("otherPartyBankId");

                entity.Property(e => e.ReceiverAccountId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("receiverAccountId");

                entity.Property(e => e.Sendername)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("sendername");

                entity.Property(e => e.TransactionAmount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("transactionAmount");

                entity.Property(e => e.TransactionOn)
                    .HasColumnType("datetime")
                    .HasColumnName("transactionOn");

                entity.Property(e => e.TransactionType).HasColumnName("transactionType");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__transacti__accou__47DBAE45");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.TransactionBanks)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("Fk_transaction_bank");

                entity.HasOne(d => d.OtherPartyBank)
                    .WithMany(p => p.TransactionOtherPartyBanks)
                    .HasForeignKey(d => d.OtherPartyBankId)
                    .HasConstraintName("FK__Transacti__other__6E01572D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
