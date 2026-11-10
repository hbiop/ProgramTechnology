using GuessNumberGame.Domain.RandomNumberGenerator;

namespace GuessNumberGame.Domain.Models;

public sealed class GuessingGame
{
    private readonly int _secret;

    public GuessingGame(GameSettings settings, IRandomNumberGenerator random)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(random);

        var errors = settings.Validate();
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" ", errors), nameof(settings));
        }

        MinNumber = settings.MinNumber;
        MaxNumber = settings.MaxNumber;
        MaxAttempts = settings.MaxAttempts;
        _secret = random.Next(MinNumber, MaxNumber);
    }

    public int MinNumber { get; }

    public int MaxNumber { get; }

    public int MaxAttempts { get; }

    public int Attempts { get; private set; }

    public bool IsWon { get; private set; }

    public bool IsFinished => IsWon || Attempts >= MaxAttempts;

    public int? RevealedSecret => IsFinished ? _secret : null;

    public GuessOutcome MakeGuess(int number)
    {
        if (IsFinished)
        {
            throw new InvalidOperationException("Игра уже завершена.");
        }

        if (number < MinNumber || number > MaxNumber)
        {
            return GuessOutcome.OutOfRange;
        }

        Attempts++;

        if (number == _secret)
        {
            IsWon = true;
            return GuessOutcome.Correct;
        }

        return number < _secret ? GuessOutcome.TooLow : GuessOutcome.TooHigh;
    }
}
