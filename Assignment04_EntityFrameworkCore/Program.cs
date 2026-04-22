using Assignment04_EntityFrameworkCore.Classes;
using Microsoft.EntityFrameworkCore;

namespace Assignment04_EntityFrameworkCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new BankDbContext();

            context.Database.Migrate();
            while (true)
            {
                Console.Clear();
                PrintMenu();

                Console.Write("Select option: ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    ShowError();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddCustomer(context);
                        break;
                    case 2:
                        OpenAccount(context);
                        break;
                    case 3:
                        UpdateAccountStatus(context);
                        break;
                    case 4:
                        RemoveAccountFromCustomer(context);
                        break;
                    case 5:
                        ListCustomers(context);
                        break;
                    case 0:
                        return;
                    default:
                        ShowError();
                        break;
                }

                Pause();
            }
        }
        static void PrintMenu()
        {
            Console.WriteLine("==== Bank System Menu ====");
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. Open Account");
            Console.WriteLine("3. Update Account Status");
            Console.WriteLine("4. Remove Account from Customer");
            Console.WriteLine("5. List Customers with Accounts");
            Console.WriteLine("0. Exit");
        }
        static void ShowError()
        {
            Console.WriteLine("Invalid input. Try again.");
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
        static void AddCustomer(BankDbContext context)
        {
            Console.Write("Full Name: ");
            var name = Console.ReadLine();

            Console.Write("National ID: ");
            var nationalId = Console.ReadLine();

            Console.Write("DOB (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var dob))
            {
                ShowError(); return;
            }

            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("Phone: ");
            var phone = Console.ReadLine();

            Console.Write("Address: ");
            var address = Console.ReadLine();

            Console.Write("Type (Individual/Business): ");
            var type = Console.ReadLine();

            var customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = type
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            Console.WriteLine("Customer added successfully.");
        }
        static void OpenAccount(BankDbContext context)
        {
            Console.Write("Account Number: ");
            var accNumber = Console.ReadLine();

            Console.Write("Account Type: ");
            var type = Console.ReadLine();

            Console.Write("Branch Code: ");
            var branchCode = Console.ReadLine();

            var branch = context.Branches.FirstOrDefault(b => b.Code == branchCode);
            if (branch == null)
            {
                Console.WriteLine("Branch not found.");
                return;
            }

            Console.Write("Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                ShowError(); return;
            }

            var customer = context.Customers.Find(customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.Write("Ownership (Primary/CoHolder): ");
            var ownership = Console.ReadLine();

            var account = new Account
            {
                AccountNumber = accNumber,
                AccountType = type,
                OpeningDate = DateTime.Now,
                CurrentBalance = 0,
                BranchId = branch.Id
            };

            context.Accounts.Add(account);
            context.SaveChanges();

            var link = new AccountCustomer
            {
                AccountId = account.Id,
                CustomerId = customerId,
                OwnershipStartDate = DateTime.Now,
                OwnershipType = ownership,
                AccountStatus = "Active"
            };

            context.AccountCustomers.Add(link);
            context.SaveChanges();

            Console.WriteLine("Account created successfully.");
        }
        static void UpdateAccountStatus(BankDbContext context)
        {
            Console.Write("Account Number: ");
            var accNumber = Console.ReadLine();

            Console.Write("Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                ShowError(); return;
            }

            var link = context.AccountCustomers
                .FirstOrDefault(ac => ac.Account.AccountNumber == accNumber && ac.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine("Record not found.");
                return;
            }

            link.AccountStatus = link.AccountStatus == "Active" ? "Closed" : "Active";

            context.SaveChanges();

            Console.WriteLine("Status updated.");
        }
        static void RemoveAccountFromCustomer(BankDbContext context)
        {
            Console.Write("Account Number: ");
            var accNumber = Console.ReadLine();

            Console.Write("Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                ShowError(); return;
            }

            var link = context.AccountCustomers
                .FirstOrDefault(ac => ac.Account.AccountNumber == accNumber && ac.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine("Not found.");
                return;
            }

            context.AccountCustomers.Remove(link);
            context.SaveChanges();

            Console.WriteLine("Removed successfully.");
        }
        static void ListCustomers(BankDbContext context)
        {
            var customers = context.Customers
                .Select(c => new
                {
                    c.FullName,
                    Accounts = c.AccountCustomers.Select(ac => new
                    {
                        ac.Account.AccountNumber,
                        ac.Account.AccountType,
                        ac.Account.CurrentBalance,
                        ac.OwnershipType,
                        ac.AccountStatus
                    })
                })
                .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"\nCustomer: {c.FullName}");

                foreach (var acc in c.Accounts)
                {
                    Console.WriteLine($"  - {acc.AccountNumber} | {acc.AccountType} | {acc.CurrentBalance} | {acc.OwnershipType} | {acc.AccountStatus}");
                }
            }
        }
    }
}
