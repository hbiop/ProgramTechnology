using System.Text.Json;
using GuessNumberGame.Domain.Events;
using GuessNumberGame.Domain.Models;
using GuessNumberGame.Domain.RandomNumberGenerator;
using GuessNumberGame.Domain.Services;

namespace GuessTheNumber.View;

public sealed class GameApplication
{
    private const int AbsoluteMaxNumber = 1_000_000;
    private readonly IUserInterface _ui;
    private readonly IRandomNumberGenerator _random;
    private GameSettings _settings;
    private PlayerStatistics _statistics;
    private string _statisticsFilePath;
    private string _settingsFilePath;

    public GameApplication(
        IUserInterface ui,
        IRandomNumberGenerator random,
        GameSettings settings,
        PlayerStatistics statistics,
        string settingsFilePath = "settings.json",
        string statisticsFilePath = "statistics.json")
    {
        _ui = ui;
        _random = random;
        _settings = settings;
        _statistics = statistics;
        _settingsFilePath = settingsFilePath;
        _statisticsFilePath = statisticsFilePath;
    }

    public void Run()
    {
        while (true)
        {
            ShowMainMenu();
            var choice = _ui.ReadLine("Выберите пункт: ");

            switch (choice)
            {
                case "1": StartGame(); break;
                case "2": SettingsMenu(); break;
                case "3": StatisticsMenu(); break;
                case "4": return;
                default: _ui.WriteLine("Неизвестный пункт меню."); break;
            }
        }
    }

    private void ShowMainMenu()
    {
        _ui.WriteLine();
        _ui.WriteLine("=== УГАДАЙ ЧИСЛО ===");
        _ui.WriteLine("1. Старт");
        _ui.WriteLine("2. Настройки");
        _ui.WriteLine("3. Статистика");
        _ui.WriteLine("4. Выход");
        _ui.WriteLine($"Текущие настройки: {_settings.MinNumber}..{_settings.MaxNumber}, {_settings.Difficulty}, {_settings.MaxAttempts} попыток.");
    }

    private void StartGame()
    {
        var game = new GuessingGame(_settings, _random);
        game.GuessMade += OnGuessMade;

        _ui.WriteLine($"\nЗагадано число от {game.MinNumber} до {game.MaxNumber}. Попыток: {game.MaxAttempts}.");
        while (!game.IsFinished)
        {
            var input = _ui.ReadLine("Ваш вариант (q — отмена): ");
            if (input is null || input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                game.GuessMade -= OnGuessMade;
                return;
            }

            if (!int.TryParse(input, out var guess))
            {
                _ui.WriteLine("Введите целое число.");
                continue;
            }

            game.MakeGuess(guess);
        }

        game.GuessMade -= OnGuessMade;
        _statistics.RecordGame(game.IsWon, game.Attempts);
        SaveStatistics(_statisticsFilePath, false);

        if (game.IsWon)
        {
            _ui.WriteLine($"Победа за {game.Attempts} попыток!");
        }
        else
        {
            _ui.WriteLine($"Попытки закончились. Загаданное число: {game.RevealedSecret}.");
        }

        _ui.ReadLine("Нажмите Enter, чтобы продолжить: ");
    }

    private void OnGuessMade(object? sender, GuessMadeEventArgs args)
    {
        _ui.WriteLine(args.Outcome switch
        {
            GuessOutcome.TooLow => "Больше.",
            GuessOutcome.TooHigh => "Меньше.",
            GuessOutcome.Correct => "Верно!",
            GuessOutcome.OutOfRange => "Число вне заданного диапазона.",
            _ => "Неизвестный результат."
        });
    }

    private void SettingsMenu()
    {
        while (true)
        {
            _ui.WriteLine("\n=== НАСТРОЙКИ ===");
            _ui.WriteLine($"1. Изменить настройки ({_settings.MinNumber}..{_settings.MaxNumber}, {_settings.Difficulty})");
            _ui.WriteLine("2. Загрузить настройки из файла");
            _ui.WriteLine("3. Экспортировать настройки в файл");
            _ui.WriteLine("4. Назад");

            switch (_ui.ReadLine("Выберите пункт: "))
            {
                case "1": ChangeSettings(); break;
                case "2": LoadSettings(); break;
                case "3": ExportSettings(); break;
                case "4": return;
                default: _ui.WriteLine("Неизвестный пункт меню."); break;
            }
        }
    }

    private void ChangeSettings()
    {
        var min = ReadInt($"Минимальное число (текущее {_settings.MinNumber}): ");
        var max = ReadInt($"Максимальное число (текущее {_settings.MaxNumber}): ");
        var difficulty = ReadDifficulty();
        var candidate = new GameSettings { MinNumber = min, MaxNumber = max, Difficulty = difficulty };
        var errors = candidate.Validate();

        if (errors.Count > 0 || max > AbsoluteMaxNumber)
        {
            _ui.WriteLine(errors.Count > 0 ? string.Join(" ", errors) : $"Максимум не может быть больше {AbsoluteMaxNumber}.");
            return;
        }

        _settings = candidate;
        _ui.WriteLine("Настройки изменены.");
    }

    private void LoadSettings()
    {
        var path = ReadPath("Путь к файлу настроек (Enter — settings.json): ", _settingsFilePath);
        try
        {
            _settings = new JsonSettingsStore(path).Load();
            _settingsFilePath = path;
            _ui.WriteLine("Настройки загружены.");
        }
        catch (Exception exception) when (exception is IOException or JsonException or InvalidDataException)
        {
            _ui.WriteLine($"Не удалось загрузить настройки: {exception.Message}");
        }
    }

    private void ExportSettings()
    {
        var path = ReadPath("Путь для экспорта настроек (Enter — settings.json): ", _settingsFilePath);
        try
        {
            new JsonSettingsStore(path).Save(_settings);
            _settingsFilePath = path;
            _ui.WriteLine("Настройки экспортированы.");
        }
        catch (Exception exception) when (exception is IOException or ArgumentException)
        {
            _ui.WriteLine($"Не удалось сохранить настройки: {exception.Message}");
        }
    }

    private void StatisticsMenu()
    {
        while (true)
        {
            _ui.WriteLine("\n=== СТАТИСТИКА ===");
            PrintStatistics();
            _ui.WriteLine("1. Загрузить статистику из файла");
            _ui.WriteLine("2. Экспортировать статистику в файл");
            _ui.WriteLine("3. Назад");

            switch (_ui.ReadLine("Выберите пункт: "))
            {
                case "1": LoadStatistics(); break;
                case "2": ExportStatistics(); break;
                case "3": return;
                default: _ui.WriteLine("Неизвестный пункт меню."); break;
            }
        }
    }

    private void LoadStatistics()
    {
        var path = ReadPath("Путь к файлу статистики (Enter — statistics.json): ", _statisticsFilePath);
        try
        {
            _statistics = new JsonStatisticsStore(path).Load();
            _statisticsFilePath = path;
            _ui.WriteLine("Статистика загружена.");
        }
        catch (Exception exception) when (exception is IOException or JsonException)
        {
            _ui.WriteLine($"Не удалось загрузить статистику: {exception.Message}");
        }
    }

    private void ExportStatistics()
    {
        var path = ReadPath("Путь для экспорта статистики (Enter — statistics.json): ", _statisticsFilePath);
        try
        {
            new JsonStatisticsStore(path).Save(_statistics);
            _statisticsFilePath = path;
            _ui.WriteLine("Статистика экспортирована.");
        }
        catch (IOException exception)
        {
            _ui.WriteLine($"Не удалось сохранить статистику: {exception.Message}");
        }
    }

    private void SaveStatistics(string path, bool showMessage)
    {
        try
        {
            new JsonStatisticsStore(path).Save(_statistics);
            if (showMessage) _ui.WriteLine("Статистика сохранена.");
        }
        catch (IOException exception)
        {
            _ui.WriteLine($"Не удалось сохранить статистику: {exception.Message}");
        }
    }

    private void PrintStatistics()
    {
        _ui.WriteLine($"Игр: {_statistics.GamesPlayed}");
        _ui.WriteLine($"Побед: {_statistics.Wins}");
        _ui.WriteLine($"Поражений: {_statistics.Losses}");
        _ui.WriteLine($"Среднее число попыток в победах: {_statistics.AverageAttemptsInWins:F1}");
        _ui.WriteLine($"Лучший результат: {_statistics.BestAttempts?.ToString() ?? "—"}");
    }

    private int ReadInt(string prompt)
    {
        while (true)
        {
            var value = _ui.ReadLine(prompt);
            if (int.TryParse(value, out var number)) return number;
            _ui.WriteLine("Введите целое число.");
        }
    }

    private Difficulty ReadDifficulty()
    {
        while (true)
        {
            var value = _ui.ReadLine("Сложность (1 — лёгкая, 2 — средняя, 3 — сложная): ");
            if (int.TryParse(value, out var number) && number is >= 1 and <= 3)
            {
                return (Difficulty)(number - 1);
            }

            _ui.WriteLine("Выберите число от 1 до 3.");
        }
    }

    private string ReadPath(string prompt, string defaultPath)
    {
        var value = _ui.ReadLine(prompt);
        return string.IsNullOrWhiteSpace(value) ? defaultPath : value.Trim();
    }
}
