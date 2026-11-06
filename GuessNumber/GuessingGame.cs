namespace GuessNumber;

/// <summary>Источник загаданного числа.</summary>
public interface INumberGenerator
{
    /// <summary>Возвращает число из диапазона включительно.</summary>
    int Next(int minimum, int maximum);
}

/// <summary>Генератор на основе <see cref="Random"/>.</summary>
public sealed class RandomNumberGenerator : INumberGenerator
{
    private readonly Random _random = new();

    /// <inheritdoc />
    public int Next(int minimum, int maximum) => _random.Next(minimum, maximum + 1);
}

/// <summary>Аргументы события хода игрока.</summary>
public sealed class GuessMadeEventArgs(int guess, GuessHint hint, int attemptsRemaining) : EventArgs
{
    /// <summary>Введённое число.</summary>
    public int Guess { get; } = guess;

    /// <summary>Подсказка для игрока.</summary>
    public GuessHint Hint { get; } = hint;

    /// <summary>Количество оставшихся попыток.</summary>
    public int AttemptsRemaining { get; } = attemptsRemaining;
}

/// <summary>Аргументы события завершения игры.</summary>
public sealed class GameFinishedEventArgs(bool won, int secretNumber, int attemptsUsed) : EventArgs
{
    /// <summary>Признак победы.</summary>
    public bool Won { get; } = won;

    /// <summary>Загаданное число.</summary>
    public int SecretNumber { get; } = secretNumber;

    /// <summary>Количество использованных попыток.</summary>
    public int AttemptsUsed { get; } = attemptsUsed;
}

/// <summary>Подсказка после попытки игрока.</summary>
public enum GuessHint
{
    /// <summary>Загаданное число больше.</summary>
    Higher,

    /// <summary>Загаданное число меньше.</summary>
    Lower,

    /// <summary>Число угадано.</summary>
    Correct
}

/// <summary>Одна игра «Угадай число». Не зависит от консоли и файлов.</summary>
public sealed class GuessingGame
{
    private readonly int _secretNumber;
    private readonly int _maxAttempts;
    private int _attemptsUsed;

    /// <summary>Признак завершения игры победой или исчерпанием попыток.</summary>
    public bool IsFinished { get; private set; }

    /// <summary>Возникает после каждой попытки игрока.</summary>
    public event EventHandler<GuessMadeEventArgs>? GuessMade;

    /// <summary>Возникает, когда игра завершилась победой или закончились попытки.</summary>
    public event EventHandler<GameFinishedEventArgs>? GameFinished;

    /// <summary>Создаёт игру с диапазоном и лимитом попыток.</summary>
    /// <param name="minimum">Нижняя граница загадываемого числа.</param>
    /// <param name="maximum">Верхняя граница загадываемого числа.</param>
    /// <param name="maxAttempts">Количество попыток.</param>
    /// <param name="generator">Источник загаданного числа.</param>
    public GuessingGame(int minimum, int maximum, int maxAttempts, INumberGenerator generator)
    {
        _secretNumber = generator.Next(minimum, maximum);
        _maxAttempts = maxAttempts;
    }

    /// <summary>Обрабатывает попытку игрока и публикует события.</summary>
    /// <param name="guess">Число, которое назвал игрок.</param>
    /// <returns>Подсказка либо <see langword="null"/>, если игра уже завершена.</returns>
    public GuessHint? Guess(int guess)
    {
        if (IsFinished)
            return null;

        _attemptsUsed++;
        var hint = guess == _secretNumber ? GuessHint.Correct
            : guess < _secretNumber ? GuessHint.Higher
            : GuessHint.Lower;

        GuessMade?.Invoke(this, new GuessMadeEventArgs(guess, hint, _maxAttempts - _attemptsUsed));

        if (hint == GuessHint.Correct || _attemptsUsed >= _maxAttempts)
        {
            IsFinished = true;
            GameFinished?.Invoke(this, new GameFinishedEventArgs(hint == GuessHint.Correct, _secretNumber, _attemptsUsed));
        }

        return hint;
    }
}
