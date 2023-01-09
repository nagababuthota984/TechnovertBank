using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITransactionService transService;
        private readonly BankStorageContext dbContext;
        private readonly IMapper mapper;

        #region Constructor
		public AccountService(ITransactionService transactionService, BankStorageContext context, IMapper mapper)
        {
            this.transService = transactionService;
            dbContext = context;
            this.mapper = mapper;
        } 

	    #endregion

        public bool IsValidCustomer(string userName, string password)
        {
            var acc = dbContext.Accounts.FirstOrDefault(acc => acc.Username.Equals(userName) && acc.Password.Equals(password) && acc.Status == (int)AccountStatus.Active);
            if (acc == null) return false;
            InitializeSessionContext(acc);
            return true;
        }

        private void InitializeSessionContext(Account loggedInAccount)
        {
            SessionContext.Account = loggedInAccount;
            SessionContext.Bank = mapper.Map<Bank>(dbContext.Banks.FirstOrDefault(b => b.BankId.Equals(loggedInAccount.BankId)));
        }
        
        public void DepositAmount(Account userAccount, decimal amount, Currency currency)
        {
            amount *= currency.ExchangeRate;
            userAccount.Balance += amount;
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency.Name);
            dbContext.SaveChanges();
        }
        
        public void WithdrawAmount(Account userAccount, decimal amount)
        {
            userAccount.Balance -= amount;
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, "INR");//SessionContext.Bank.DefaultCurrencyName);
            dbContext.SaveChanges();
        }
        
        public void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransferOptions mode)
        {

            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;
            dbContext.Accounts.Update(mapper.Map<Account>(senderAccount));
            dbContext.Accounts.Update(mapper.Map<Account>(receiverAccount));
            ApplyTransferCharges(senderAccount, senderBank, receiverAccount.BankId, amount, mode, "INR");//SessionContext.Bank.DefaultCurrencyName);
            transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, "INR");// SessionContext.Bank.DefaultCurrencyName);
            dbContext.SaveChanges();
        }
        
        public void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransferOptions mode, string currencyName)
        {
            decimal charges;
            if (mode == ModeOfTransferOptions.RTGS)
            {
                charges = senderAccount.BankId.Equals(receiverBankId) ? (senderBank.SelfRtgs * amount) / 100 : (senderBank.OtherRtgs * amount) / 100;
            }
            else
            {
                charges = senderAccount.BankId.Equals(receiverBankId) ? (senderBank.SelfImps * amount) / 100 : (senderBank.OtherImps * amount) / 100;
            }
            senderAccount.Balance -= charges;
            senderBank.Funds += charges;
            transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
            dbContext.SaveChanges();
        }

        public void UpdateAccount(Customer updatedCustomer)
        {
            Customer originalCustomer = dbContext.Customers.FirstOrDefault(cus=>cus.AadharNumber==updatedCustomer.AadharNumber);
            foreach (var property in typeof(Customer).GetProperties())
            {
                if(property.GetValue(updatedCustomer)!=null)
                    property.SetValue(originalCustomer, property.GetValue(updatedCustomer));
            }
            dbContext.SaveChanges();

        }

        #region Getters

		public List<Account> GetAllAccounts()
        {
            return mapper.Map<List<Account>>(dbContext.Accounts);
        }
        
        public Account GetAccountByAccNumber(long accNumber)
        {
            return dbContext.Accounts.FirstOrDefault(ac => ac.AccountNumber.Equals(accNumber));
           
        }
        
        public Customer GetCustomerById(string accountId)
        {
            return dbContext.Customers.FirstOrDefault(cust => cust.CustomerId.Equals(accountId));
        }
        
        public Account GetAccountById(string accountId)
        {
            return dbContext.Accounts.FirstOrDefault(ac => ac.AccountId.Equals(accountId));
            
        } 

	    #endregion
    }
}

