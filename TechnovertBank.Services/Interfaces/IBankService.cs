
using System;
using System.Collections.Generic;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services
{
    public interface IBankService
    {
        Bank CreateAndGetBank(string name, string branch, string ifsc);
        bool IsValidEmployee(string userName, string password);
        Account CreateAndAddAccount(Account newAccount, Customer customer, Bank bank);
        bool DeleteAccount(Account userAccount);
        bool AddNewCurrency(Bank bank, string newName, decimal exchangeRate);
        bool ModifyServiceCharge(ModeOfTransferOptions mode, bool isSelfBankCharge, Bank bank, decimal newValue);
        List<Transaction> GetAccountTransactions(string accountId);
        bool RevertTransaction(Transaction transaction, Bank bank);
        Employee CreateAndGetEmployee(Customer customer, EmployeeDesignation role, Bank bank);
        List<Transaction> GetTransactionsByDate(DateTime date, string bankId);
        List<Transaction> GetTransactions(string bankId);
        Bank GetBankById(string bankid);
        Currency GetCurrencyByName(string currencyName);
    }
}