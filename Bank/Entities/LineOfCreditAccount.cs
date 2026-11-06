namespace Bank;

/// <summary>
/// Представляет кредитный счёт, позволяющий балансу уходить в минус до достижения установленного лимита кредитования.
/// </summary>
internal class LineOfCreditAccount : BankAccount
{
    /// <summary>
    /// Инициализирует новый экземпляр кредитного счёта с указанием имени владельца, начального баланса и кредитного лимита.
    /// </summary>
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit)
        : base(name, initialBalance, -creditLimit)
    {

    }

    /// <summary>
    /// Проверяет наличие задолженности в конце месяца и списывает комиссию в размере 7% от суммы долга.
    /// </summary>
    public override void PerformMonthEndTransactions()
    {
        if (Balance < 0)
        {
            decimal interest = -Balance * 0.07m;
            MakeWithdrawal(interest, DateTime.UtcNow, "Charge monthly interest");
        }
    }

    /// <summary>
    /// Проверяет выход за рамки доступного баланса и возвращает транзакцию штрафа в размере 20 единиц при овердрафте.
    /// </summary>
    protected override Transaction? CheckWithdrawalLimit(bool isOverdrawn)
        => isOverdrawn ? new Transaction(-20, DateTime.UtcNow, "Apply overdraft fee") : null;
}