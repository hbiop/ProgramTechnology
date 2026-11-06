namespace Bank;

/// <summary>
/// Представляет неизменяемую операцию по банковскому счёту.
/// </summary>
public record Transaction
{
    /// <summary>
    /// Создаёт операцию с указанной суммой, датой и комментарием.
    /// </summary>
    /// <param name="amount">Изменение баланса: положительное или отрицательное.</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="note">Комментарий к операции.</param>
    public Transaction(decimal amount, DateTime date, string note)
    {
        Amount = amount;
        Date = date;
        Note = note;
    }

    /// <summary>
    /// Изменение баланса, внесённое операцией.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Дата и время операции.
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Комментарий к операции.
    /// </summary>
    public string Note { get; }
}
