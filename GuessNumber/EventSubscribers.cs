namespace GuessNumber;

/// <summary>Подписчик, превращающий события игры в текстовые сообщения.</summary>
public sealed class GameMessageSubscriber
{
    private readonly Action<string> _write;

    /// <summary>Создаёт подписчик с функцией вывода.</summary>
    public GameMessageSubscriber(Action<string> write) => _write = write;

    /// <summary>Подписывается на события игры.</summary>
    public void Subscribe(GuessingGame game)
    {
        game.GuessMade += OnGuessMade;
        game.GameFinished += OnGameFinished;
    }

    private void OnGuessMade(object? sender, GuessResult result)
    {
        var message = result.Hint switch
        {
            GuessHint.Higher => "Загаданное число больше.",
            GuessHint.Lower => "Загаданное число меньше.",
            GuessHint.Correct => "Поздравляем, число угадано!",
            _ => "Попытки закончились."
        };
        _write($"{message} Осталось попыток: {result.AttemptsRemaining}");
    }

    private void OnGameFinished(object? sender, GameSummary summary)
    {
        if (!summary.Won)
            _write($"Игра окончена. Загаданное число: {summary.SecretNumber}.");
    }
}

/// <summary>Подписчик, который обновляет статистику после завершения игры.</summary>
public sealed class StatisticsSubscriber
{
    private readonly Action<GameSummary> _onFinished;

    /// <summary>Создаёт подписчик статистики.</summary>
    public StatisticsSubscriber(Action<GameSummary> onFinished) => _onFinished = onFinished;

    /// <summary>Подписывается на завершение игры.</summary>
    public void Subscribe(GuessingGame game) => game.GameFinished += (_, summary) => _onFinished(summary);
}
