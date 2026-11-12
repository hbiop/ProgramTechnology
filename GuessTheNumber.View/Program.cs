using GuessNumberGame.Domain.Models;
using GuessNumberGame.Domain.RandomNumberGenerator;
using GuessNumberGame.Domain.Services;

namespace GuessTheNumber.View;

internal static class Program
{
    private const int DefaultMaxNumber = 100;
    private const int AbsoluteMaxNumber = 1_000_000;

    private static void Main()
    {
        var statisticsStore = new JsonStatisticsStore("statistics.json");
        var statistics = statisticsStore.Load();

        Console.WriteLine("=== Угадай число ===");
        Console.WriteLine("Введите q в любой момент, чтобы выйти.\n");
        PrintStatistics(statistics);

        while (true)
        {
            var settings = ReadSettings();
            if (settings is null)
            {
                break;
            }

            var game = new GuessingGame(settings, new RandomNumberGenerator());
            game.GuessMade += (_, args) => PrintHint(args.Outcome);

            Console.WriteLine($"\nУгадайте число от {game.MinNumber} до {game.MaxNumber}. Попыток: {game.MaxAttempts}.");
            while (!game.IsFinished)
            {
                Console.Write("Ваш вариант: ");
                var input = Console.ReadLine();
                if (input is null || input.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                if (!int.TryParse(input, out var guess))
                {
                    Console.WriteLine("Введите целое число или q.");
                    continue;
                }

                try
                {
                    game.MakeGuess(guess);
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            if (game.IsWon)
            {
                Console.WriteLine($"Победа за {game.Attempts} попыток!");
            }
            else
            {
                Console.WriteLine($"Попытки закончились. Загаданное число: {game.RevealedSecret}.");
            }

            statistics.RecordGame(game.IsWon, game.Attempts);
            statisticsStore.Save(statistics);
            Console.WriteLine("Статистика сохранена.");
            Console.Write("Сыграть ещё раз? (y/д): ");
            var again = Console.ReadLine();
            if (!IsYes(again))
            {
                break;
            }

            Console.WriteLine();
        }

        PrintStatistics(statistics);
    }

    private static GameSettings? ReadSettings()
    {
        Console.Write("Сложность (1 — лёгкая, 2 — средняя, 3 — сложная, q — выход): ");
        var difficultyInput = Console.ReadLine();
        if (difficultyInput is null || difficultyInput.Equals("q", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!int.TryParse(difficultyInput, out var difficultyNumber) || difficultyNumber is < 1 or > 3)
        {
            Console.WriteLine("Выберите сложность числом от 1 до 3.");
            return ReadSettings();
        }

        Console.Write($"Минимум (по умолчанию {GameSettings.DefaultMinNumber}): ");
        var minInput = Console.ReadLine();
        if (minInput is null || minInput.Equals("q", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        Console.Write($"Максимум (по умолчанию {DefaultMaxNumber}): ");
        var maxInput = Console.ReadLine();
        if (maxInput is null || maxInput.Equals("q", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var min = string.IsNullOrWhiteSpace(minInput) ? GameSettings.DefaultMinNumber : ParseNumber(minInput);
        var max = string.IsNullOrWhiteSpace(maxInput) ? DefaultMaxNumber : ParseNumber(maxInput);
        if (min is null || max is null || min < 1 || max > AbsoluteMaxNumber || min >= max)
        {
            Console.WriteLine($"Диапазон должен быть от 1 до {AbsoluteMaxNumber}, минимум меньше максимума.");
            return ReadSettings();
        }

        return new GameSettings
        {
            MinNumber = min.Value,
            MaxNumber = max.Value,
            Difficulty = (Difficulty)(difficultyNumber - 1)
        };
    }

    private static int? ParseNumber(string value) => int.TryParse(value, out var number) ? number : null;

    private static void PrintHint(GuessOutcome outcome)
    {
        Console.WriteLine(outcome switch
        {
            GuessOutcome.TooLow => "Больше.",
            GuessOutcome.TooHigh => "Меньше.",
            GuessOutcome.Correct => "Верно!",
            GuessOutcome.OutOfRange => "Число вне заданного диапазона.",
            _ => "Неизвестный результат."
        });
    }

    private static void PrintStatistics(PlayerStatistics statistics)
    {
        Console.WriteLine($"Игр: {statistics.GamesPlayed}, побед: {statistics.Wins}, поражений: {statistics.Losses}, " +
                          $"среднее число попыток: {statistics.AverageAttemptsInWins:F1}, лучший результат: {statistics.BestAttempts?.ToString() ?? "—"}.");
    }

    private static bool IsYes(string? value) => value is "y" or "Y" or "д" or "Д";
}
