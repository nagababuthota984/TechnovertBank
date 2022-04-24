using AutoMapper;
using System.Linq;
using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.Services
{
    public class TransactionService : ITransactionService
    {
        private BankStorageContext dbContext;
        private IMapper _mapper;
        public TransactionService(BankStorageContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }
        public void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, string currencyName)
        {
            AccountViewModel accountViewModel = _mapper.Map<AccountViewModel>(userAccount);
            TransactionViewModel newTransaction = new TransactionViewModel(accountViewModel, transtype, transactionamount, currencyName, false);
            dbContext.Transactions.Add(_mapper.Map<Transaction>(newTransaction));
            dbContext.SaveChanges();
        }
        public void CreateTransferTransaction(Account userAccount, Account receiverAccount, decimal transactionAmount, ModeOfTransferOptions mode, string currencyName)
        {
            //Mapping into view models.. since user-defined constructors are available in view models.
            AccountViewModel userAccountModel = _mapper.Map<AccountViewModel>(userAccount);
            AccountViewModel receiverAccountModel = _mapper.Map<AccountViewModel>(receiverAccount);
            
            //creating transactions
            TransactionViewModel senderTransaction = new TransactionViewModel(userAccountModel, receiverAccountModel, TransactionType.Transfer, transactionAmount, currencyName, mode);
            dbContext.Transactions.Add(_mapper.Map<Transaction>(senderTransaction));
            TransactionViewModel receiverTransaction = new TransactionViewModel(userAccountModel, receiverAccountModel, TransactionType.Transfer, transactionAmount, currencyName, mode);
            receiverTransaction.AccountId = receiverAccount.AccountId;
            dbContext.Transactions.Add(_mapper.Map<Transaction>(receiverTransaction));

            dbContext.SaveChanges();
        }
        public void CreateAndAddBankTransaction(Bank bank, Account userAccount, decimal charges, string currencyName)
        {
            AccountViewModel userAccountModel = _mapper.Map<AccountViewModel>(userAccount);
            BankViewModel bankViewModel = _mapper.Map<BankViewModel>(bank);

            TransactionViewModel userTransaction = new TransactionViewModel(userAccountModel, TransactionType.Debit, charges, currencyName, true);
            TransactionViewModel newBankTransaction = new TransactionViewModel(userAccountModel, bankViewModel, TransactionType.ServiceCharge, charges, currencyName);
            _mapper.Map<Transaction>(userTransaction);
            dbContext.Transactions.Add(_mapper.Map<Transaction>(newBankTransaction));
            dbContext.Transactions.Add(_mapper.Map<Transaction>(userTransaction));
            dbContext.SaveChanges();
        }
        public Transaction GetTransactionById(string transactionId)
        {
            return dbContext.Transactions.FirstOrDefault(tr => tr.TransId.Equals(transactionId,System.StringComparison.OrdinalIgnoreCase));
        }
    }
}