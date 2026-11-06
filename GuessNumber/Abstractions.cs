namespace GuessNumber;

/// <summary>Выбирает случайное число.</summary>
public interface INumberGenerator
{
    /// <summary>Возвращает число из диапазона.</summary>
    int Next(NumberRange range);
}

/// <summary>Преобразует сложность в правила игры.</summary>
public interface IGameRules
{
    /// <summary>Возвращает диапазон и лимит попыток для настроек.</summary>
    (NumberRange Range, int Attempts) Resolve(GameSettings settings);
}

/// <summary>Хранилище статистики игроков.</summary>
public interface IStatisticsStore
{
    /// <summary>Загружает статистику.</summary>
    Statistics Load();
    /// <summary>Сохраняет статистику.</summary>
    void Save(Statistics statistics);
}

/// <summary>Источник пользовательского ввода и вывода.</summary>
public interface IUserInterface
{
    /// <summary>Читает строку.</summary>
    string? ReadLine();
    /// <summary>Показывает сообщение.</summary>
    void WriteLine(string message);
}

/// <summary>Генерирует число с помощью системного генератора случайных чисел.</summary>
public sealed class RandomNumberGenerator : INumberGenerator
{
    private readonly Random _random;
    /// <summary>Создаёт генератор.</summary>
    public RandomNumberGenerator(Random? random = null) => _random = random ?? new Random();
    /// <inheritdoc />
    public int Next(NumberRange range) => _random.Next(range.Minimum, range.Maximum + 1);
}
