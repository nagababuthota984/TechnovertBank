using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TechnovertBank.Models;
using TechnovertBank.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankAppDbFirstApproach.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IBankService bankService;
        private readonly IAccountService accountService;
        private readonly IMapper mapper;
        private readonly ITransactionService transactionService;

        public TransactionsController(IBankService bnkService, IAccountService accService, ITransactionService transService,IMapper _mapper)
        {
            bankService = bnkService;
            accountService = accService;
            mapper = _mapper;
            transactionService = transService;
        }
        [HttpGet("getTransByAcc/{accountId}")]
        public List<TransactionViewModel> GetTransByAcc(string accountId)
        {
            return mapper.Map<List<TransactionViewModel>>(bankService.GetAccountTransactions(accountId));
        }

        [HttpGet("getTransById/{transId}")]
        public TransactionViewModel GetTransById(string transactionId)
        {
            return mapper.Map<TransactionViewModel>(transactionService.GetTransactionById(transactionId));
        }
        [HttpGet("getTransByDate/{date}")]
        public List<TransactionViewModel> GetTransactionByDate(DateTime date,string bankId)
        {
            return mapper.Map<List<TransactionViewModel>>(bankService.GetTransactionsByDate(date,bankId));
        }
        [HttpGet("getBankTrans/{bankId}")]
        public List<TransactionViewModel> GetTransactionsOfBank(string bankId)
        {
            return mapper.Map<List<TransactionViewModel>>(bankService.GetTransactions(bankId));
        }

    }
}
