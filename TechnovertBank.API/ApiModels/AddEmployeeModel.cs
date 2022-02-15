using TechnovertBank.Data;
using TechnovertBank.Models;

namespace TechnovertBank.API.ApiModels
{
    public class AddEmployeeModel
    {
        public Customer Customer { get; set; }
        public EmployeeDesignation Role { get; set; }
        public string BankId { get; set; }
    }
}
