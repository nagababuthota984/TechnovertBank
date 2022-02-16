using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnovertBank.API.ApiModels;
using TechnovertBank.API.Authentication;
using TechnovertBank.Data;
using TechnovertBank.Services;

namespace TechnovertBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles=UserRoles.User)]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        public UserController(IAccountService accService,IBankService bnkService)
        {
            _accountService = accService;
            _bankService = bnkService;
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
