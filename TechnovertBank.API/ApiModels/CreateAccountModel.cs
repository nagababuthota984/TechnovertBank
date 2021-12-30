using System;
using TechnovertBank.Models;

namespace TechnovertBank.API.ApiModels
{
    public class CreateAccountModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public long AadharNumber { get; set; }
        public string PanNumber { get; set; }
        public AccountType Status { get; set; }
        public string BankId { get; set; }
    }
}
