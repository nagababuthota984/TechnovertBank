using System;
using System.Collections.Generic;

#nullable disable

namespace TechnovertBank.Data
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Employees = new HashSet<Employee>();
        }

        public string CustomerId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public long AadharNumber { get; set; }
        public string PanNumber { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
