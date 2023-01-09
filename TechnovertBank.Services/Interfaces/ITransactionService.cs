
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services
{
    public interface ITransactionService
    {
        void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, string currencyName);
        
        void CreateTransferTransaction(Account userAccount, Account receiverAccount, decimal transactionAmount, ModeOfTransferOptions mode, string currencyName);
        
        void CreateAndAddBankTransaction(Bank bank, Account userAccount, decimal charges, string currencyName);
        
        Transaction GetTransactionById(string transactionId);


    }
}