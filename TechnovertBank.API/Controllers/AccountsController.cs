using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;
        private readonly UserManager<BankUser> userManager;
        private readonly SignInManager<BankUser> signInManager;
         

        public AccountsController(IAccountService accService, IBankService bankService, IMapper mapper,UserManager<BankUser> manager,SignInManager<BankUser> smanager)
        {
            _accountService = accService;
            _bankService = bankService;
            _mapper = mapper;
            userManager = manager;
            signInManager = smanager;
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
                CustomerViewModel customerModel = _mapper.Map<CustomerViewModel>(updatedCustomerDetails);
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
        [HttpPut("deposit")]
        public IActionResult Deposit(UpdateBalanceModel inputDetails)
        {

            Account account = _accountService.GetAccountById(inputDetails.AccountId);
            if (account != null)
            {
                Currency curr = _bankService.GetCurrencyByName(inputDetails.CurrencyName);
                if (curr != null)
                {
                    if (inputDetails.Amount > 0)
                    {
                        _accountService.DepositAmount(account, inputDetails.Amount, curr);
                        return Ok("Deposited Successfully!");
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
        public IActionResult Withdraw(UpdateBalanceModel inputDetails)
        {
            if (!string.IsNullOrEmpty(inputDetails.AccountId))
            {
                Account account = _accountService.GetAccountById(inputDetails.AccountId);
                if (account != null)
                {
                    if (inputDetails.Amount > 0)
                    {
                        if (inputDetails.Amount <= account.Balance)
                        {
                            _accountService.WithdrawAmount(account, inputDetails.Amount);
                            return Ok("Withdrawn successfully!");
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
            else
            {
                return BadRequest("Invalid account Id. Please provide a valid accountId");
            }
        }
        [HttpPut("transfer")]
        public IActionResult Transfer(TransferAmountModel inputDetails)
        {
            if (!string.IsNullOrEmpty(inputDetails.SenderAccountId))
            {
                Account senderAccount = _accountService.GetAccountById(inputDetails.SenderAccountId);
                if (senderAccount != null)
                {
                    Account receiverAccount = _accountService.GetAccountByAccNumber(inputDetails.ReceiverAccountNumber);// check this (acc not returning)
                    if (receiverAccount != null)
                    {
                        if (inputDetails.Amount > 0)
                        {
                            if (inputDetails.Amount <= senderAccount.Balance)
                            {
                                Bank senderBank = _bankService.GetBankById(senderAccount.BankId);
                                _accountService.TransferAmount(senderAccount, senderBank, receiverAccount, inputDetails.Amount, inputDetails.Mode);
                                return Ok("Amount transferred successfully");
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
            else
            {
                return BadRequest("Invalid account Id format");
            }
        }
    }
}
