namespace GuessNumberGame.Domain.RandomNumberGenerator;

public sealed class RandomNumberGenerator : IRandomNumberGenerator
{
    public int Next(int min, int max) => Random.Shared.Next(min, max + 1);
}