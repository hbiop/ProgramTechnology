using System.Text.Json;
using GuessNumberGame.Domain.Models;
using GuessNumberGame.Domain.RandomNumberGenerator;
using GuessNumberGame.Domain.Services;

namespace GuessTheNumber.View;

internal static class Program
{
    public static void Main()
    {
        var ui = new ConsoleUserInterface();
        var settings = LoadOrDefaultSettings();
        var statistics = LoadOrDefaultStatistics();
        var application = new GameApplication(
            ui,
            new RandomNumberGenerator(),
            settings,
            statistics);

        application.Run();
    }

    private static GameSettings LoadOrDefaultSettings()
    {
        try
        {
            return new JsonSettingsStore("settings.json").Load();
        }
        catch (Exception exception) when (exception is IOException or JsonException or InvalidDataException)
        {
            return new GameSettings();
        }
    }

    private static PlayerStatistics LoadOrDefaultStatistics()
    {
        try
        {
            return new JsonStatisticsStore("statistics.json").Load();
        }
        catch (Exception exception) when (exception is IOException or JsonException or InvalidDataException)
        {
            return new PlayerStatistics();
        }
    }
}
