using GuessNumberGame.Domain.Models;

namespace GuessNumberGame.Domain.Events;

public sealed class GuessMadeEventArgs : EventArgs
{
    public GuessMadeEventArgs(int guess, GuessOutcome outcome, int attempts)
    {
        Guess = guess;
        Outcome = outcome;
        Attempts = attempts;
    }

    public int Guess { get; }

    public GuessOutcome Outcome { get; }

    public int Attempts { get; }
}
