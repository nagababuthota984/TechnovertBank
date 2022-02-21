using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TechnovertBank.API.ApiModels;
using TechnovertBank.API.Authentication;
using TechnovertBank.Data;
using TechnovertBank.Models;
using TechnovertBank.Services;
using Constant = TechnovertBank.API.ApiModels.Constant;

namespace TechnovertBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;



        public AdminController(IAccountService accService, ITransactionService transService, IBankService bankService, IMapper mapper, UserManager<IdentityUser> manager, SignInManager<IdentityUser> smanager)
        {
            _accountService = accService;
            _bankService = bankService;
            _mapper = mapper;
            accountService = accService;
            transactionService = transService;
        }
        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                return Ok(_mapper.Map<List<AccountViewModel>>(_accountService.GetAllAccounts()));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getAccountById/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult GetAccountById(string id)
        {
            AccountViewModel account = _mapper.Map<AccountViewModel>(_accountService.GetAccountById(id));
            if (account != null)
                return Ok(account);
            else
                return NotFound(Constant.AccountWithIdNotFound);
        }
        [HttpGet("getAccountByAccNum/{accNumber}")]
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult GetAccountByAccNum(string accNumber)
        {
            if (long.TryParse(accNumber, out long accountNumber))
            {
                AccountViewModel account = _mapper.Map<AccountViewModel>(_accountService.GetAccountByAccNumber(accountNumber));
                if (account != null)
                    return Ok(account);
                else
                    return NotFound(Constant.AccountWithNumNotFound);
            }
            else
                return BadRequest(Constant.InvalidAccountNumFormat);
        }
        [HttpPost("createAccount")]
        public IActionResult CreateAccount(CreateAccountModel inputDetails)
        {
            try
            {
                CustomerViewModel customer = _mapper.Map<CustomerViewModel>(inputDetails);
                if (!string.IsNullOrEmpty(inputDetails.BankId) && _bankService.IsValidBank(inputDetails.BankId))
                {
                    AccountViewModel newAccount = new AccountViewModel(customer, inputDetails.AccountType, inputDetails.BankId);
                    Account createdAccount = _bankService.CreateAndAddAccount(_mapper.Map<Account>(newAccount), _mapper.Map<Customer>(customer), inputDetails.BankId);
                    return Ok(_mapper.Map<AccountViewModel>(createdAccount));
                }
                else
                    return NotFound(Constant.BankWithIdNotFound);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("updateCustomer")]
        public IActionResult UpdateAccount(UpdateAccountModel updatedCustomerDetails)
        {
            try
            {
                Customer customerModel = _mapper.Map<Customer>(updatedCustomerDetails);
                _accountService.UpdateAccount(customerModel);
                return Ok(Constant.CustomerDetailsUpdated);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deleteAccount/{id}")]
        public IActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Account acc = _accountService.GetAccountById(id);
                if (acc != null)
                {
                    _bankService.DeleteAccount(acc);
                    return Ok(Constant.AccountDeleted);
                }
                else
                    return NotFound(Constant.AccountWithIdNotFound); 
            }
            return BadRequest(Constant.InvalidBankIdFormat);
        }
        [HttpGet("getBankById/{bankId}")]
        public IActionResult GetBankById(string bankId)
        {
            if (!string.IsNullOrEmpty(bankId))
            {
                Bank bank = _bankService.GetBankById(bankId);
                if (bank != null)
                    return Ok(_mapper.Map<BankViewModel>(bank));
                else
                    return BadRequest(Constant.BankWithIdNotFound);
            }
            return BadRequest(Constant.InvalidBankIdFormat);
        }
        [HttpPost("createBank")]
        public IActionResult CreateBank(CreateBankModel inputBank)
        {
            try
            {
                Bank bank = _bankService.CreateAndGetBank(inputBank.Name, inputBank.Branch, inputBank.Ifsc);
                if(bank==null)
                {
                    return BadRequest(Constant.BankCreationFailed);
                }
                return Ok(bank);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addCurrency")]
        public IActionResult AddCurrency(CreateCurrencyModel inputCurrency) 
        {
            Bank bank = _bankService.GetBankById(inputCurrency.BankId);
            if (bank != null)
            {
                if (_bankService.AddNewCurrency(bank, inputCurrency.CurrencyName, inputCurrency.ExchangeRate))
                {
                    return Ok(Constant.CurrencyAdded);
                }
                else
                {
                    return BadRequest(Constant.CurrencyAlreadyExists);
                }
            }
            else
            {
                return BadRequest(Constant.BankWithIdNotFound);
            }
        }
        [HttpPost("addEmployee/{bankId}")]
        public IActionResult AddEmployee(AddEmployeeModel inputEmp)
        {
            try
            {
                Bank bank = _bankService.GetBankById(inputEmp.BankId);
                if (bank != null && inputEmp.Customer != null)
                {
                    Employee createdEmp = _bankService.CreateAndGetEmployee(inputEmp.Customer, inputEmp.Role, bank);
                    if (createdEmp != null)
                        return Ok(createdEmp);
                    else
                        return BadRequest(Constant.EmployeeCreationFailed);
                }
                else
                    return BadRequest(Constant.BankWithIdNotFound);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("getCustomerById/{id}")]
        public IActionResult GetCustomerById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Customer customer = accountService.GetCustomerById(id);
                if (customer != null)
                    return Ok(customer);
                return BadRequest(Constant.CustomerWithIdNotFound);
            }
            return BadRequest(Constant.InvalidCustomerIdFormat);
        }
        [HttpGet("getTransById/{transId}")]
        public IActionResult GetTransById(string transactionId)
        {
            try
            {
                Transaction trans = transactionService.GetTransactionById(transactionId);
                if (trans != null)
                    return Ok(_mapper.Map<TransactionViewModel>(trans));
                return BadRequest(Constant.TransWithMatchingIdNotFound);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getTransByDate/{date}")]
        public IActionResult GetTransactionByDate(DateTime date, string bankId)
        {
            try
            {
                List<Transaction> transactions = _bankService.GetTransactionsByDate(date, bankId);
                if (transactions != null)
                    return Ok(_mapper.Map<TransactionViewModel>(transactions));
                else
                    return BadRequest(Constant.TransWithMatchingDateNotFound);
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
                List<Transaction> transactions = _bankService.GetTransactions(bankId);
                if (transactions != null)
                    return Ok(_mapper.Map<List<TransactionViewModel>>(transactions));
                else
                    return BadRequest(Constant.TransWithMatchingBankIdNotFound);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getTransByAcc/{accountId}")]
        public IActionResult GetTransByAcc(string accountId)
        {
            try
            {
                List<Transaction> transactions = _bankService.GetAccountTransactions(accountId);
                if (transactions != null)
                {
                    return Ok(_mapper.Map<List<TransactionViewModel>>(transactions));
                }
                else
                {
                    return BadRequest(Constant.AccountWithIdNotFound);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
