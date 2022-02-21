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

            if (!string.IsNullOrEmpty(inputDetails.AccountId) && !string.IsNullOrEmpty(inputDetails.CurrencyName))
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
                            return Ok(Constant.DepositSuccess);
                        }
                        return BadRequest(Constant.AmountMustAboveZero);
                    }
                    return NotFound(Constant.CurrencyWithMatchingNameNotFound);
                }
                return NotFound(Constant.AccountWithIdNotFound);
            }
            return BadRequest(Constant.InvalidAccountIdFormat);
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
                            return Ok(Constant.WithdrawlSuccess);
                        }
                        else
                            return BadRequest(Constant.InsufficientFunds);
                    }
                    else
                        return BadRequest(Constant.AmountMustAboveZero);
                }
                else
                    return NotFound(Constant.AccountWithIdNotFound);
            }
            else
            {
                return BadRequest(Constant.InvalidAccountIdFormat);
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
                    Account receiverAccount = _accountService.GetAccountByAccNumber(inputDetails.ReceiverAccountNumber);
                    if (receiverAccount != null)
                    {
                        if (inputDetails.Amount > 0)
                        {
                            if (inputDetails.Amount <= senderAccount.Balance)
                            {
                                Bank senderBank = _bankService.GetBankById(senderAccount.BankId);
                                _accountService.TransferAmount(senderAccount, senderBank, receiverAccount, inputDetails.Amount, inputDetails.Mode);
                                return Ok(Constant.AmountTransferSuccess);
                            }
                            else
                                return BadRequest(Constant.InsufficientFunds);
                        }
                        else
                            return BadRequest(Constant.AmountMustAboveZero);
                    }
                    else
                        return NotFound($"Receiver {Constant.AccountWithNumNotFound}");
                }
                else
                    return NotFound(Constant.AccountWithNumNotFound);
            }
            else
            {
                return BadRequest(Constant.InvalidAccountIdFormat);
            }
        }

    }
}
