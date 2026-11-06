namespace GuessNumber;

/// <summary>Уровень сложности игры.</summary>
public enum DifficultyLevel
{
    /// <summary>Диапазон 1–50.</summary>
    Easy,
    /// <summary>Диапазон 1–100.</summary>
    Medium,
    /// <summary>Диапазон 1–500.</summary>
    Hard
}

/// <summary>Настройки новой игры.</summary>
public sealed record GameSettings(DifficultyLevel Difficulty, int MaximumAttempts);

/// <summary>Диапазон загадываемого числа.</summary>
public sealed record NumberRange(int Minimum, int Maximum);

/// <summary>Подсказка для игрока.</summary>
public enum GuessHint
{
    /// <summary>Нужно назвать число больше.</summary>
    Higher,
    /// <summary>Нужно назвать число меньше.</summary>
    Lower,
    /// <summary>Число угадано.</summary>
    Correct,
    /// <summary>Попытки закончились.</summary>
    AttemptsExceeded
}

/// <summary>Событие хода игрока.</summary>
public sealed class GuessResult : EventArgs
{
    /// <summary>Создаёт результат попытки.</summary>
    public GuessResult(int guess, GuessHint hint, int attemptsUsed, int attemptsRemaining)
    {
        Guess = guess;
        Hint = hint;
        AttemptsUsed = attemptsUsed;
        AttemptsRemaining = attemptsRemaining;
    }

    /// <summary>Введённое число.</summary>
    public int Guess { get; }
    /// <summary>Полученная подсказка.</summary>
    public GuessHint Hint { get; }
    /// <summary>Число использованных попыток.</summary>
    public int AttemptsUsed { get; }
    /// <summary>Число оставшихся попыток.</summary>
    public int AttemptsRemaining { get; }
}

/// <summary>Итог завершённой игры.</summary>
public sealed class GameSummary : EventArgs
{
    /// <summary>Создаёт итог игры.</summary>
    public GameSummary(bool won, int? secretNumber, int attemptsUsed)
    {
        Won = won;
        SecretNumber = secretNumber;
        AttemptsUsed = attemptsUsed;
    }

    /// <summary>Признак победы.</summary>
    public bool Won { get; }
    /// <summary>Загаданное число при поражении.</summary>
    public int? SecretNumber { get; }
    /// <summary>Число использованных попыток.</summary>
    public int AttemptsUsed { get; }
}

/// <summary>Статистика завершённых игр.</summary>
public sealed record Statistics(int GamesPlayed, int GamesWon, int BestScore)
{
    /// <summary>Пустая статистика.</summary>
    public static Statistics Empty => new(0, 0, 0);
}
