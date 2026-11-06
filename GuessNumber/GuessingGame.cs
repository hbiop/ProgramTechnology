namespace GuessNumber;

/// <summary>Управляет одной игрой и публикует события её хода.</summary>
public sealed class GuessingGame
{
    private readonly int _secretNumber;
    private readonly int _maximumAttempts;
    private int _attempts;
    private bool _finished;

    /// <summary>Возникает после каждой попытки.</summary>
    public event EventHandler<GuessResult>? GuessMade;
    /// <summary>Возникает после завершения игры.</summary>
    public event EventHandler<GameSummary>? GameFinished;

    /// <summary>Создаёт игру по настройкам и зависимостям.</summary>
    public GuessingGame(GameSettings settings, IGameRules rules, INumberGenerator generator)
    {
        var resolved = rules.Resolve(settings);
        _secretNumber = generator.Next(resolved.Range);
        _maximumAttempts = resolved.Attempts;
    }

    /// <summary>Регистрирует попытку игрока.</summary>
    public GuessResult Guess(int value)
    {
        if (_finished)
            return new GuessResult(value, GuessHint.AttemptsExceeded, _attempts, 0);

        _attempts++;
        var hint = value == _secretNumber ? GuessHint.Correct : value < _secretNumber ? GuessHint.Higher : GuessHint.Lower;
        var result = new GuessResult(value, hint, _attempts, Math.Max(0, _maximumAttempts - _attempts));
        GuessMade?.Invoke(this, result);

        if (hint == GuessHint.Correct || _attempts >= _maximumAttempts)
        {
            _finished = true;
            GameFinished?.Invoke(this, new GameSummary(hint == GuessHint.Correct, hint == GuessHint.Correct ? null : _secretNumber, _attempts));
        }
        return result;
    }
}
