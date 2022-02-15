using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechnovertBank.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    bankId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    bankname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    branch = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ifsc = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    selfRTGS = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    selfIMPS = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    otherRTGS = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    otherIMPS = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    funds = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    defaultCurrencyName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.bankId);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    contactNumber = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    dob = table.Column<DateTime>(type: "datetime", nullable: false),
                    address = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    aadharNumber = table.Column<long>(type: "bigint", nullable: false),
                    panNumber = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    exchangeRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    bankId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                    table.ForeignKey(
                        name: "FK__currency__bankId__3A81B327",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    accountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    bankId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    accountNumber = table.Column<long>(type: "bigint", nullable: false),
                    accountType = table.Column<int>(type: "int", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.accountId);
                    table.ForeignKey(
                        name: "FK__account__bankId__4222D4EF",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_account_customer",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    bankId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    designation = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.employeeId);
                    table.ForeignKey(
                        name: "FK__employee__bankId__3E52440B",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employee_customer",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    transId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    accountId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    sendername = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    receiverAccountId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    transactionOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    transactionAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    modeOfTransfer = table.Column<int>(type: "int", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    transactionType = table.Column<int>(type: "int", nullable: false),
                    bankId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    otherPartyBankId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    isBankTransaction = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__transact__DB107FA7045820EB", x => x.transId);
                    table.ForeignKey(
                        name: "FK__transacti__accou__47DBAE45",
                        column: x => x.accountId,
                        principalTable: "Account",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Transacti__other__6E01572D",
                        column: x => x.otherPartyBankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Fk_transaction_bank",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_bankId",
                table: "Account",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_customerId",
                table: "Account",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "UQ__account__F3DBC5729C769750",
                table: "Account",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__bank__206168F8576BD5B7",
                table: "Bank",
                column: "bankname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currency_bankId",
                table: "Currency",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_bankId",
                table: "Employee",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_customerId",
                table: "Employee",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "UQ__employee__F3DBC57229C10B7B",
                table: "Employee",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_accountId",
                table: "Transaction",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_bankId",
                table: "Transaction",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_otherPartyBankId",
                table: "Transaction",
                column: "otherPartyBankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
