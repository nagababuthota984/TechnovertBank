using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TechnovertBank.Data;
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
        public IActionResult GetTransByAcc(string accountId)
        {
            try
            {
                List<Transaction> transactions = bankService.GetAccountTransactions(accountId);
                if(transactions != null)
                {
                    return Ok(mapper.Map<List<TransactionViewModel>>(transactions));
                }
                else
                {
                    return BadRequest("Account with matching id not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getTransById/{transId}")]
        public IActionResult GetTransById(string transactionId)
        {
            try
            {
                Transaction trans = transactionService.GetTransactionById(transactionId);
                if (trans != null)
                    return Ok(mapper.Map<TransactionViewModel>(trans));
                else
                    return BadRequest("Transaction with matching id not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getTransByDate/{date}")]
        public IActionResult GetTransactionByDate(DateTime date,string bankId)
        {
            try
            {
                List<Transaction> transactions = bankService.GetTransactionsByDate(date, bankId);
                if (transactions != null)
                    return Ok(mapper.Map<TransactionViewModel>(transactions));
                else
                    return BadRequest("Transactions with matching date not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getBankTrans/{bankId}")]
        public IActionResult GetTransactionsOfBank(string bankId)
        {
            try
            {
                List<Transaction> transactions = bankService.GetTransactions(bankId);
                if (transactions != null)
                    return Ok(mapper.Map<List<TransactionViewModel>>(transactions));
                else
                    return BadRequest("Transactions with matching bankid not found");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
