using System.Text.Json;

namespace GuessNumber;

/// <summary>Хранилище статистики в JSON-файле.</summary>
public sealed class JsonStatisticsStore : IStatisticsStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    /// <summary>Создаёт хранилище по указанному пути.</summary>
    public JsonStatisticsStore(string filePath) => _filePath = filePath;

    /// <inheritdoc />
    public Statistics Load()
    {
        if (!File.Exists(_filePath))
            return Statistics.Empty;
        try
        {
            return JsonSerializer.Deserialize<Statistics>(File.ReadAllText(_filePath)) ?? Statistics.Empty;
        }
        catch (JsonException)
        {
            return Statistics.Empty;
        }
    }

    /// <inheritdoc />
    public void Save(Statistics statistics)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(statistics, _options));
    }
}

/// <summary>Обновляет статистику по итогам игры.</summary>
public sealed class StatisticsService
{
    private readonly IStatisticsStore _store;

    /// <summary>Создаёт сервис с хранилищем.</summary>
    public StatisticsService(IStatisticsStore store) => _store = store;

    /// <summary>Загружает текущую статистику.</summary>
    public Statistics GetCurrent() => _store.Load();

    /// <summary>Добавляет результат игры и сохраняет новую статистику.</summary>
    public Statistics Register(GameSummary summary)
    {
        var current = _store.Load();
        var score = summary.Won ? Math.Max(1, 1000 - summary.AttemptsUsed * 10) : 0;
        var bestScore = Math.Max(current.BestScore, score);
        var updated = new Statistics(current.GamesPlayed + 1, current.GamesWon + (summary.Won ? 1 : 0), bestScore);
        _store.Save(updated);
        return updated;
    }
}
