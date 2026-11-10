namespace GuessNumberGame.Domain.Models;

public sealed class GameSettings
{
    public const int DefaultMinNumber = 1;
    public const int DefaultMaxNumber = 100;
    public const int EasyAttempts = 10;
    public const int MediumAttempts = 8;
    public const int HardAttempts = 5;

    public int MinNumber { get; set; } = DefaultMinNumber;

    public int MaxNumber { get; set; } = DefaultMaxNumber;

    public Difficulty Difficulty { get; set; } = Difficulty.Easy;

    public int MaxAttempts => Difficulty switch
    {
        Difficulty.Easy => EasyAttempts,
        Difficulty.Medium => MediumAttempts,
        Difficulty.Hard => HardAttempts,
        _ => EasyAttempts
    };

    public GameSettings Clone() => new()
    {
        MinNumber = MinNumber,
        MaxNumber = MaxNumber,
        Difficulty = Difficulty
    };

    public IReadOnlyList<string> Validate()
    {
        var errors = new List<string>();

        if (MaxNumber - MinNumber < 1)
        {
            errors.Add("Максимальное число должно быть больше минимального.");
        }

        if (!Enum.IsDefined(Difficulty))
        {
            errors.Add("Выбран неизвестный уровень сложности.");
        }

        return errors;
    }
}
