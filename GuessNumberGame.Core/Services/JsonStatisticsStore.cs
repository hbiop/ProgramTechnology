using System.Text.Json;
using GuessNumberGame.Domain.Models;

namespace GuessNumberGame.Domain.Services;

public sealed class JsonStatisticsStore : IStatisticsStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonStatisticsStore(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
    }

    public PlayerStatistics Load()
    {
        if (!File.Exists(_filePath))
        {
            return new PlayerStatistics();
        }

        try
        {
            return JsonSerializer.Deserialize<PlayerStatistics>(File.ReadAllText(_filePath))
                ?? new PlayerStatistics();
        }
        catch (JsonException)
        {
            return new PlayerStatistics();
        }
    }

    public void Save(PlayerStatistics statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        var directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_filePath, JsonSerializer.Serialize(statistics, _options));
    }
}
