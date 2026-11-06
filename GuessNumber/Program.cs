namespace GuessNumber;

internal static class Program
{
    private static void Main()
    {
        var rules = new GameRules();
        var factory = new GuessingGameFactory(rules, new RandomNumberGenerator());
        var settingsParser = new SettingsParser();
        var statisticsService = new StatisticsService(new JsonStatisticsStore("statistics.json"));

        Console.WriteLine("=== Игра «Угадай число» ===");
        Console.WriteLine("Статистика сохраняется в statistics.json.");

        while (true)
        {
            var settings = ReadSettings(settingsParser);
            var game = factory.Create(settings);
            var messages = new GameMessageSubscriber(Console.WriteLine);
            messages.Subscribe(game);

            var statistics = new StatisticsSubscriber(summary =>
            {
                var updated = statisticsService.Register(summary);
                Console.WriteLine($"Игр: {updated.GamesPlayed}; побед: {updated.GamesWon}; лучший результат: {updated.BestScore}.");
            });
            statistics.Subscribe(game);

            PlayRound(game);
            Console.Write("Сыграть ещё раз? (y/n): ");
            if (!string.Equals(Console.ReadLine()?.Trim(), "y", StringComparison.OrdinalIgnoreCase))
                break;
        }

        var finalStatistics = statisticsService.GetCurrent();
        Console.WriteLine($"Итог: {finalStatistics.GamesPlayed} игр, {finalStatistics.GamesWon} побед.");
    }

    private static GameSettings ReadSettings(SettingsParser parser)
    {
        while (true)
        {
            Console.Write("Сложность (easy/medium/hard): ");
            var difficulty = Console.ReadLine() ?? string.Empty;
            Console.Write("Количество попыток (0 — по умолчанию): ");
            var attemptsText = Console.ReadLine();

            if (int.TryParse(attemptsText, out var attempts))
            {
                try
                {
                    return parser.Parse(difficulty, attempts);
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine($"Ошибка настроек: {exception.Message}");
                }
            }
            else
            {
                Console.WriteLine("Количество попыток должно быть целым числом.");
            }
        }
    }

    private static void PlayRound(GuessingGame game)
    {
        while (true)
        {
            Console.Write("Ваш вариант: ");
            if (!int.TryParse(Console.ReadLine(), out var guess))
            {
                Console.WriteLine("Введите целое число.");
                continue;
            }

            var result = game.Guess(guess);
            if (result.Hint is GuessHint.Correct or GuessHint.AttemptsExceeded)
                return;
        }
    }
}
