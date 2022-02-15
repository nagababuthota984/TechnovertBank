﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnovertBank.Data;

namespace TechnovertBank.Data.Migrations
{
    [DbContext(typeof(BankStorageContext))]
    partial class BankStorageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TechnovertBank.Data.Account", b =>
                {
                    b.Property<string>("AccountId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("accountId");

                    b.Property<long>("AccountNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("accountNumber");

                    b.Property<int>("AccountType")
                        .HasColumnType("int")
                        .HasColumnName("accountType");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("balance");

                    b.Property<string>("BankId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bankId");

                    b.Property<string>("CustomerId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("customerId");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("password");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("username");

                    b.HasKey("AccountId");

                    b.HasIndex("BankId");

                    b.HasIndex("CustomerId");

                    b.HasIndex(new[] { "Username" }, "UQ__account__F3DBC5729C769750")
                        .IsUnique();

                    b.ToTable("Account");
                });

            modelBuilder.Entity("TechnovertBank.Data.Bank", b =>
                {
                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bankId");

                    b.Property<string>("Bankname")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bankname");

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("branch");

                    b.Property<string>("DefaultCurrencyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("defaultCurrencyName");

                    b.Property<decimal>("Funds")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("funds");

                    b.Property<string>("Ifsc")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ifsc");

                    b.Property<decimal>("OtherImps")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("otherIMPS");

                    b.Property<decimal>("OtherRtgs")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("otherRTGS");

                    b.Property<decimal>("SelfImps")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("selfIMPS");

                    b.Property<decimal>("SelfRtgs")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("selfRTGS");

                    b.HasKey("BankId");

                    b.HasIndex(new[] { "Bankname" }, "UQ__bank__206168F8576BD5B7")
                        .IsUnique();

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("TechnovertBank.Data.Currency", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BankId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bankId");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("exchangeRate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("TechnovertBank.Data.Customer", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("customerId");

                    b.Property<long>("AadharNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("aadharNumber");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("address");

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasColumnName("age");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("contactNumber");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime")
                        .HasColumnName("dob");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("gender");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("name");

                    b.Property<string>("PanNumber")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("panNumber");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("TechnovertBank.Data.Employee", b =>
                {
                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("employeeId");

                    b.Property<string>("BankId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bankId");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("customerId");

                    b.Property<int>("Designation")
                        .HasColumnType("int")
                        .HasColumnName("designation");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("username");

                    b.HasKey("EmployeeId");

                    b.HasIndex("BankId");

                    b.HasIndex("CustomerId");

                    b.HasIndex(new[] { "Username" }, "UQ__employee__F3DBC57229C10B7B")
                        .IsUnique();

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("TechnovertBank.Data.Transaction", b =>
                {
                    b.Property<string>("TransId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("transId");

                    b.Property<string>("AccountId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("accountId");

                    b.Property<string>("BankId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bankId");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("currency");

                    b.Property<bool>("IsBankTransaction")
                        .HasColumnType("bit")
                        .HasColumnName("isBankTransaction");

                    b.Property<int>("ModeOfTransfer")
                        .HasColumnType("int")
                        .HasColumnName("modeOfTransfer");

                    b.Property<string>("OtherPartyBankId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("otherPartyBankId");

                    b.Property<string>("ReceiverAccountId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("receiverAccountId");

                    b.Property<string>("Sendername")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("sendername");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("transactionAmount");

                    b.Property<DateTime>("TransactionOn")
                        .HasColumnType("datetime")
                        .HasColumnName("transactionOn");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int")
                        .HasColumnName("transactionType");

                    b.HasKey("TransId")
                        .HasName("PK__transact__DB107FA7045820EB");

                    b.HasIndex("AccountId");

                    b.HasIndex("BankId");

                    b.HasIndex("OtherPartyBankId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("TechnovertBank.Data.Account", b =>
                {
                    b.HasOne("TechnovertBank.Data.Bank", "Bank")
                        .WithMany("Accounts")
                        .HasForeignKey("BankId")
                        .HasConstraintName("FK__account__bankId__4222D4EF");

                    b.HasOne("TechnovertBank.Data.Customer", "Customer")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_account_customer");

                    b.Navigation("Bank");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("TechnovertBank.Data.Currency", b =>
                {
                    b.HasOne("TechnovertBank.Data.Bank", "Bank")
                        .WithMany("Currencies")
                        .HasForeignKey("BankId")
                        .HasConstraintName("FK__currency__bankId__3A81B327");

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("TechnovertBank.Data.Employee", b =>
                {
                    b.HasOne("TechnovertBank.Data.Bank", "Bank")
                        .WithMany("Employees")
                        .HasForeignKey("BankId")
                        .HasConstraintName("FK__employee__bankId__3E52440B");

                    b.HasOne("TechnovertBank.Data.Customer", "Customer")
                        .WithMany("Employees")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_employee_customer")
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("TechnovertBank.Data.Transaction", b =>
                {
                    b.HasOne("TechnovertBank.Data.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__transacti__accou__47DBAE45");

                    b.HasOne("TechnovertBank.Data.Bank", "Bank")
                        .WithMany("TransactionBanks")
                        .HasForeignKey("BankId")
                        .HasConstraintName("Fk_transaction_bank");

                    b.HasOne("TechnovertBank.Data.Bank", "OtherPartyBank")
                        .WithMany("TransactionOtherPartyBanks")
                        .HasForeignKey("OtherPartyBankId")
                        .HasConstraintName("FK__Transacti__other__6E01572D");

                    b.Navigation("Account");

                    b.Navigation("Bank");

                    b.Navigation("OtherPartyBank");
                });

            modelBuilder.Entity("TechnovertBank.Data.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("TechnovertBank.Data.Bank", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Currencies");

                    b.Navigation("Employees");

                    b.Navigation("TransactionBanks");

                    b.Navigation("TransactionOtherPartyBanks");
                });

            modelBuilder.Entity("TechnovertBank.Data.Customer", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
