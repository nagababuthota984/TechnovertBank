
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services
{
    public class AccountService : IAccountService
    {
        private ITransactionService transService;
        private BankStorageContext dbContext;
        private IMapper mapper;
        public AccountService(ITransactionService transactionService, BankStorageContext context, IMapper mapperObject)
        {
            transService = transactionService;
            dbContext = context;
            mapper = mapperObject;
        }
        public bool IsValidCustomer(string userName, string password)
        {
            var acc = dbContext.Accounts.FirstOrDefault(acc => acc.Username.Equals(userName) && acc.Password.Equals(password) && acc.Status == (int)AccountStatus.Active);
            if (acc == null)
                return false;
            else
            {
                InitializeSessionContext(acc);
                return true;
            }
        }
        private void InitializeSessionContext(Account acc)
        {
            SessionContext.Account = acc;
            SessionContext.Bank = mapper.Map<Bank>(dbContext.Banks.FirstOrDefault(b => b.BankId.Equals(acc.BankId)));
        }
        public Account GetAccountByAccNumber(long accNumber)
        {
            return dbContext.Accounts.FirstOrDefault(ac => ac.AccountNumber.Equals(accNumber));
        }
        public Account GetAccountById(string accountId)
        {
            return dbContext.Accounts.FirstOrDefault(ac => ac.AccountId.Equals(accountId));
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
            decimal charges = 0;
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

        public List<Account> GetAllAccounts()
        {
            return mapper.Map<List<Account>>(dbContext.Accounts);
        }
        public void UpdateAccount(Customer updatedCustomer)
        {
            dbContext.Customers.Update(updatedCustomer);
            dbContext.SaveChanges();

        }
    }
}

