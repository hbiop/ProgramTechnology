namespace Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankAccount account1 = new BankAccount("Yana", 100000);
            BankAccount account2 = new BankAccount("Lena", 10);
            Console.WriteLine($"account {account1.Balance} №{account1.Number} {account1.Owner}");
            Console.WriteLine($"account {account2.Balance} №{account2.Number} {account2.Owner}");

            account1.MakeDeposit(2000000, DateTime.UtcNow, ":)");
            Console.WriteLine(account1.Balance);
            account1.MakeWithdrawal(200, DateTime.UtcNow, ":(");
            Console.WriteLine(account1.Balance);
            Console.WriteLine(account1.GetAccountHistory());
            
            try
            {
                account2.MakeWithdrawal(10000, DateTime.UtcNow, ":(");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            InterestEarningAccount acc = new InterestEarningAccount("ac", 100m);
            acc.MakeDeposit(2000, DateTime.UtcNow, ":)");
            acc.MakeWithdrawal(1000, DateTime.UtcNow, ":)");
            acc.PerformMonthAndTransactions();
            Console.WriteLine(acc.ToString());
        }
    }
}