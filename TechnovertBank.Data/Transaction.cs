using System;
using System.Collections.Generic;

#nullable disable

namespace TechnovertBank.Data
{
    public partial class Transaction
    {
        public string TransId { get; set; }
        public string AccountId { get; set; }
        public string Sendername { get; set; }
        public string ReceiverAccountId { get; set; }
        public DateTime TransactionOn { get; set; }
        public decimal TransactionAmount { get; set; }
        public int ModeOfTransfer { get; set; }
        public string Currency { get; set; }
        public int TransactionType { get; set; }
        public string BankId { get; set; }
        public string OtherPartyBankId { get; set; }
        public bool IsBankTransaction { get; set; }

        public virtual Account Account { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Bank OtherPartyBank { get; set; }
    }
}
