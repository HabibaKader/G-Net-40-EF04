using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment04_EntityFrameworkCore.Classes
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public DateTime OpeningDate { get; set; }
        public decimal CurrentBalance { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<AccountCustomer> AccountCustomers { get; set; } = new List<AccountCustomer>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
