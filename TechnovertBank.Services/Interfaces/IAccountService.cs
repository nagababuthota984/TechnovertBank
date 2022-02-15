using System.Collections.Generic;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services

{
    public interface IAccountService
    {
        bool IsValidCustomer(string userName, string password);
        Account GetAccountByAccNumber(long accNumber);
        Account GetAccountById(string accountId);
        void DepositAmount(Account userAccount, decimal amount, Currency currency);
        void WithdrawAmount(Account userAccount, decimal amount);
        void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransferOptions mode);
        void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransferOptions mode, string currencyName);
        List<Account> GetAllAccounts();
        void UpdateAccount(Customer customer);
        Customer GetCustomerById(string accountId);
    }
}