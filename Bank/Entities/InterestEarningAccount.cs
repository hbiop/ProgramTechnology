namespace Bank;

/// <summary>
/// Представляет сберегательный счёт, на остаток которого в конце месяца автоматически начисляются проценты.
/// </summary>
public class InterestEarningAccount: BankAccount
{
    /// <summary>
    /// Инициализирует новый экземпляр сберегательного счёта с указанием имени владельца и начального баланса.
    /// </summary>
    public InterestEarningAccount(string name, decimal initialBalance) : base(name, initialBalance) { }

    /// <summary>
    /// Проверяет остаток на счёте в конце месяца и начисляет бонусные 2% годовых, если баланс превышает 500 единиц.
    /// </summary>
    public override void PerformMonthEndTransactions()
    {
        if (Balance > 500m)
        {
            decimal interest = Balance * 0.02m;
            MakeDeposit(interest, DateTime.UtcNow, "apply mountly interest");

        }
    }
  
}