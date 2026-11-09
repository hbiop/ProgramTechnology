namespace GuessNumberGame.Domain.RandomNumberGenerator;

public class RandomNumberGenerator
{
    public int Next(int min, int max) => Random.Shared.Next(min, max + 1);
}