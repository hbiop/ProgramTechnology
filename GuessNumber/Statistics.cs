using System.Text.Json;

namespace GuessNumber;

/// <summary>Статистика игрока.</summary>
public sealed record Statistics(int GamesPlayed, int GamesWon)
{
    /// <summary>Возвращает статистику с учётом завершённой игры.</summary>
    public Statistics WithGame(bool won) => new(GamesPlayed + 1, GamesWon + (won ? 1 : 0));
}

/// <summary>Абстракция хранилища статистики.</summary>
public interface IStatisticsStore
{
    /// <summary>Загружает статистику.</summary>
    Statistics Load();

    /// <summary>Сохраняет статистику.</summary>
    void Save(Statistics statistics);
}

/// <summary>Хранит статистику в JSON-файле.</summary>
public sealed class JsonStatisticsStore(string filePath) : IStatisticsStore
{
    /// <inheritdoc />
    public Statistics Load()
    {
        if (!File.Exists(filePath))
            return new Statistics(0, 0);

        return JsonSerializer.Deserialize<Statistics>(File.ReadAllText(filePath))
               ?? throw new InvalidDataException($"Не удалось прочитать статистику из {filePath}.");
    }

    /// <inheritdoc />
    public void Save(Statistics statistics) =>
        File.WriteAllText(filePath, JsonSerializer.Serialize(statistics, new JsonSerializerOptions { WriteIndented = true }));
}
