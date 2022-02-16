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
        private readonly UserManager<BankUser> userManager;
        private readonly SignInManager<BankUser> signInManager;
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;



        public AdminController(IAccountService accService, ITransactionService transService, IBankService bankService, IMapper mapper, UserManager<BankUser> manager, SignInManager<BankUser> smanager)
        {
            _accountService = accService;
            _bankService = bankService;
            _mapper = mapper;
            userManager = manager;
            signInManager = smanager;
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
                return NotFound("Account with matching id not found");
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
                    return NotFound("Account with matching Account Number not found");
            }
            else
                return BadRequest("Account number should not contain letters or special characters. Please enter a valid Account Number");
        }
        [HttpPost("createAccount")]
        [Authorize(Roles = UserRoles.Admin)]
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
                    return NotFound("Bank with matching id not found.Please give valid bank id.");
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
                _accountService.UpdateAccount(_mapper.Map<Customer>(customerModel));
                return Ok("Customer details updated successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deleteAcc/{id}")]
        public IActionResult Delete(string id)
        {
            Account acc = _accountService.GetAccountById(id);
            if (acc != null)
            {
                _bankService.DeleteAccount(acc);
                return Ok("Account has been deleted");
            }
            else
                return NotFound("Account with matching id not found.Please provide a valid Account ID");
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
                    return BadRequest("Bank with matching Id not found");
            }
            return BadRequest("Invalid bank Id format.Please enter a valid bankID");
        }
        [HttpPost("createBank")]
        public IActionResult CreateBank(CreateBankModel inputBank)
        {
            try
            {
                return Ok(_bankService.CreateAndGetBank(inputBank.Name, inputBank.Branch, inputBank.Ifsc));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addCurrency")]
        public IActionResult AddCurrency(CreateCurrencyModel inputCurrency) //string currencyName, decimal exchangeRate, string bankId)
        {
            Bank bank = _bankService.GetBankById(inputCurrency.BankId);
            if (bank != null)
            {
                if (_bankService.AddNewCurrency(bank, inputCurrency.CurrencyName, inputCurrency.ExchangeRate))
                {
                    return Ok("Currency added successfully");
                }
                else
                {
                    return BadRequest("Sorry! Unable to add currency. Please try again");
                }
            }
            else
            {
                return BadRequest("Bank with matching id not found.");
            }
        }
        [HttpPost("addEmployee/{bankId}")]
        public IActionResult AddEmployee(AddEmployeeModel inputEmp)//[FromBody]CustomerViewModel newCustomer,string bankId,EmployeeDesignation role)
        {
            Bank bank = _bankService.GetBankById(inputEmp.BankId);
            if (bank != null && inputEmp.Customer != null)
            {
                Employee createdEmp = _bankService.CreateAndGetEmployee(inputEmp.Customer, inputEmp.Role, bank);
                if (createdEmp != null)
                    return Ok(createdEmp);
                else
                    return BadRequest("Employee not created. Please try again!");
            }
            else
                return BadRequest("Bank with matching id not found.Please provide valid details of the customer.");
        }
        [HttpGet("getCustomerById/{id}")]
        public IActionResult GetCustomerById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Customer customer = accountService.GetCustomerById(id);
                if (customer != null)
                    return Ok(customer);
                else
                    return BadRequest("Customer with matching id not found");
            }
            else
                return BadRequest("Please provide a valid customer id");
        }
        [HttpGet("getTransById/{transId}")]
        public IActionResult GetTransById(string transactionId)
        {
            try
            {
                Transaction trans = transactionService.GetTransactionById(transactionId);
                if (trans != null)
                    return Ok(_mapper.Map<TransactionViewModel>(trans));
                else
                    return BadRequest("Transaction with matching id not found");
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
                List<Transaction> transactions = _bankService.GetTransactions(bankId);
                if (transactions != null)
                    return Ok(_mapper.Map<List<TransactionViewModel>>(transactions));
                else
                    return BadRequest("Transactions with matching bankid not found");
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
                    return BadRequest("Account with matching id not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
