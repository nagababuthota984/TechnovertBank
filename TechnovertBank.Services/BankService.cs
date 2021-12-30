using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services
{
    public class BankService : IBankService
    {
        private IAccountService accountService;
        private BankStorageContext dbContext;
        private IMapper mapper;
        public BankService(IAccountService accService, BankStorageContext context, IMapper mapperObject)
        {
            accountService = accService;
            dbContext = context;
            mapper = mapperObject;
        }
        public Bank CreateAndGetBank(string name, string branch, string ifsc)
        {
            BankViewModel newBank = new BankViewModel(name, branch, ifsc);
            CurrencyViewModel currency = new CurrencyViewModel("INR", 1, newBank.BankId);
            CustomerViewModel customer = new CustomerViewModel("Admin", newBank.BankId);
            EmployeeViewModel employee = new EmployeeViewModel(newBank.BankId, "Admin", "admin", customer.CustomerId);
            dbContext.Banks.Add(mapper.Map<Bank>(newBank));
            dbContext.Customers.Add(mapper.Map<Customer>(customer));
            dbContext.Currencies.Add(mapper.Map<Currency>(currency));
            dbContext.Employees.Add(mapper.Map<Employee>(employee));
            dbContext.SaveChanges();
            return mapper.Map<Bank>(newBank);
        }
        public Employee CreateAndGetEmployee(Customer newCustomer, EmployeeDesignation role, Bank bank)
        {
            EmployeeViewModel employee = new EmployeeViewModel(mapper.Map<CustomerViewModel>(newCustomer), role, mapper.Map<BankViewModel>(bank));
            dbContext.Customers.Add(mapper.Map<Customer>(newCustomer));
            dbContext.Employees.Add(mapper.Map<Employee>(employee));
            dbContext.SaveChanges();
            return mapper.Map<Employee>(employee);
        }
        public bool IsValidEmployee(string userName, string password)
        {
            Employee emp = dbContext.Employees.FirstOrDefault(e => e.Username.Equals(userName) && e.Password.Equals(password)); ;
            if (emp == null)
                return false;
            else
            {
                InitializeEmployeeSessionContext(emp);
                return true;
            }
        }
        private void InitializeEmployeeSessionContext(Employee emp)
        {
            SessionContext.Employee = emp;
            SessionContext.Bank = GetBankById(emp.BankId);
        }
        public void UpdateAccount(CustomerViewModel customer)
        {
            dbContext.Customers.Update(mapper.Map<Customer>(customer));
            dbContext.SaveChanges();
        }
        public Account CreateAndAddAccount(Account newAccount, Customer newCustomer, Bank bank)
        {
            newAccount.BankId = bank.BankId;
            newAccount.AccountNumber = GenerateAccountNumber();
            newAccount.CustomerId = newCustomer.CustomerId;
            dbContext.Accounts.Add(newAccount);
            dbContext.Customers.Add(newCustomer);
            dbContext.SaveChanges();
            return dbContext.Accounts.FirstOrDefault(acc => acc.AccountId.Equals(newAccount.AccountId));

        }
        public Bank GetBankById(string bankId)
        {
            Bank bank = dbContext.Banks.FirstOrDefault(b => b.BankId.Equals(bankId));
            return bank;
        }
        public List<Transaction> GetTransactionsByDate(DateTime date, string bankId)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.AddRange(dbContext.Transactions.Where(tr => tr.TransactionOn.Date == date && tr.BankId.Equals(bankId)));
            return transactions;
        }
        public List<Transaction> GetAccountTransactions(string accountId)
        {
            return dbContext.Transactions.Where(tr => tr.AccountId.Equals(accountId)).ToList();
        }
        public List<Transaction> GetTransactions(string bankId)
        {
            return dbContext.Transactions.Where(tr => (tr.IsBankTransaction) && tr.BankId.Equals(bankId)).ToList();
        }
        public bool AddNewCurrency(Bank bank, string newCurrencyName, decimal exchangeRate)
        {
            if (dbContext.Currencies.Any(c => c.BankId.Equals(bank.BankId) && c.Name.Equals(newCurrencyName)))
            {
                return false;
            }
            dbContext.Currencies.Add(mapper.Map<Currency>(new CurrencyViewModel(newCurrencyName, exchangeRate, bank.BankId)));
            dbContext.SaveChanges();
            return true;
        }
        public bool DeleteAccount(Account userAccount)
        {
            userAccount.Status = (int)AccountStatus.Closed;
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ModifyServiceCharge(ModeOfTransferOptions mode, bool isSelfBankCharge, Bank bank, decimal newValue)
        {
            if (isSelfBankCharge)
            {
                if (mode == ModeOfTransferOptions.RTGS)
                {
                    bank.SelfRtgs = newValue;
                }
                else
                {
                    bank.SelfImps = newValue;
                }
            }
            else
            {
                if (mode == ModeOfTransferOptions.RTGS)
                {
                    bank.OtherRtgs = newValue;
                }
                else
                {
                    bank.OtherImps = newValue;
                }
            }
            try
            {
                dbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool RevertTransaction(Transaction transaction, Bank bank)
        {
            Account userAccount = accountService.GetAccountById(transaction.AccountId);
            if (transaction.TransactionType == (int)TransactionType.Credit)
            {
                accountService.WithdrawAmount(userAccount, transaction.TransactionAmount);
            }
            else if (transaction.TransactionType == (int)TransactionType.Debit)
            {
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, dbContext.Currencies.FirstOrDefault(c => c.Name.Equals(bank.DefaultCurrencyName)));
            }
            else if (transaction.TransactionType == (int)TransactionType.Transfer)
            {
                Account receiverAccount = accountService.GetAccountById(transaction.AccountId);
                accountService.WithdrawAmount(receiverAccount, transaction.TransactionAmount);
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, dbContext.Currencies.FirstOrDefault(c => c.Name.Equals(bank.DefaultCurrencyName)));
            }
            dbContext.SaveChanges();
            return true;
        }
        private long GenerateAccountNumber()
        {
            long lastAccNumber = dbContext.Accounts.LastOrDefault()?.AccountNumber ?? 1000;
            if(lastAccNumber==0)
            {
                return 1000;
            }
            else
            {
                return lastAccNumber + 1;
            }

        }
        
        public Currency GetCurrencyByName(string currencyName)
        {
            return dbContext.Currencies.FirstOrDefault(cr=>cr.Name==currencyName);
        }
    }
}






