using System.Text;

namespace Bank;

/// <summary>
/// Журнал аудита операций, выполненных по банковским счетам.
/// </summary>
public static class AuditLog
{
    private static readonly List<string> Records = new();

    /// <summary>
    /// Записывает операцию в журнал аудита.
    /// </summary>
    /// <param name="accountNumber">Номер счёта.</param>
    /// <param name="operationType">Тип операции: Deposit, Withdrawal или Interest.</param>
    /// <param name="amount">Положительная сумма операции.</param>
    /// <param name="date">Дата операции.</param>
    public static void Record(string accountNumber, string operationType, decimal amount, DateTime date)
    {
        Records.Add($"{date:yyyy-MM-dd HH:mm} | {accountNumber} | {operationType} | {amount}");
    }

    /// <summary>
    /// Возвращает копию всех записей журнала аудита.
    /// </summary>
    /// <returns>Список строк журнала.</returns>
    public static List<string> GetRecords() => new(Records);

    /// <summary>
    /// Возвращает журнал аудита в виде одной строки для вывода.
    /// </summary>
    /// <returns>Все записи журнала, разделённые переводами строк.</returns>
    public static string Dump()
    {
        var builder = new StringBuilder();
        foreach (var record in Records)
            builder.AppendLine(record);
        return builder.ToString();
    }
}
