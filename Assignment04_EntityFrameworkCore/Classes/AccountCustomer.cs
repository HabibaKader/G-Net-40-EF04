using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04_EntityFrameworkCore.Classes
{
    public class AccountCustomer
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OwnershipStartDate { get; set; }
        public string OwnershipType { get; set; }
        public string AccountStatus { get; set; }
    }
}
