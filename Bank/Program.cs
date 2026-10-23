namespace Bank;

internal static class Program
{
    private static void Main()
    {
        DateTime operationDate = DateTime.UtcNow;

        Console.WriteLine("=== Bank application ===");

        var yanaAccount = new BankAccount("Yana", 100_000m);
        var lenaAccount = new BankAccount("Lena", 10m);

        PrintAccount(yanaAccount);
        PrintAccount(lenaAccount);

        yanaAccount.MakeDeposit(2_000_000m, operationDate, "Salary");
        yanaAccount.MakeWithdrawal(200m, operationDate, "Purchase");
        Console.WriteLine($"Yana balance after operations: {yanaAccount.Balance:N2}");

        try
        {
            lenaAccount.MakeWithdrawal(10_000m, operationDate, "Too large withdrawal");
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine($"Expected error: {exception.Message}");
        }

        var vipAccount = new BankAccount("VIP client", 20_000m)
        {
            Status = ClientStatus.VIP
        };
        vipAccount.MakeWithdrawal(1_000m, operationDate, "VIP purchase");
        vipAccount.PerformMonthEndTransactions();
        Console.WriteLine($"VIP balance after month end: {vipAccount.Balance:N2}");

        var interestAccount = new InterestEarningAccount("Savings client", 1_000m);
        interestAccount.PerformMonthEndTransactions();
        Console.WriteLine($"Interest account balance: {interestAccount.Balance:N2}");

        var recipientAccount = new BankAccount("Recipient", 0m);
        BankTransfer.Transfer(
            yanaAccount,
            recipientAccount,
            500m,
            operationDate,
            "Monthly transfer");
        Console.WriteLine($"Transfer completed: {yanaAccount.Balance:N2} -> {recipientAccount.Balance:N2}");

        Console.WriteLine();
        Console.WriteLine("=== Yana account history ===");
        Console.WriteLine(yanaAccount.GetAccountHistory());

        Console.WriteLine("=== Audit log ===");
        Console.WriteLine(AuditLog.Dump());
    }

    private static void PrintAccount(BankAccount account)
    {
        Console.WriteLine(account);
    }
}
