namespace Bank;


/// <summary>
/// Представляет счёт подарочной карты, который может автоматически пополняться раз в месяц на фиксированную сумму.
/// </summary>
internal class GiftCartAccount : BankAccount
{
    /// <summary>
    /// Размер фиксированного ежемесячного пополнения, установленный для данной карты.
    /// </summary>
    private readonly decimal _monthlyDeposit = 0m;

    /// <summary>
    /// Инициализирует новый экземпляр подарочного счёта с указанием владельца, начальной суммы и настраиваемого ежемесячного пополнения.
    /// </summary>
    public GiftCartAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0)
        : base(name, initialBalance) => _monthlyDeposit = monthlyDeposit;

    /// <summary>
    /// Проверяет настройки карты в конце месяца и автоматически зачисляет фиксированную сумму, если она больше нуля.
    /// </summary>
    public override void PerformMonthEndTransactions()
    {
        if (_monthlyDeposit != 0)
        {
            MakeDeposit(_monthlyDeposit, DateTime.UtcNow, "Add monthly deposit");
        }
    }

    /// <summary>
    /// Возвращает текстовое описание информации о счёте, дополненное сведениями о размере ежемесячного зачисления.
    /// </summary>
    public override string ToString()
        => base.ToString() + $"monthly deposit: {_monthlyDeposit}";

}