namespace GuessNumberGame.Domain.RandomNumberGenerator;

public interface IRandomNumberGenerator
{
    /// <summary>Returns a random integer in the inclusive range [min, max].</summary>
    int Next(int min, int max);
}
