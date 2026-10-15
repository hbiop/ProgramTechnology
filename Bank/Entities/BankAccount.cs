using System.Text;

namespace Bank;

/// <summary>
/// Представляет базовый класс банковского счёта с поддержкой операций пополнения, снятия и ведения истории.
/// </summary>
public class BankAccount
{
    /// <summary>
    /// Минимально допустимый баланс для данного счёта.
    /// </summary>
    private readonly decimal _minimumBalance;

    /// <summary>
    /// Статус клиента банка. Определяет размер комиссии за обслуживание,
    /// процент на остаток свыше 10 000 и кэшбэк на списания.
    /// </summary>
    public ClientStatus Status { get; set; }

    /// <summary>
    /// Начальное значение для генерации уникальных номеров счетов.
    /// </summary>
    private static int s_accountNumberSeed = 1000000000;
    /// <summary>
    /// Уникальный текстовый номер банковского счёта.
    /// </summary>
    public string Number { get; }
    /// <summary>
    /// Имя и фамилия владельца банковского счёта.
    /// </summary>
    public string Owner { get; private set; }
    /// <summary>
    /// Текущий баланс счёта, автоматически вычисляемый как сумма всех совершённых транзакций.
    /// </summary>
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var transaction in _allTransactions)
            {
                balance += transaction.Amount;
            }
            return balance;
        }
    }

    /// <summary>
    /// Внутренний список, хранящий всю историю транзакций по данному счёту.
    /// </summary>
    private List<Transaction> _allTransactions = new List<Transaction>();

    /// <summary>
    /// Инициализирует обычный банковский счёт с указанием владельца и начального баланса. Минимальный лимит баланса устанавливается в 0.
    /// </summary>
    public BankAccount(string name, decimal initialBalance)
        : this(name, initialBalance, 0)
    {
    }

    /// <summary>
    /// Инициализирует банковский счёт с указанием владельца, начального баланса и пользовательского лимита минимального баланса.
    /// </summary>
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
        Owner = name; //this.Owner = name;

        Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;

        _minimumBalance = minimumBalance;

        Status = ClientStatus.Regular;

        if (initialBalance > 0)
            MakeDeposit(initialBalance, DateTime.UtcNow, "Initial balance");

    }

    /// <summary>
    /// Зачисляет на счёт указанную сумму на определённую дату с текстовым комментарием. Сумма должна быть строго положительной.
    /// </summary>
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
        }
        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);

        AuditLog.Record(Number, "Deposit", amount, date);
    }

    /// <summary>
    /// Списывает со счёта указанную сумму на определённую дату с комментарием, а также автоматически применяет овердрафт при нехватке лимита.
    /// </summary>
    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        Transaction? overdraftTransaction
            = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        Transaction? withdrawal = new(-amount, date, note);

        _allTransactions.Add(withdrawal);

        AuditLog.Record(Number, "Withdrawal", amount, date);

        if (overdraftTransaction is not null)
            _allTransactions.Add(overdraftTransaction);

    }

    /// <summary>
    /// Проверяет, уходит ли счёт в минус ниже лимита. Возвращает транзакцию штрафа для кредитного счёта или генерирует ошибку для обычного.
    /// </summary>
    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("nit sufficient rubls for this withdrawal");
        }
        else
        {
            // default - содержит значение по умолчанию, так как тип возвращающего значения - ссылочный, то
            // default = null
            return default; // == return null;
        }
    }

    /// <summary>
    /// Формирует и возвращает форматированную текстовую строку со всей историей изменений баланса и транзакций по счёту.
    /// </summary>
    public string GetAccountHistory()
    {
        var report = new StringBuilder();

        decimal balance = 0;
        report.AppendLine("Data\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Note}");
        }

        return report.ToString();
    }

    // Ключевое слово virtual позволяет в дочернем классе
    // предоставить другую реализацию 
    // метода PerformMounthEndTransactions
    /// <summary>
    /// Выполняет регламентные расчёты и транзакции в конце месяца и начисляет бонусы по статусу клиента. Переопределяется в классах-наследниках.
    /// </summary>
    public virtual void PerformMonthEndTransactions()
    {
        switch (Status)
        {
            case ClientStatus.Regular:
                MakeWithdrawal(50m, DateTime.UtcNow, "Monthly fee (Regular)");
                break;

            case ClientStatus.Premium:
                if (Balance > 10000m)
                {
                    MakeDeposit((Balance - 10000m) * 0.01m, DateTime.UtcNow, "Premium interest (1%)");
                }
                break;

            case ClientStatus.VIP:
                if (Balance > 10000m)
                {
                    MakeDeposit((Balance - 10000m) * 0.03m, DateTime.UtcNow, "VIP interest (3%)");
                }

                decimal totalWithdrawals = 0m;
                foreach (var t in _allTransactions)
                {
                    if (t.Amount < 0)
                        totalWithdrawals += -t.Amount;
                }
                if (totalWithdrawals > 0)
                {
                    MakeDeposit(totalWithdrawals * 0.05m, DateTime.UtcNow, "VIP cashback (5%)");
                }
                break;
        }
        // ===== КОНЕЦ ДОБАВЛЕНО =====
    }

    // переопределяем метод, который унаследовал от object
    // этот метод должен возвращать строку с состоянием объекта
    /// <summary>
    /// Возвращает текстовое представление счёта, включающее его тип, имя владельца, номер и текущий баланс.
    /// </summary>
    public override string ToString()
    {
        return $"Type: {GetType().Name}\tOwner: {Owner}\tNumber: {Number}\tBalance: {Balance}";
    }

    /// <summary>
    /// Переводит деньги с текущего счёта на другой счёт. Списывает сумму с этого счёта, затем зачисляет на целевой.
    /// </summary>
    /// <param name="toAccount">Счёт, на который переводятся деньги.</param>
    /// <param name="amount">Сумма перевода. Должна быть положительной.</param>
    /// <param name="date">Дата перевода.</param>
    /// <param name="note">Комментарий к переводу.</param>
    /// <exception cref="ArgumentNullException">Если <paramref name="toAccount"/> равен null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Если <paramref name="amount"/> меньше или равен нулю.</exception>
    public void Transfer(BankAccount toAccount, decimal amount, DateTime date, string note)
    {
        if (toAccount is null)
            throw new ArgumentNullException(nameof(toAccount));
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Transfer amount must be positive");

        MakeWithdrawal(amount, date, note);

        toAccount.MakeDeposit(amount, date, note);
    }
}