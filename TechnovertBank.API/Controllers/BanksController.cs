using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("getBankById/bankId")]
        public BankViewModel GetBankById(string bankId)
        {
            return mapper.Map<BankViewModel>(bankService.GetBankById(bankId));
        }
        [HttpPost("createBank")]
        public void CreateBank(string name,string branch,string ifsc)
        {
            bankService.CreateAndGetBank(name, branch, ifsc);
        }
        [HttpPost("addCurrency")]
        public void AddCurrency(string currencyName, decimal exchangeRate, string bankId)
        {
            Bank bank = bankService.GetBankById(bankId);
            bankService.AddNewCurrency(bank, currencyName, exchangeRate);
        }
        [HttpPost("addEmployee/{bankId}")]
        public void AddEmployee([FromBody]CustomerViewModel newCustomer,string bankId,EmployeeDesignation role)
        {
            Bank bank = bankService.GetBankById(bankId);
            bankService.CreateAndGetEmployee(mapper.Map<Customer>(newCustomer), role, bank);
        }
    }
}
