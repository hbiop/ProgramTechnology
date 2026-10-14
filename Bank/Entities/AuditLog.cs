using System.Text;

namespace Bank;

/// <summary>
/// Журнал аудита операций. Хранит все операции по всем счетам.
/// </summary>
public static class AuditLog
{
    /// <summary>
    /// Внутренний список записей журнала аудита.
    /// </summary>
    private static List<string> _records = new List<string>();

    /// <summary>
    /// Записывает операцию в журнал аудита.
    /// </summary>
    /// <param name="accountNumber">Номер счёта.</param>
    /// <param name="operationType">Тип операции.</param>
    /// <param name="amount">Сумма операции.</param>
    /// <param name="date">Дата операции.</param>
    public static void Record(string accountNumber, string operationType, decimal amount, DateTime date)
    {
        string line = $"{date:yyyy-MM-dd HH:mm} | {accountNumber} | {operationType} | {amount}";
        _records.Add(line);
    }

    /// <summary>
    /// Возвращает все записи журнала аудита.
    /// </summary>
    public static List<string> GetRecords()
    {
        return _records;
    }

    /// <summary>
    /// Возвращает журнал аудита в виде одной строки для вывода.
    /// </summary>
    public static string Dump()
    {
        var sb = new StringBuilder();
        foreach (var r in _records)
            sb.AppendLine(r);
        return sb.ToString();
    }
}