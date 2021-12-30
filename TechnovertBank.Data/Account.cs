using System;
using System.Collections.Generic;

#nullable disable

namespace TechnovertBank.Data
{
    public partial class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        public string AccountId { get; set; }
        public string BankId { get; set; }
        public long AccountNumber { get; set; }
        public int AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public string CustomerId { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
