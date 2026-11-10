namespace GuessNumberGame.Domain.Models;

public sealed class PlayerStatistics
{
    public int GamesPlayed { get; set; }

    public int Wins { get; set; }

    public int TotalAttemptsInWins { get; set; }

    public int? BestAttempts { get; set; }

    public int Losses => GamesPlayed - Wins;

    public double AverageAttemptsInWins => Wins == 0 ? 0 : (double)TotalAttemptsInWins / Wins;

    public void RecordGame(bool won, int attempts)
    {
        GamesPlayed++;

        if (!won)
        {
            return;
        }

        Wins++;
        TotalAttemptsInWins += attempts;
        BestAttempts = BestAttempts is null ? attempts : Math.Min(BestAttempts.Value, attempts);
    }
}
