namespace GuessNumber;

/// <summary>Преобразует пользовательский выбор в настройки игры.</summary>
public sealed class SettingsParser
{
    /// <summary>Создаёт настройки из названия сложности и лимита попыток.</summary>
    /// <param name="difficultyText">easy, medium или hard.</param>
    /// <param name="attempts">Лимит попыток, либо 0 для значения по умолчанию.</param>
    /// <returns>Проверенные настройки игры.</returns>
    public GameSettings Parse(string difficultyText, int attempts)
    {
        if (!Enum.TryParse<DifficultyLevel>(difficultyText, true, out var difficulty))
            throw new ArgumentException("Difficulty must be easy, medium or hard.", nameof(difficultyText));
        if (attempts < 0)
            throw new ArgumentOutOfRangeException(nameof(attempts));
        return new GameSettings(difficulty, attempts);
    }
}

/// <summary>Создаёт игру из настроек.</summary>
public sealed class GuessingGameFactory
{
    private readonly IGameRules _rules;
    private readonly INumberGenerator _generator;

    /// <summary>Создаёт фабрику с внедрёнными правилами и генератором.</summary>
    public GuessingGameFactory(IGameRules rules, INumberGenerator generator)
    {
        _rules = rules;
        _generator = generator;
    }

    /// <summary>Создаёт новую игру.</summary>
    public GuessingGame Create(GameSettings settings) => new(settings, _rules, _generator);
}
