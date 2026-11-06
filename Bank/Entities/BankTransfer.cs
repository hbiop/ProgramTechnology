namespace Bank;

/// <summary>
/// Выполняет переводы между банковскими счетами.
/// </summary>
public static class BankTransfer
{
    /// <summary>
    /// Переводит деньги с одного счёта на другой.
    /// </summary>
    /// <param name="fromAccount">Счёт, с которого списываются деньги.</param>
    /// <param name="toAccount">Счёт, на который зачисляются деньги.</param>
    /// <param name="amount">Сумма перевода. Должна быть положительной.</param>
    /// <param name="date">Дата перевода.</param>
    /// <param name="note">Комментарий к переводу.</param>
    /// <exception cref="ArgumentNullException">Если один из счетов равен null.</exception>
    /// <exception cref="ArgumentException">Если счета совпадают.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Если сумма меньше или равна нулю.</exception>
    public static void Transfer(
        BankAccount fromAccount,
        BankAccount toAccount,
        decimal amount,
        DateTime date,
        string note)
    {
        ArgumentNullException.ThrowIfNull(fromAccount);
        ArgumentNullException.ThrowIfNull(toAccount);
        if (ReferenceEquals(fromAccount, toAccount))
            throw new ArgumentException("Source and destination accounts must differ.", nameof(toAccount));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        fromAccount.MakeWithdrawal(amount, date, $"Transfer to {toAccount.Number}: {note}");
        toAccount.MakeDeposit(amount, date, $"Transfer from {fromAccount.Number}: {note}");
    }
}
