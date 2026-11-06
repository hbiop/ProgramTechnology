namespace GuessNumber;

/// <summary>Содержит правила уровней сложности.</summary>
public sealed class GameRules : IGameRules
{
    /// <summary>Возвращает правила уровней сложности.</summary>
    /// <inheritdoc />
    public (NumberRange Range, int Attempts) Resolve(GameSettings settings) => settings.Difficulty switch
    {
        DifficultyLevel.Easy => (new NumberRange(1, 50), settings.MaximumAttempts > 0 ? settings.MaximumAttempts : 10),
        DifficultyLevel.Medium => (new NumberRange(1, 100), settings.MaximumAttempts > 0 ? settings.MaximumAttempts : 7),
        DifficultyLevel.Hard => (new NumberRange(1, 500), settings.MaximumAttempts > 0 ? settings.MaximumAttempts : 5),
        _ => throw new ArgumentOutOfRangeException(nameof(settings))
    };
}
