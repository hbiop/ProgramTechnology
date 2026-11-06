namespace GuessNumber;

internal static class Program
{
    private const string StatisticsFile = "statistics.json";

    private static void Main()
    {
        IStatisticsStore store = new JsonStatisticsStore(StatisticsFile);
        INumberGenerator generator = new RandomNumberGenerator();

        Console.WriteLine("=== Игра «Угадай число» ===");

        do
        {
            var (minimum, maximum, attempts) = ReadDifficulty();
            var game = new GuessingGame(minimum, maximum, attempts, generator);

            game.GuessMade += (_, e) => Console.WriteLine(e.Hint switch
            {
                GuessHint.Higher => $"Загаданное число больше. Осталось попыток: {e.AttemptsRemaining}",
                GuessHint.Lower => $"Загаданное число меньше. Осталось попыток: {e.AttemptsRemaining}",
                _ => "Поздравляем, вы угадали!"
            });
            game.GameFinished += (_, e) =>
            {
                if (!e.Won)
                    Console.WriteLine($"Попытки закончились. Загаданное число: {e.SecretNumber}.");
                store.Save(store.Load().WithGame(e.Won));
            };

            PlayRound(game);
        } while (AskToPlayAgain());

        var statistics = store.Load();
        Console.WriteLine($"Статистика: игр — {statistics.GamesPlayed}, побед — {statistics.GamesWon}.");
    }

    private static (int Minimum, int Maximum, int Attempts) ReadDifficulty()
    {
        while (true)
        {
            Console.Write("Сложность (easy/medium/hard): ");
            var level = Console.ReadLine()?.Trim().ToLowerInvariant();
            var difficulty = level switch
            {
                "easy" => (Minimum: 1, Maximum: 50, Attempts: 10),
                "medium" => (Minimum: 1, Maximum: 100, Attempts: 7),
                "hard" => (Minimum: 1, Maximum: 500, Attempts: 5),
                _ => default
            };

            if (difficulty != default)
                return difficulty;

            Console.WriteLine("Введите easy, medium или hard.");
        }
    }

    private static void PlayRound(GuessingGame game)
    {
        while (!game.IsFinished)
        {
            Console.Write("Ваш вариант: ");
            if (int.TryParse(Console.ReadLine(), out var guess))
                game.Guess(guess);
            else
                Console.WriteLine("Введите целое число.");
        }
    }

    private static bool AskToPlayAgain()
    {
        Console.Write("Сыграть ещё раз? (y/n): ");
        return string.Equals(Console.ReadLine()?.Trim(), "y", StringComparison.OrdinalIgnoreCase);
    }
}
