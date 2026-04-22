using Microsoft.EntityFrameworkCore;

namespace Assignment04_EntityFrameworkCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new BankDbContext();

            context.Database.Migrate();

            Console.WriteLine("Database created & migrated successfully!");
        }
    }
}
