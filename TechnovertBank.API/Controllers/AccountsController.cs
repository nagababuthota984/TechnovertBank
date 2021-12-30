using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TechnovertBank.API.ApiModels;
using TechnovertBank.Data;
using TechnovertBank.Models;
using TechnovertBank.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechnovertBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accService, IBankService bankService, IMapper mapper)
        {
            _accountService = accService;
            _bankService = bankService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            return Ok(_mapper.Map<List<AccountViewModel>>(_accountService.GetAllAccounts()));
        }
        [HttpGet("getAccountById/{id}")]
        public IActionResult GetAccountById(string id)
        {

            AccountViewModel account = _mapper.Map<AccountViewModel>(_accountService.GetAccountById(id));
            if (account != null)
                return Ok(account);
            else
                return NotFound("Account with matching id not found");


        }
        [HttpGet("getAccountByAccNum/{accNumber}")]
        public IActionResult GetAccountByAccNum(long accNumber)
        {
            AccountViewModel account = _mapper.Map<AccountViewModel>(_accountService.GetAccountByAccNumber(accNumber));
            if (account != null)
                return Ok(account);
            else
                return NotFound("Account with matching Account Number not found");
        }
        [HttpPost("createAccount")]
        public IActionResult CreateAccount(CreateAccountModel accountDetails)
        {
            BankViewModel bank = _mapper.Map<BankViewModel>(_bankService.GetBankById(accountDetails.BankId));
            CustomerViewModel customer = _mapper.Map<CustomerViewModel>(accountDetails);
            if (bank != null)
            {
                AccountViewModel newAccount = new AccountViewModel(customer, accountDetails.Status, bank);
                return Ok(_bankService.CreateAndAddAccount(_mapper.Map<Account>(newAccount), _mapper.Map<Customer>(customer), _mapper.Map<Bank>(bank)));
            }
            else
                return NotFound("Bank with matching id not found.Please give valid bank id.");
        }
        [HttpPut("updateCustomer/{accountId}")]
        public IActionResult UpdateAccount([FromBody] CustomerViewModel customer,string accountId)
        {
            //not gonna work.. should come up another way.
            _accountService.UpdateAccount(_mapper.Map<Customer>(customer));
            return Ok();
        }
        [HttpDelete("deleteAcc/{id}")]
        public IActionResult Delete(string id)
        {
            Account acc = _mapper.Map<Account>(_accountService.GetAccountById(id));
            if (acc != null)
            {
                _bankService.DeleteAccount(acc);
                return Ok();
            }
            else
                return NotFound("Account with matching id not found.Please provide a valid Account ID");

        }
        [HttpPut("deposit")]
        public IActionResult Deposit(JObject parameters)
        {
            string accountId = parameters["accountId"].ToString();
            decimal.TryParse(parameters["amount"].ToString(),out decimal amount);
            string currencyName = parameters["currencyName"].ToString();

            Account account = _accountService.GetAccountById(accountId);
            if (account != null)
            {
                Currency curr = _bankService.GetCurrencyByName(currencyName);
                if (curr != null)
                {
                    if (amount > 0)
                    {
                        _accountService.DepositAmount(account, amount, curr);
                        return Ok();
                    }
                    else
                        return BadRequest("Depositing amount should be greater than 0.");
                }
                else
                    return NotFound("Currency with matching name not found.");
            }
            else
                return NotFound("Account with matching Id not found.Please provide a valid account ID");
        }
        [HttpPut("withdraw")]
        public IActionResult Withdraw(string accountId, decimal amount)
        {
            Account account = _accountService.GetAccountById(accountId);
            if (account != null)
            {
                if (amount > 0)
                {
                    if (amount <= account.Balance)
                    {
                        _accountService.WithdrawAmount(account, amount);
                        return Ok();
                    }
                    else
                        return BadRequest("Insufficient funds.");
                }
                else
                    return BadRequest("Withdrawl amount should be greater than 0.");
            }
            else
                return NotFound("Account with matching Id not found");
        }
        [HttpPut("transfer")]
        public IActionResult Transfer(string senderAccId, long receiverAccNumber, decimal amount, ModeOfTransferOptions mode)
        {
            Account senderAccount = _accountService.GetAccountById(senderAccId);
            if (senderAccount != null)
            {
                Account receiverAccount = _accountService.GetAccountByAccNumber(receiverAccNumber);
                if (receiverAccount != null)
                {
                    if (amount > 0)
                    {
                        if (amount >= senderAccount.Balance)
                        {
                            Bank senderBank = _bankService.GetBankById(senderAccount.BankId);
                            _accountService.TransferAmount(senderAccount, senderBank, receiverAccount, amount, mode);
                            return Ok();
                        }
                        else
                            return BadRequest("Insufficient funds.");
                    }
                    else
                        return BadRequest("Amount should be greater than 0.");
                }
                else
                    return NotFound("Receiver Account with matching Account Number not found");
            }
            else
                return NotFound("Account with matching Account Number not found");
        }
    }
}
