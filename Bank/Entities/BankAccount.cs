using System.Text;

namespace Bank;

/// <summary>
/// Представляет банковский счёт с историей операций и ограничением минимального баланса.
/// </summary>
public class BankAccount
{
    private readonly decimal _minimumBalance;
    private static int s_accountNumberSeed = 1000000000;
    private readonly List<Transaction> _allTransactions = new();

    /// <summary>
    /// Статус клиента, определяющий ежемесячные условия обслуживания.
    /// </summary>
    public ClientStatus Status { get; set; }

    /// <summary>
    /// Уникальный текстовый номер счёта.
    /// </summary>
    public string Number { get; }

    /// <summary>
    /// Имя владельца счёта.
    /// </summary>
    public string Owner { get; private set; }

    /// <summary>
    /// Текущий баланс, рассчитанный по истории операций.
    /// </summary>
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var transaction in _allTransactions)
                balance += transaction.Amount;
            return balance;
        }
    }

    /// <summary>
    /// Инициализирует счёт с минимальным балансом, равным нулю.
    /// </summary>
    /// <param name="name">Имя владельца.</param>
    /// <param name="initialBalance">Начальный положительный баланс.</param>
    public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0)
    {
    }

    /// <summary>
    /// Инициализирует счёт с заданным минимальным балансом.
    /// </summary>
    /// <param name="name">Имя владельца.</param>
    /// <param name="initialBalance">Начальный положительный баланс.</param>
    /// <param name="minimumBalance">Минимально допустимый баланс.</param>
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
        Owner = name;
        Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;
        _minimumBalance = minimumBalance;
        Status = ClientStatus.Regular;

        if (initialBalance > 0)
            MakeDeposit(initialBalance, DateTime.UtcNow, "Initial balance");
    }

    /// <summary>
    /// Зачисляет положительную сумму на счёт.
    /// </summary>
    /// <param name="amount">Сумма пополнения.</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="note">Комментарий к операции.</param>
    /// <exception cref="ArgumentOutOfRangeException">Если сумма меньше или равна нулю.</exception>
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        AddTransaction(new Transaction(amount, date, note), "Deposit");
    }

    /// <summary>
    /// Списывает положительную сумму со счёта.
    /// </summary>
    /// <param name="amount">Сумма списания.</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="note">Комментарий к операции.</param>
    /// <exception cref="ArgumentOutOfRangeException">Если сумма меньше или равна нулю.</exception>
    /// <exception cref="InvalidOperationException">Если списание нарушает минимальный баланс.</exception>
    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        AddTransaction(new Transaction(-amount, date, note), "Withdrawal");

        if (overdraftTransaction is not null)
            AddTransaction(overdraftTransaction, "Withdrawal");
    }

    /// <summary>
    /// Проверяет, нарушает ли списание минимальный баланс.
    /// </summary>
    /// <param name="isOverdrawn">Признак выхода за минимальный баланс.</param>
    /// <returns>Дополнительная операция для особого типа счёта или null.</returns>
    /// <exception cref="InvalidOperationException">Если обычный счёт выходит за лимит.</exception>
    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
            throw new InvalidOperationException("Not sufficient funds for this withdrawal");
        return null;
    }

    /// <summary>
    /// Добавляет операцию во внутреннюю историю и журнал аудита.
    /// </summary>
    /// <param name="transaction">Добавляемая операция.</param>
    /// <param name="operationType">Тип операции аудита.</param>
    protected void AddTransaction(Transaction transaction, string operationType)
    {
        _allTransactions.Add(transaction);
        AuditLog.Record(Number, operationType, Math.Abs(transaction.Amount), transaction.Date);
    }

    /// <summary>
    /// Зачисляет проценты и записывает их как операцию Interest.
    /// </summary>
    /// <param name="amount">Сумма процентов.</param>
    /// <param name="date">Дата начисления.</param>
    /// <param name="note">Комментарий к начислению.</param>
    protected void ApplyInterest(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
            return;
        AddTransaction(new Transaction(amount, date, note), "Interest");
    }

    /// <summary>
    /// Формирует текстовую историю операций и балансов.
    /// </summary>
    /// <returns>Отформатированная история счёта.</returns>
    public string GetAccountHistory()
    {
        var report = new StringBuilder();
        decimal balance = 0;
        report.AppendLine("Date\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Note}");
        }
        return report.ToString();
    }

    /// <summary>
    /// Выполняет ежемесячную комиссию, проценты и кэшбэк по статусу клиента.
    /// </summary>
    public virtual void PerformMonthEndTransactions()
    {
        var now = DateTime.UtcNow;
        switch (Status)
        {
            case ClientStatus.Regular:
                MakeWithdrawal(50m, now, "Monthly fee (Regular)");
                break;
            case ClientStatus.Premium:
                ApplyInterest((Balance - 10000m) * 0.01m, now, "Premium interest (1%)");
                break;
            case ClientStatus.VIP:
                ApplyInterest((Balance - 10000m) * 0.03m, now, "VIP interest (3%)");
                decimal totalWithdrawals = 0m;
                foreach (var transaction in _allTransactions)
                {
                    if (transaction.Amount < 0)
                        totalWithdrawals -= transaction.Amount;
                }
                ApplyInterest(totalWithdrawals * 0.05m, now, "VIP cashback (5%)");
                break;
        }
    }

    /// <summary>
    /// Возвращает тип, владельца, номер и баланс счёта.
    /// </summary>
    /// <returns>Текстовое представление счёта.</returns>
    public override string ToString() =>
        $"Type: {GetType().Name}\tOwner: {Owner}\tNumber: {Number}\tBalance: {Balance}";
}
