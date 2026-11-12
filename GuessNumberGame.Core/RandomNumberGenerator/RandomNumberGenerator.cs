namespace GuessNumberGame.Domain.RandomNumberGenerator;

public sealed class RandomNumberGenerator : IRandomNumberGenerator
{
    public int Next(int min, int max) => new Random().Next(min, max + 1);
}