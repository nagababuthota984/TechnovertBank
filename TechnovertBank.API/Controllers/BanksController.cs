using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using TechnovertBank.API.ApiModels;
using TechnovertBank.Data;
using TechnovertBank.Models;
using TechnovertBank.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechnovertBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBankService bankService;
        public BanksController(IBankService _bankService,IMapper _mapperObj)
        {
            mapper = _mapperObj;
            bankService = _bankService;
        }
        [HttpGet("getBankById/{bankId}")]
        public IActionResult GetBankById(string bankId)
        {
            if (!string.IsNullOrEmpty(bankId))
            {
                Bank bank = bankService.GetBankById(bankId);
                if (bank != null)
                    return Ok(mapper.Map<BankViewModel>(bank));
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
                return Ok(bankService.CreateAndGetBank(inputBank.Name,inputBank.Branch,inputBank.Ifsc));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addCurrency")]
        public IActionResult AddCurrency(CreateCurrencyModel inputCurrency) //string currencyName, decimal exchangeRate, string bankId)
        {
            Bank bank = bankService.GetBankById(inputCurrency.BankId);
            if (bank != null)
            {
                if(bankService.AddNewCurrency(bank, inputCurrency.CurrencyName, inputCurrency.ExchangeRate))
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
            Bank bank = bankService.GetBankById(inputEmp.BankId);
            if (bank != null && inputEmp.Customer!=null)
            {
                Employee createdEmp = bankService.CreateAndGetEmployee(inputEmp.Customer, inputEmp.Role, bank);
                if (createdEmp != null)
                    return Ok(createdEmp);
                else
                    return BadRequest("Employee not created. Please try again!");
            }
            else
                return BadRequest("Bank with matching id not found.Please provide valid details of the customer.");
        }
    }
}
