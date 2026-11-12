using System.Text.Json;
using GuessNumberGame.Domain.Models;

namespace GuessNumberGame.Domain.Services;

public sealed class JsonSettingsStore : ISettingsStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonSettingsStore(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
    }

    public GameSettings Load()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("Файл настроек не найден.", _filePath);
        }

        var settings = JsonSerializer.Deserialize<GameSettings>(File.ReadAllText(_filePath));
        if (settings is null)
        {
            throw new InvalidDataException("Файл настроек пуст.");
        }

        var errors = settings.Validate();
        if (errors.Count > 0)
        {
            throw new InvalidDataException(string.Join(" ", errors));
        }

        return settings;
    }

    public void Save(GameSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var errors = settings.Validate();
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" ", errors), nameof(settings));
        }

        var directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_filePath, JsonSerializer.Serialize(settings, _options));
    }
}
