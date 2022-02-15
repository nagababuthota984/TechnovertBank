using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnovertBank.Data;
using TechnovertBank.Services;

namespace TechnovertBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;
        public CustomersController(IAccountService accService, IMapper mapperObj)
        {
            accountService = accService;
            mapper = mapperObj;
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
    }
}
