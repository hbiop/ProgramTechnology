namespace Bank;

public class InterestEarningAccount : BankAccount
{
    public InterestEarningAccount(string name, decimal initialBalance) : base(name, initialBalance)
    {
    }

    public override void PerformMonthAndTransactions()
    {
        if (this.Balance > 500m)
        {
            this.MakeDeposit(this.Balance / 100m * 2m, DateTime.Now, "Проценты по счёту");
        }
    }

}