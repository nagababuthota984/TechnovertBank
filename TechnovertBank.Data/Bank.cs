using System;
using System.Collections.Generic;

#nullable disable

namespace TechnovertBank.Data
{
    public partial class Bank
    {
        public Bank()
        {
            Accounts = new HashSet<Account>();
            Currencies = new HashSet<Currency>();
            Employees = new HashSet<Employee>();
            TransactionBanks = new HashSet<Transaction>();
            TransactionOtherPartyBanks = new HashSet<Transaction>();
        }

        public string BankId { get; set; }
        public string Bankname { get; set; }
        public string Branch { get; set; }
        public string Ifsc { get; set; }
        public decimal SelfRtgs { get; set; }
        public decimal SelfImps { get; set; }
        public decimal OtherRtgs { get; set; }
        public decimal OtherImps { get; set; }
        public decimal Funds { get; set; }
        public string DefaultCurrencyName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Transaction> TransactionBanks { get; set; }
        public virtual ICollection<Transaction> TransactionOtherPartyBanks { get; set; }
    }
}
